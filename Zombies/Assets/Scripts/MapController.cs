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
        enemiesKilled = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (enemiesKilled == 4)
        {
            RoundNumber++;
        }
        if (RoundNumber == 1)
        {
            SpawnRate = 1;
            SpawnEnemies();
        }
        if (RoundNumber == 2)
        {
            SpawnRate = 2;
            SpawnEnemies();
        }
    }
    private void SpawnEnemies()
    {
        if (CanSpawn == true)
        {
            if (SpawnIndex < 4)
            {
                //instantiates enemy at spawn index, starts cooldown, and adds one to spawn index
                GameObject Enemy = Instantiate(enemy1, spawns[SpawnIndex].transform.position, spawns[SpawnIndex].transform.rotation);
                StartCoroutine(EnemySpawnCooldown());
                SpawnIndex++;
            }
        }
    }
    /// <summary>
    /// creates cooldown between enemy spawns based on spawn rate
    /// </summary>
    /// <returns></returns>
    IEnumerator EnemySpawnCooldown()
    {
        CanSpawn = false;
        yield return new WaitForSeconds(11 - SpawnRate);
        CanSpawn = true;
    }
}
