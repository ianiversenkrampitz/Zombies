using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
//Iversen-Krampitz, Ian
//11/16/2023
//Controls the player, collision, guns, UI. 

public class PlayerController : MonoBehaviour
{
    public float speed;
    public float health;
    public int smallAmmo;
    public int medAmmo;
    public int bigAmmo;
    public int score;
    public int totalScore;
    public bool IsDown;
    public bool takesDamage;
    public bool hasWeapon1;
    public bool hasWeapon2;
    public bool hasWeapon3;
    public bool usingNoGun;
    public bool usingWeapon1;
    public bool usingWeapon2;
    public bool usingWeapon3;
    public bool canFireW1;
    public bool canFireW2;
    public bool canFireW3;
    public bool switchingWeapon;
    public bool switchWeapon1;
    public bool switchWeapon2;
    public bool switchWeapon3;
    public bool switchWeaponNone;
    public Enemy enemy;
    public GameObject hurt1;
    public GameObject hurt2;
    public GameObject hurt3;
    public GameObject noWeapon;
    public GameObject weapon1;
    public GameObject weapon2;
    public GameObject weapon3;
    public Animation pistolAnimation;
    public Animation shotgunAnimation;
    public Animation arAnimation;
    private Rigidbody rb;
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
        canFireW1 = true;
        canFireW2 = true;
        canFireW3 = true;
        speed = 6;
        smallAmmo = 15;
        rb = GetComponent<Rigidbody>();
        switchingWeapon = false;
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
            transform.position += speed * Time.deltaTime * -transform.right;
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.position += speed * Time.deltaTime * transform.right;
        }
        if (Input.GetKey(KeyCode.W))
        {
            transform.position += speed * Time.deltaTime * transform.forward;
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.position += speed * Time.deltaTime * -transform.forward;
        }
        //fires gun 
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            FireWeapon();
        }
        if (Input.GetKey(KeyCode.Mouse0) && usingWeapon3 == true)
        {
            FireAutomaticWeapon();
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
            //get reference to enemy being collided with 
            GameObject thisEnemy = other.gameObject;
            Enemy EnemyScript = thisEnemy.GetComponent<Enemy>();
            if (EnemyScript.CanAttack == true)
            {
                TakeDamage();
                //start the enemy attack cooldown 
                EnemyScript.StartAttackCooldown();
                Debug.Log("Player hit an enemy.");
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Weapon1")
        {
            //autoswitches to weapon 1 if player didn't previously have it 
            if (hasWeapon1 == false || usingNoGun == true)
            {
                SwitchWeapon1();
                hasWeapon1 = true;
                usingNoGun = false;
            }
            smallAmmo += 15;
            other.gameObject.SetActive(false);
            Debug.Log("Picked up weapon1.");
        }
        if (other.gameObject.tag == "Weapon2")
        {
            //autoswitches to weapon 2 if player didn't previously have it 
            if (hasWeapon2 == false || usingNoGun == true)
            {
                SwitchWeapon2();
                hasWeapon2 = true;
                usingNoGun = false;
            }
            medAmmo += 15;
            other.gameObject.SetActive(false);
            Debug.Log("Picked up weapon2.");
        }
        if (other.gameObject.tag == "Weapon3")
        {
            //autoswitches to weapon 3 if player didn't previously have it 
            if (hasWeapon3 == false || usingNoGun == true)
            {
                SwitchWeapon3();
                hasWeapon3 = true;
                usingNoGun = false;
            }
            bigAmmo += 15;
            other.gameObject.SetActive(false);
            Debug.Log("Picked up weapon3.");
        }
        if (other.gameObject.tag == "SmallAmmo")
        {
            smallAmmo += 10;
            if (usingNoGun == true && hasWeapon1 == true)
            {
                SwitchWeapon1();
            }
            other.gameObject.SetActive(false);
        }
        if (other.gameObject.tag == "MedAmmo")
        {
            medAmmo += 10;
            if (usingNoGun == true && hasWeapon2 == true)
            {
                SwitchWeapon2();
            }
            other.gameObject.SetActive(false);
        }
        if (other.gameObject.tag == "BigAmmo")
        {
            bigAmmo += 10;
            if (usingNoGun == true && hasWeapon3 == true)
            {
                SwitchWeapon3();
            }
            other.gameObject.SetActive(false);
        }
    }
    /// <summary>
    /// fires the weapon and deals appropriate damage, subtracts ammo 
    /// </summary>
    private void FireWeapon()
    {
        if (switchingWeapon == false)
        {
            //for weapon 1 
            if (hasWeapon1 == true && usingWeapon1 == true && canFireW1 == true)
            {
                if (smallAmmo >= 1)
                {
                    //plays gun animation 
                    pistolAnimation.Play("PistolFire");
                    if (Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward, out RaycastHit hit, 200f))
                    {
                        smallAmmo -= 1;
                        if (hit.collider.tag == "Environment")
                        {
                            //insert appropriate bullet decal here
                            Debug.Log("bullet hit wall.");
                        }
                        else if (hit.collider.tag == "Enemy")
                        {
                            Enemy enemyScript = hit.collider.GetComponent<Enemy>();
                            enemyScript.health -= 3;
                            Debug.Log("bullet hit enemy.");
                        }
                    }
                    else
                    {
                        smallAmmo -= 1;
                        Debug.Log("bullet hit nothing.");
                    }
                    StartCoroutine(PistolCooldown());
                    Debug.Log("Fired weapon 1.");
                }
                else
                {
                    AutoSwitch();
                    Debug.Log("no bullets for weapon1.");
                }
            }
            //for weapon 2 
            if (hasWeapon2 == true && usingWeapon2 == true && canFireW2 == true)
            {
                if (medAmmo >= 1)
                {
                    shotgunAnimation.Play("ShotgunFire");
                    if (Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward, out RaycastHit hit, 200f))
                    {
                        medAmmo -= 1;
                        if (hit.collider.tag == "Environment")
                        {
                            //insert appropriate bullet decal here 
                            Debug.Log("bullet hit wall.");
                        }
                        else if (hit.collider.tag == "Enemy")
                        {
                            Enemy enemyScript = hit.collider.GetComponent<Enemy>();
                            enemyScript.health -= 7;
                            Debug.Log("bullet hit enemy.");
                        }
                    }
                    else
                    {
                        medAmmo -= 1;
                        Debug.Log("bullet hit nothing.");
                    }
                    StartCoroutine(ShotgunCooldown());
                    Debug.Log("Fired weapon 2.");
                }
                else
                {
                    AutoSwitch();
                    Debug.Log("no bullets for weapon2.");
                }
            }
        }
    }
    private void FireAutomaticWeapon()
    {
        //for weapon 3 
        if (canFireW3 == true)
        {
            if (bigAmmo >= 1)
            {
                arAnimation.Play("ArFire");
                if (Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward, out RaycastHit hit, 200f))
                {
                    bigAmmo -= 1;
                    if (hit.collider.tag == "Environment")
                    {
                        //insert appropriate bullet decal here 
                        Debug.Log("bullet hit wall.");
                    }
                    else if (hit.collider.tag == "Enemy")
                    {
                        Enemy enemyScript = hit.collider.GetComponent<Enemy>();
                        enemyScript.health -= 3;
                        Debug.Log("bullet hit enemy.");
                    }
                }
                else
                {
                    bigAmmo -= 1;
                    Debug.Log("bullet hit nothing.");
                }
                StartCoroutine(ARCooldown());
                Debug.Log("Fired weapon 3.");
            }
            else
            {
                AutoSwitch();
                Debug.Log("no bullets for weapon2.");
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
        if (usingWeapon1 == false)
        {
            StartCoroutine(SwitchWeapons1());
        }
        else
        {
            Debug.Log("Already using weapon 1.");
        }
    }
    /// <summary>
    /// switches to weapon 2 
    /// </summary>
    private void SwitchWeapon2()
    {
        if (usingWeapon2 == false)
        {
            StartCoroutine(SwitchWeapons2());
        }
        else
        {
            Debug.Log("Already using weapon 2.");
        }
    }
    /// <summary>
    /// switches to weapon 3 
    /// </summary>
    private void SwitchWeapon3()
    {
        if (usingWeapon3 == false)
        {
            StartCoroutine(SwitchWeapons3());
        }
        else
        {
            Debug.Log("Already using weapon 3.");
        }
    }
    /// <summary>
    /// switches to no weapon
    /// </summary>
    private void SwitchWeaponNone()
    {
        if (usingNoGun == false)
        {
            StartCoroutine(SwitchWeaponsNone());
        }
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
    /// cooldown for firing pistol
    /// </summary>
    /// <returns></returns>
    IEnumerator PistolCooldown()
    {
        canFireW1 = false;
        yield return new WaitForSeconds(.4f);
        canFireW1 = true;
    }
    /// <summary>
    /// cooldown for firing shotgun
    /// </summary>
    /// <returns></returns>
    IEnumerator ShotgunCooldown()
    {
        canFireW2 = false;
        yield return new WaitForSeconds(1f);
        canFireW2 = true;
    }
    /// <summary>
    /// cooldown between each assault rifle bullet
    /// </summary>
    /// <returns></returns>
    IEnumerator ARCooldown()
    {
        canFireW3 = false;
        yield return new WaitForSeconds(.1f);
        canFireW3 = true;
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
    //keeps you from shooting while switching weapons 
    IEnumerator SwitchWeapons1()
    {
        switchingWeapon = true;
        if (usingWeapon2 == true)
        {
            //lower previous weapon 
            shotgunAnimation.Play("LowerWeapon");
            yield return new WaitForSeconds(.5f);
            usingWeapon2 = false;
            weapon2.SetActive(false);
            Debug.Log("Switched off weapon 2.");
        }
        else if (usingWeapon3 == true)
        {
            arAnimation.Play("LowerWeapon");
            yield return new WaitForSeconds(.5f);
            usingWeapon3 = false;
            weapon3.SetActive(false);
            Debug.Log("Switched off weapon 3.");
        }
        else if (usingNoGun == true)
        {
            yield return new WaitForSeconds(.5f);
            usingNoGun = false;
            Debug.Log("Switched off no weapon.");
        }
        //raise current weapon
        usingWeapon1 = true;
        weapon1.SetActive(true);
        pistolAnimation.Play("RaiseWeapon");
        yield return new WaitForSeconds(.5f);
        switchingWeapon = false;
    }
    IEnumerator SwitchWeapons2()
    {
        switchingWeapon = true;
        if (usingWeapon1 == true)
        {
            //lower previous weapon 
            pistolAnimation.Play("LowerWeapon");
            yield return new WaitForSeconds(.5f);
            usingWeapon1 = false;
            weapon1.SetActive(false);
            Debug.Log("Switched off weapon 1.");
        }
        else if (usingWeapon3 == true)
        {
            arAnimation.Play("LowerWeapon");
            yield return new WaitForSeconds(.5f);
            usingWeapon3 = false;
            weapon3.SetActive(false);
            Debug.Log("Switched off weapon 3.");
        }
        else if (usingNoGun == true)
        {
            yield return new WaitForSeconds(.5f);
            usingNoGun = false;
            Debug.Log("Switched off no weapon.");
        }
        //raise current weapon
        usingWeapon2 = true;
        weapon2.SetActive(true);
        shotgunAnimation.Play("RaiseWeapon");
        yield return new WaitForSeconds(.5f);
        switchingWeapon = false;
    }
    IEnumerator SwitchWeapons3()
    {
        switchingWeapon = true;
        if (usingWeapon1 == true)
        {
            //lower previous weapon 
            pistolAnimation.Play("LowerWeapon");
            yield return new WaitForSeconds(.5f);
            usingWeapon1 = false;
            weapon1.SetActive(false);
            Debug.Log("Switched off weapon 1.");
        }
        else if (usingWeapon2 == true)
        {
            shotgunAnimation.Play("LowerWeapon");
            yield return new WaitForSeconds(.5f);
            usingWeapon2 = false;
            weapon2.SetActive(false);
            Debug.Log("Switched off weapon 2.");
        }
        else if (usingNoGun == true)
        {
            yield return new WaitForSeconds(.5f);
            usingNoGun = false;
            Debug.Log("Switched off no weapon.");
        }
        //raise current weapon
        usingWeapon3 = true;
        weapon3.SetActive(true);
        arAnimation.Play("RaiseWeapon");
        yield return new WaitForSeconds(.5f);
        switchingWeapon = false;
    }
    IEnumerator SwitchWeaponsNone()
    {
        switchingWeapon = true;
        if (usingWeapon1 == true)
        {
            //lower previous weapon 
            pistolAnimation.Play("LowerWeapon");
            yield return new WaitForSeconds(.5f);
            usingWeapon1 = false;
            weapon1.SetActive(false);
            Debug.Log("Switched off weapon 1.");
        }
        else if (usingWeapon2 == true)
        {
            shotgunAnimation.Play("LowerWeapon");
            yield return new WaitForSeconds(.5f);
            usingWeapon2 = false;
            weapon2.SetActive(false);
            Debug.Log("Switched off weapon 2.");
        }
        else if (usingWeapon3 == true)
        {
            arAnimation.Play("LowerWeapon");
            yield return new WaitForSeconds(.5f);
            usingWeapon3 = false;
            weapon3.SetActive(false);
            Debug.Log("Switched off weapon 3.");
        }
        usingNoGun = true;
        Debug.Log("Switched to no weapon.");
        switchingWeapon = false;
    }
}
