using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEditor;
using UnityEngine;
//Iversen-Krampitz, Ian 
//12/5/2023
//controls enemies. 

public class Enemy : MonoBehaviour
{
    public bool CanAttack;
    public int aggression;
    public int speed;
    public float health;
    public float jumpForce;
    private float playerX;
    private float playerZ;
    private float playerY;
    public MapController mapController;
    public PlayerController playerController;
    private Rigidbody rb;
    public Transform player;
    public GameObject Player;
    public GameObject smallAmmo;
    public GameObject medAmmo;
    public GameObject bigAmmo;
    // Start is called before the first frame update
    void Start()
    {
        CanAttack = true;
        rb = GetComponent<Rigidbody>();
        speed = 3;
        jumpForce = 7;
    }

    // Update is called once per frame
    void Update()
    {
        //find enemy's current position 
        if (CanAttack == true)
        {
            //get the position of player and enemy
            Vector3 currentPosition = transform.position;
            Vector3 direction = player.position - transform.position;
            //set the playerX, playerY, playerZ
            playerX = Player.transform.position.x;
            playerY = Player.transform.position.y;
            playerZ = Player.transform.position.z;
            direction.Normalize();
            //move the enemy towards the player on x and z axes 
            float xMovement = direction.x * (speed + aggression) * Time.deltaTime;
            float zMovement = direction.z * (speed + aggression) * Time.deltaTime;
            transform.Translate(new Vector3(xMovement, 0f, zMovement));
            //makes enemy jump if it's close enough to the player on x and z but the player is above it
            if (playerY > currentPosition.y + 2 && currentPosition.x + 2 > playerX && playerX > currentPosition.x - 2 && currentPosition.z + 2 > playerZ && playerZ > currentPosition.z - 2)
            {
                Jump();
                StartCoroutine(WaitToJump());
            }
        }
        if (health <= 0)
        {
            Die();
        }
    }
    /// <summary>
    /// kills enemy and adds to enemies killed count 
    /// </summary>
    private void Die()
    {
        gameObject.SetActive(false);
        mapController.enemiesKilled++;
        int randomDrop = Random.Range(0, 3);
        if (randomDrop == 0)
        {
            Instantiate(smallAmmo, transform.position, transform.rotation);
        }
        if (randomDrop == 1)
        {
            Instantiate(medAmmo, transform.position, transform.rotation);
        }
        if (randomDrop == 2)
        {
            Instantiate(bigAmmo, transform.position, transform.rotation);
        }
    }
    private void Jump()
    {
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }
    /// <summary>
    /// workaround to use this ienumerator in a different script 
    /// </summary>
    public void StartAttackCooldown()
    {
        StartCoroutine(WaitToAttack());
    }
    IEnumerator WaitToAttack()
    {
        CanAttack = false;
        yield return new WaitForSeconds(1f);
        CanAttack = true;
    }
    IEnumerator WaitToJump()
    {
        CanAttack = false;
        yield return new WaitForSeconds(.5f);
        CanAttack = true;
    }
}
