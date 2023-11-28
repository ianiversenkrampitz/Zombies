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
    public bool noGun;
    public bool usingWeapon1;
    public bool usingWeapon2;
    public bool usingWeapon3;
    public GameObject hurt1;
    public GameObject hurt2;
    public GameObject hurt3;
    public GameObject noWeapon;
    public GameObject weapon1;
    public GameObject weapon2;
    public GameObject weapon3;
    public int score;
    public int totalScore;
    public float dificultyLevel;
    public Animation gunAnimation;
    Camera mainCamera;

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
        usingWeapon1 = true;
        usingWeapon2 = false;
        usingWeapon3 = false;
    }

    // Update is called once per frame
    void Update()
    {
        //gets rid of appropriate damage indicators by health amount
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
        //fires gun 
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Debug.Log("Player fired.");
            FireWeapon();
        }
        //switches weapons 
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (smallAmmo >= 1)
            {
                SwitchWeapon1();
            }
            else
            {
                Debug.Log("No ammo for weapon1.");
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (medAmmo >= 1)
            {
                SwitchWeapon2();
            }
            else
            {
                Debug.Log("No ammo for weapon2.");
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            if (bigAmmo >= 1)
            {
                SwitchWeapon3();
            }
            else
            {
                Debug.Log("No ammo for weapon3.");
            }
        }
    }
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            TakeDamage();
            Debug.Log("Player hit an enemy.");
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Weapon1")
        {
            //autoswitches to weapon 1 if player didn't previously have it 
            if (hasWeapon1 == false)
            {
                SwitchWeapon1();
                hasWeapon1 = true;
            }
            smallAmmo += 15;
            other.gameObject.SetActive(false);
            Debug.Log("Picked up weapon1.");
        }
        if (other.gameObject.tag == "Weapon2")
        {
            //autoswitches to weapon 2 if player didn't previously have it 
            if (hasWeapon2 == false)
            {
                SwitchWeapon2();
                hasWeapon2 = true;
            }
            medAmmo += 15;
            other.gameObject.SetActive(false);
            Debug.Log("Picked up weapon2.");
        }
        if (other.gameObject.tag == "Weapon3")
        {
            //autoswitches to weapon 3 if player didn't previously have it 
            if (hasWeapon3 == false)
            {
                SwitchWeapon3();
                hasWeapon3 = true;
            }
            bigAmmo += 15;
            other.gameObject.SetActive(false);
            Debug.Log("Picked up weapon3.");
        }
        if (other.gameObject.tag == "SmallAmmo")
        {
            smallAmmo += 10;
            if (noGun == true && hasWeapon1 == true)
            {
                usingWeapon1 = true;
            }
            other.gameObject.SetActive(false);
        }
        if (other.gameObject.tag == "MedAmmo")
        {
            medAmmo += 10;
            if (noGun == true && hasWeapon2 == true)
            {
                usingWeapon2 = true;
            }
            other.gameObject.SetActive(false);
        }
        if (other.gameObject.tag == "BigAmmo")
        {
            bigAmmo += 10;
            if (noGun == true && hasWeapon3 == true)
            {
                usingWeapon3 = true;
            }
            other.gameObject.SetActive(false);
        }
    }
    /// <summary>
    /// fires the weapon and deals appropriate damage, subtracts ammo 
    /// </summary>
    private void FireWeapon()
    {
        //temporary gun animation, shouldn't actually be placed here when complete
        gunAnimation.Play("GunTestAnimation");
        RaycastHit hit;
        {
            if (Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward, out hit, 200f))
            {
                if (hit.collider.tag == "Environment")
                {
                    //for weapon 1 
                    if (hasWeapon1 == true && usingWeapon1 == true)
                    {
                        if (smallAmmo >= 1)
                        {
                            //whatever goes here will make a bullethole in the wall depending on the weapon being used 
                            smallAmmo -= 1;
                            Debug.Log("bullet hit wall.");
                        }
                        else
                        {
                            AutoSwitch();
                            Debug.Log("no bullets for weapon1.");
                        }
                    }
                    //for weapon 2
                    if (hasWeapon2 == true && usingWeapon2 == true)
                    {
                        if (medAmmo >= 1)
                        {
                            //whatever goes here will make a bullethole in the wall depending on the weapon being used   
                            medAmmo -= 1;
                            Debug.Log("bullet hit wall.");
                        }
                        else
                        {
                            AutoSwitch();
                            Debug.Log("no bullets for weapon2.");
                        }
                    }
                    //for weapon 3
                    if (hasWeapon3 == true && usingWeapon3 == true)
                    {
                        if (bigAmmo >= 1)
                        {
                            //whatever goes here will make a bullethole in the wall depending on the weapon being used 
                            bigAmmo -= 1;
                            Debug.Log("bullet hit enemy.");
                        }
                        else
                        {
                            AutoSwitch();
                            Debug.Log("no bullets for weapon3.");
                        }
                    }
                }
                if (hit.collider.tag == "Enemy")
                {
                    //for weapon 1 
                    if (hasWeapon1 == true && usingWeapon1 == true)
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
                            AutoSwitch();
                            Debug.Log("no bullets for weapon1.");
                        }
                    }
                    //for weapon 2
                    if (hasWeapon2 == true && usingWeapon2 == true)
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
                            AutoSwitch();
                            Debug.Log("no bullets for weapon2.");
                        }
                    }
                    //for weapon 3
                    if (hasWeapon3 == true && usingWeapon3 == true)
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
                            AutoSwitch();
                            Debug.Log("no bullets for weapon3.");
                        }
                    }
                }
            }
            else
            {
                //for weapon 1 
                if (hasWeapon1 == true && usingWeapon1 == true)
                {
                    if (smallAmmo >= 1)
                    {
                        smallAmmo -= 1;
                        Debug.Log("bullet hit nothing.");
                    }
                    else
                    {
                        AutoSwitch();
                        Debug.Log("no bullets for weapon1.");
                    }
                }
                //for weapon 2
                if (hasWeapon2 == true && usingWeapon2 == true)
                {
                    if (medAmmo >= 1)
                    {
                        medAmmo -= 1;
                        Debug.Log("bullet hit nothing.");
                    }
                    else
                    {
                        AutoSwitch();
                        Debug.Log("no bullets for weapon2.");
                    }
                }
                //for weapon 3
                if (hasWeapon3 == true && usingWeapon3 == true)
                {
                    if (bigAmmo >= 1)
                    {
                        bigAmmo -= 1;
                        Debug.Log("bullet hit nothing.");
                    }
                    else
                    {
                        AutoSwitch();
                        Debug.Log("no bullets for weapon3.");
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
            //damage indicator on screen based on health 
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
    /// <summary>
    /// switches to weapon 1 
    /// </summary>
    private void SwitchWeapon1()
    {
        usingWeapon1 = true;
        usingWeapon2 = false;
        usingWeapon3 = false;
        noGun = false;
        Debug.Log("Switched to weapon1.");
    }
    /// <summary>
    /// switches to weapon 2 
    /// </summary>
    private void SwitchWeapon2()
    {
        usingWeapon2 = true;
        usingWeapon3 = false;
        usingWeapon1 = false;
        noGun = false;
        Debug.Log("Switched to weapon2.");
    }
    /// <summary>
    /// switches to weapon 3 
    /// </summary>
    private void SwitchWeapon3()
    {
        usingWeapon3 = true;
        usingWeapon2 = false;
        usingWeapon1 = false;
        noGun = false;
        Debug.Log("Switched to weapon3.");
    }
    /// <summary>
    /// switches to no weapon
    /// </summary>
    private void SwitchWeaponNone()
    {
        usingWeapon3 = false;
        usingWeapon2 = false;
        usingWeapon1 = false;
        noGun = true;
        Debug.Log("Switched to no weapon.");
    }
    /// <summary>
    /// automatically switches to weapon with ammo
    /// </summary>
    private void AutoSwitch()
    {
        if (hasWeapon1 && smallAmmo >= 1)
        {
            SwitchWeapon1();
        }
        else if (hasWeapon2 && medAmmo >= 1)
        {
            SwitchWeapon2();
        }
        else if (hasWeapon3 && bigAmmo >= 1)
        {
            SwitchWeapon3();
        }
        else
        {
            SwitchWeaponNone();
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
