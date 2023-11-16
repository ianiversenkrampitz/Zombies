using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Iversen-Krampitz, Ian
//11/16/2023
//Controls the player, collision, guns. 

public class PlayerController : MonoBehaviour
{
    public float speed;
    public float health;
    public int smallAmmo;
    public int medAmmo;
    public int bigAmmo;
    public bool IsDown;
    Camera mainCamera;
    public GameObject hurt1;
    public GameObject hurt2;
    public GameObject hurt3;
    public bool takesDamage;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
        hurt1.SetActive(false);
        hurt2.SetActive(false);
        hurt3.SetActive(false);
        health = 31;
        takesDamage = true;
        StartCoroutine(Regen());
    }

    // Update is called once per frame
    void Update()
    {
        if (health >= 11)
        {
            hurt3.SetActive(false);
        }
        if (health >= 21)
        {
            hurt2.SetActive(false);
            hurt3.SetActive(false);
        }
        if (health >= 31)
        {
            hurt1.SetActive(false);
            hurt2.SetActive(false);
            hurt3.SetActive(false);
        }
        //moves the player 
        if (Input.GetKey(KeyCode.A))
        {
            transform.position += -transform.right * speed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.position += transform.right * speed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.W))
        {
            transform.position += transform.forward * speed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.position += -transform.forward * speed * Time.deltaTime;
        }
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Debug.Log("Player fired.");
            FireWeapon();
        }
        Debug.DrawLine(mainCamera.transform.position, mainCamera.transform.position + mainCamera.transform.forward * 200f);
    }
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            TakeDamage();
            Debug.Log("Player hit an enemy.");
        }
    }
    /// <summary>
    /// fires the weapon and deals appropriate damage, subtracts ammo 
    /// </summary>
    private void FireWeapon()
    {
        RaycastHit hit;
        {
            if (Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward, out hit, 200f))
            {
                if (hit.collider.tag == "Environment")
                {
                    //whatever goes here will make a bullethole in the wall depending on the weapon being used 
                    Debug.Log("bullet hit wall");
                }
                else if (hit.collider.tag == "Enemy")
                {
                    //whatever goes here should damage the enemy based on the weapon being used
                    Debug.Log("bullet hit enemy.");
                }
            }
        }
    }
    /// <summary>
    /// calculates damage to player and sets appropriate damage indicator
    /// </summary>
    private void TakeDamage()
    {
        if (takesDamage == true)
        {
            //subtracts ten health 
            health -= 10;
            //kills player if at or below zero 
            if (health <= 0)
            {
                Die();
            }
            //puts damage indicator on screen based on health 
            if (health >= 20 && health <= 30)
            {
                hurt1.SetActive(true);
            }
            if (health >= 10 && health <= 20)
            {
                hurt2.SetActive(true);
            }
            if (health >= 0 && health <= 10)
            {
                hurt3.SetActive(true);
            }
            StartCoroutine(Damageable());
        }
    }
    private void DealDamage()
    {

    }
    private void Die()
    {
        //whatever goes here will make the enemy load the game over screen 
        Debug.Log("Player died.");
    }
    /// <summary>
    /// gives player temp invincibility 
    /// </summary>
    /// <returns></returns>
    IEnumerator Damageable()
    {
        takesDamage = false;
        yield return new WaitForSeconds(1);
        takesDamage = true;
    }
    IEnumerator Regen()
    {
        while (true)
        {
            if (health <= 30)
            {
                health++;
                yield return new WaitForSeconds(.3f);
            }

            yield return new WaitForEndOfFrame();

        }
    }
}
