using System.Collections;
using System.Collections.Generic;
using System.Xml.Schema;
using UnityEngine;

public class CaveMechanic : MonoBehaviour
{
    [SerializeField] private GameObject monsterPrefab;
    [SerializeField] private Transform spawnLocation;
    [SerializeField] private Transform spawnContainer;

    private float spawnLimit = 10f;
    public float totalSpawn;

    public float spawnRate = 10f;
    public float spawnTimer;

    private float resetRate = 300f;
    public float resetTimer;

    public LayerMask whatIsPlayer;
    public float sightRange;
    public bool playerInSightRange;
    void Start()
    {
        
    }

    void Update()
    {
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        if(playerInSightRange && totalSpawn < spawnLimit)
        {
            resetTimer = 0f;
            spawnTimer += Time.deltaTime;
            if(spawnTimer >= spawnRate)
            {
                SpawnMonster();
                spawnTimer = 0f;
            }
        }
        else
        {
            resetTimer += Time.deltaTime;
            if(resetTimer >= resetRate)
            {
                totalSpawn = 0f;
                spawnTimer = 0f;
            }
        }
    }

    public void SpawnMonster()
    {
        if(totalSpawn < spawnLimit)
        {
            totalSpawn++;
            GameObject monster = Instantiate(monsterPrefab, spawnLocation.position, spawnLocation.rotation);
            monster.transform.SetParent(spawnContainer);
            EnemyAI enemyAI = monster.GetComponent<EnemyAI>();
            enemyAI.AssignCave(this);
        }
        

    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}
