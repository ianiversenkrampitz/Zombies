using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Iversen-Krampitz, Ian 
//11/30/2023
//controls the enemy spawns and rate, changes waves 

public class MapController : MonoBehaviour
{
    public int RoundNumber;
    public int enemiesKilled;
    public int enemiesInWave;
    public GameObject enemy1;
    public GameObject enemy2;
    public GameObject[] spawns;
    public bool CanSpawn;
    public int SpawnRate;
    public int SpawnIndex;

    // Start is called before the first frame update
    void Start()
    {
        RoundNumber = 1;
        CanSpawn = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (RoundNumber == 1)
        {
            SpawnRate = 1;
            SpawnEnemies();
        }
    }
    private void SpawnEnemies()
    {
        if (CanSpawn == true)
        {
            GameObject Enemy = Instantiate(enemy1, spawns[SpawnIndex].transform.position, spawns[SpawnIndex].transform.rotation);
            StartCoroutine(EnemySpawnCooldown());
            SpawnIndex++;
        }
    }
    IEnumerator EnemySpawnCooldown()
    {
        CanSpawn = false;
        yield return new WaitForSeconds(11 - SpawnRate);
        CanSpawn = true;
    }
}
