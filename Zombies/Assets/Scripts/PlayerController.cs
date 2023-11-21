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
    public bool takesDamage;
    public bool hasWeapon1;
    public bool hasWeapon2;
    public bool hasWeapon3;
    Camera mainCamera;
    public GameObject hurt1;
    public GameObject hurt2;
    public GameObject hurt3;
    public GameObject noWeapon;
    public GameObject weapon1;
    public GameObject weapon2;
    public GameObject weapon3;

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
                    //for weapon 1 
                    if (hasWeapon1 == true)
                    {
                        if (smallAmmo >= 1)
                        {
                            //put reference to enemy health in enemy script
                            //subtract amount from health  
                            smallAmmo -= 1;
                            Debug.Log("bullet hit enemy.");
                        }
                        else
                        {
                            //put weapon switch here 
                            Debug.Log("no bullets for weapon1.");
                        }
                    }
                    //for weapon 2
                    if (hasWeapon2 == true)
                    {
                        if (medAmmo >= 1)
                        {
                            //put reference to enemy health in enemy script
                            //put subtract amount from health  
                            medAmmo -= 1;
                            Debug.Log("bullet hit enemy.");
                        }
                        else
                        {
                            //put weapon switch here 
                            Debug.Log("no bullets for weapon2.");
                        }
                    }
                    //for weapon 3
                    if (hasWeapon3 == true)
                    {
                        if (bigAmmo >= 1)
                        {
                            //put reference to enemy health in enemy script
                            //put subtract amount from health  
                            bigAmmo -= 1;
                            Debug.Log("bullet hit enemy.");
                        }
                        else
                        {
                            //put weapon switch here 
                            Debug.Log("no bullets for weapon3.");
                        }
                    }
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
    private void Die()
    {
        //put game over screen load, probably SceneManager.LoadScene(3); 
        Debug.Log("Player died.");
    }
    /// <summary>
    /// gives player temporary invincibility 
    /// </summary>
    /// <returns></returns>
    IEnumerator Damageable()
    {
        takesDamage = false;
        yield return new WaitForSeconds(1);
        takesDamage = true;
    }
    /// <summary>
    /// regens health back to full over time 
    /// </summary>
    /// <returns></returns>
    IEnumerator Regen()
    {
        while (true)
        {
            if (health <= 30)
            {
                //adds health for each frame with .3 second delay 
                health++;
                yield return new WaitForSeconds(.3f);
            }
            yield return new WaitForEndOfFrame();
        }
    }
}
