using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class DungeonEnterTrigger : MonoBehaviour
{
    public enum DungeonMode
    {
        normal,
        miniboss,
        boss
    }

    [Header("Monster Mode")]
    public GameObject monsterRangePrefab;
    public GameObject monsterMeleePrefab;
    public Transform[] monsterRangeSpawnLocation;
    public Transform[] monsterMeleeSpawnLocation;

    [Header("Miniboss Mode")]
    public GameObject minibossPrefab;
    public Transform minibossSpawnLocation;
    public string minibossName;

    [Header("Boss Mode")]
    public GameObject bossPrefab;
    public Transform bossSpawnLocation;
    public string bossName;

    [Header("Everything Else")]
    [SerializeField] private GameObject bossHealthDisplay;
    [SerializeField] private RectTransform healthDisplay;
    [SerializeField] private RectTransform healthBGDisplay;
    [SerializeField] private TMP_Text bossNameDisplay;
    public Collider[] boundaries;
    public DungeonMode dungeonMode;
    public bool alreadyTriggered;
    public int totalDeath;
    public int deathNeeded;
    public bool allMonsterKilled = false;
    public UnityEvent onEnter;
    public UnityEvent onAllMonsterDeath;
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(totalDeath == deathNeeded && !allMonsterKilled)
        {
            allMonsterKilled = true;
            onAllMonsterDeath?.Invoke();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (alreadyTriggered)
        {
            return;
        }
        Debug.Log("lah kocak");
        if (other.gameObject.CompareTag("Player"))
        {
            onEnter?.Invoke();
            Debug.Log("Berhasil");
            alreadyTriggered = true;
        }
        
    }

    public void ActivateBoundaries()
    {
        foreach(Collider c in boundaries)
        {
            c.isTrigger = false;
        }
    }

    public void DeactivateBoundaries()
    {
        foreach (Collider c in boundaries)
        {
            c.isTrigger = true;
        }
    }

    public void ActivateBossCanvas()
    {
        LevelManager.instance.bossUI.gameObject.SetActive(true);   
    }
    
    public void SpawnEnemy()
    {
        if(dungeonMode == DungeonMode.normal)
        {
            foreach (Transform transform in monsterMeleeSpawnLocation)
            {
                EnemyAI enemy = Instantiate(monsterMeleePrefab, transform.position, Quaternion.identity).GetComponent<EnemyAI>();
                enemy.dungeonSource = this;
            }
            foreach (Transform transform in monsterRangeSpawnLocation)
            {
                EnemyAI enemy = Instantiate(monsterRangePrefab, transform.position, Quaternion.identity).GetComponent<EnemyAI>();
                enemy.dungeonSource = this;
            }
        }
        else if(dungeonMode == DungeonMode.miniboss)
        {
            bossHealthDisplay.SetActive(true);
            BossBase boss = Instantiate(minibossPrefab,minibossSpawnLocation.position, Quaternion.identity).GetComponent<BossBase>();
            boss.dungeonSource = this;
            boss.healthDisplay = healthDisplay;
            boss.healthBGDisplay = healthBGDisplay;
            bossNameDisplay.text = minibossName;
        }
        else
        {
            bossHealthDisplay.SetActive(true);
            BossBase boss = Instantiate(bossPrefab, bossSpawnLocation.position, Quaternion.identity).GetComponent<BossBase>();
            boss.dungeonSource = this;
            boss.healthDisplay = healthDisplay;
            boss.healthBGDisplay = healthBGDisplay;
            bossNameDisplay.text = bossName;
        }
        
    }
    public void ChangeMiniBossBGM()
    {
        //AudioManager.Instance.musicSource.Stop();
        AudioManager.Instance.musicSource.clip = AudioManager.Instance.miniboss;
        AudioManager.Instance.musicSource.Play();
    }

    public void ChangeBossBGM()
    {
        //AudioManager.Instance.musicSource.Stop();
        AudioManager.Instance.musicSource.clip = AudioManager.Instance.boss;
        AudioManager.Instance.musicSource.Play();
    }

    public void ChangeNormalBGM()
    {
        //AudioManager.Instance.musicSource.Stop();
        AudioManager.Instance.musicSource.clip = AudioManager.Instance.normal;
        AudioManager.Instance.musicSource.Play();
    }


    public void TurnoffBossUI()
    {
        bossHealthDisplay.SetActive(false);
    }
}
