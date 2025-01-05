using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DungeonEnterTrigger : MonoBehaviour
{
    public enum DungeonMode
    {
        normal,
        boss
    }

    [Header("Monster Mode")]
    public GameObject monsterRangePrefab;
    public GameObject monsterMeleePrefab;
    public Transform[] monsterRangeSpawnLocation;
    public Transform[] monsterMeleeSpawnLocation;
    
    
    [Header("Boss Mode")]
    public GameObject bossPrefab;
    public Transform bossSpawnLocation;
    public BossSuki bossSuki;

    [Header("Everything Else")]
    public Collider[] boundaries;
    public DungeonMode dungeonMode;
    public bool alreadyTriggered;
    public int totalDeath;
    public int deathNeeded;
    public UnityEvent onEnter;
    public UnityEvent onAllMonsterDeath;
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(totalDeath == deathNeeded)
        {
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

    public void ActivateBossScript()
    {
        bossSuki.enabled = true;
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
        else
        {
            BossBase boss = Instantiate(bossPrefab,bossSpawnLocation.position, Quaternion.identity).GetComponent<BossBase>();
        }
        
    }

    public void MinibossBeat()
    {
        
    }
}
