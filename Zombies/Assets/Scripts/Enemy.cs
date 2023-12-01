using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Iversen-Krampitz, Ian 
//11/30/2023
//controls enemies. 

public class Enemy : MonoBehaviour
{
    public int health;
    public MapController mapController;

    // Start is called before the first frame update
    void Start()
    {
        health = 20;
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
        {
            Die();
        }
    }
    private void Die()
    {
        gameObject.SetActive(false);
        mapController.enemiesKilled++;
    }
}
