using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    public GameObject monsterPrefab;
    public Transform spawnLocation;
    public GameObject monsterParent;
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnMonster()
    {
        Debug.Log("Monster Spawned");
        Instantiate(monsterPrefab, spawnLocation.position, Quaternion.identity, monsterParent.transform);
    }
}
