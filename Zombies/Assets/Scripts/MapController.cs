using System.Collections;
using System.Collections.Generic;
using System.Data;
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
    public GameObject Player;
    public GameObject[] spawns;
    public bool CanSpawn;
    public int SpawnRate;
    public int SpawnIndex;
    public int numberOfSpawns;
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
            RoundNumber = 2;
        }
        if (RoundNumber == 1)
        {
            SpawnRate = 1;
            numberOfSpawns = 4;
            SpawnEnemies();
        }
        if (RoundNumber == 2)
        {
            SpawnRate = 2;
            numberOfSpawns = 7;
            SpawnEnemies();
        }
    }
    private void SpawnEnemies()
    {
        if (CanSpawn == true)
        {
            if (SpawnIndex <= numberOfSpawns)
            {
                if (SpawnIndex == 3)
                {
                    //instantiates enemy at spawn index, starts cooldown, and adds one to spawn index
                    GameObject enemyPrefab = Instantiate(enemy1, spawns[SpawnIndex].transform.position, spawns[SpawnIndex].transform.rotation);
                    Enemy enemyScript = enemyPrefab.GetComponent<Enemy>();
                    enemyScript.Player = Player;
                    enemyScript.player = Player.transform;
                    enemyScript.mapController = this;
                    StartCoroutine(EnemySpawnCooldown());
                    SpawnIndex = 1;
                }
                else
                {
                    //instantiates enemy at spawn index, starts cooldown, and adds one to spawn index
                    GameObject enemyPrefab = Instantiate(enemy1, spawns[SpawnIndex].transform.position, spawns[SpawnIndex].transform.rotation);
                    Enemy enemyScript = enemyPrefab.GetComponent<Enemy>();
                    enemyScript.Player = Player;
                    enemyScript.player = Player.transform;
                    enemyScript.mapController = this;
                    StartCoroutine(EnemySpawnCooldown());
                    SpawnIndex++;
                }
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
