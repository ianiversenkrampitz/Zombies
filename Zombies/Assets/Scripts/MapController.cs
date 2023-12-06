using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
//Iversen-Krampitz, Ian 
//12/5/2023
//controls the enemy spawns and rate, changes waves 

public class MapController : MonoBehaviour
{
    public int RoundNumber;
    public int enemiesKilled;
    public int enemiesInWave;
    public int SpawnRate;
    public int SpawnIndex;
    public int numberSpawnedThisRound;
    public int numberOfSpawns;
    public int aggression;
    public int health;
    public GameObject enemy1;
    public GameObject enemy2;
    public GameObject Player;
    public GameObject[] spawns;
    public Enemy enemy;
    public UI ui;
    public bool CanSpawn;
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
        if (RoundNumber == 1)
        {
            SpawnRate = 1;
            numberOfSpawns = 4;
            aggression = 1;
            health = 10;
            SpawnEnemies();
        }
        if (RoundNumber == 2)
        {
            SpawnRate = 2;
            numberOfSpawns = 7;
            aggression = 3;
            health = 12;
            SpawnEnemies();
        }
        if (RoundNumber == 3)
        {
            SpawnRate = 3;
            numberOfSpawns = 8;
            aggression = 4;
            health = 27;
            SpawnEnemies();
        }
    }
    private void SpawnEnemies()
    {
        if (CanSpawn == true)
        {
            //spawns more enemies if the number for the round hasn't been met
            if (numberSpawnedThisRound < numberOfSpawns)
            {
                if (SpawnIndex == 3)
                {
                    //instantiates enemy at spawn index, starts cooldown, and adds one to spawn index
                    GameObject enemyPrefab = Instantiate(enemy1, spawns[SpawnIndex].transform.position, spawns[SpawnIndex].transform.rotation);
                    //gets reference to objects necessary for the enemy to function 
                    Enemy enemyScript = enemyPrefab.GetComponent<Enemy>();
                    enemyScript.Player = Player;
                    enemyScript.player = Player.transform;
                    enemyScript.mapController = this;
                    enemyScript.aggression = aggression;
                    enemyScript.health = health;
                    StartCoroutine(EnemySpawnCooldown());
                    numberSpawnedThisRound++;
                    SpawnIndex = 0;
                }
                else
                {
                    //instantiates enemy at spawn index, starts cooldown, and adds one to spawn index
                    GameObject enemyPrefab = Instantiate(enemy1, spawns[SpawnIndex].transform.position, spawns[SpawnIndex].transform.rotation);
                    //gets reference to objects necessary for the enemy to function 
                    Enemy enemyScript = enemyPrefab.GetComponent<Enemy>();
                    enemyScript.Player = Player;
                    enemyScript.player = Player.transform;
                    enemyScript.mapController = this;
                    enemyScript.aggression = aggression;
                    enemyScript.health = health;
                    StartCoroutine(EnemySpawnCooldown());
                    numberSpawnedThisRound++;
                    SpawnIndex++;
                }
            }
            //changes the round to the next round if player kills all enemies
            else if (numberOfSpawns == enemiesKilled)
            {
                //resets number of enemies killed and number spawned, increases round number
                enemiesKilled = 0;
                numberSpawnedThisRound = 0;
                RoundNumber++;
                Debug.Log("New round started.");
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
