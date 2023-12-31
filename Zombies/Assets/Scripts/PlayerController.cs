using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
//Iversen-Krampitz, Ian
//12/5/2023
//Controls the player, collision, guns, UI. 

public class PlayerController : MonoBehaviour
{
    public float speed;
    public float health;
    public int smallAmmo;
    public int medAmmo;
    public int bigAmmo;
    public int totalScore;
    public int damage1;
    public int damage2;
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
    public UI ui;
    public MapController mapController;
    public GameObject hurt1;
    public GameObject hurt2;
    public GameObject hurt3;
    public GameObject weapon1;
    public GameObject weapon2;
    public GameObject weapon3;
    public GameObject buyWeapon2;
    public GameObject buyWeapon3;
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
        smallAmmo = 25;
        rb = GetComponent<Rigidbody>();
        switchingWeapon = false;
        ui.showCost = false;
        damage1 = 3;
        damage2 = 6;
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
        if (switchingWeapon == false)
        {
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
                if (medAmmo >= 1 && hasWeapon2)
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
                if (bigAmmo >= 1 && hasWeapon3)
                {
                    SwitchWeapon3();
                }
                else
                {
                    Debug.Log("No ammo for weapon3.");
                }
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
        //for power up keys
        if (other.gameObject.tag == "PowerUp")
        {
            mapController.PowerUpIndex++;
            other.gameObject.SetActive(false);
            Debug.Log("Picked up power up key.");
        }
        //for quad damage power up
        if (other.gameObject.tag == "QuadDamage")
        {
            StartCoroutine(QuadDamage());
            other.gameObject.SetActive(false);
            mapController.PowerUpSpawned = false;
            mapController.CanSpawnKeys = true;
            ui.showPowerUp2 = true;
            Debug.Log("Picked up quad damage.");
        }
        //for invulnerability power up
        if (other.gameObject.tag == "Invulnerability")
        {
            StartCoroutine(Invulnerability());
            other.gameObject.SetActive(false);
            mapController.PowerUpSpawned = false;
            mapController.CanSpawnKeys = true;
            ui.showPowerUp1 = true;
            Debug.Log("Picked up invulnerability.");
        }
        //for shotgun buying text
        if (other.gameObject.tag == "BuyWeapon2")
        {
            ui.cost = 600;
            ui.showCost = true;
        }
        //for assault rifle buying text
        if (other.gameObject.tag == "BuyWeapon3")
        {
            ui.cost = 1200;
            ui.showCost = true;
        }
        //for door open buying text
        if (other.gameObject.tag == "BuyDoorOpen")
        {
            ui.cost = 2000;
            ui.showCost = true;
        }
        if (other.gameObject.tag == "Weapon2" && totalScore >= 600)
        {
            //autoswitches to weapon 2 if player didn't previously have it 
            if (hasWeapon2 == false || usingNoGun == true)
            {
                SwitchWeapon2();
                hasWeapon2 = true;
                usingNoGun = false;
            }
            totalScore -= 600;
            medAmmo += 8;
            other.gameObject.SetActive(false);
            buyWeapon2.SetActive(false);
            Debug.Log("Picked up weapon2.");
        }
        if (other.gameObject.tag == "Weapon3" && totalScore >= 1200)
        {
            //autoswitches to weapon 3 if player didn't previously have it 
            if (hasWeapon3 == false || usingNoGun == true)
            {
                SwitchWeapon3();
                hasWeapon3 = true;
                usingNoGun = false;
            }
            totalScore -= 1200;
            bigAmmo += 30;
            other.gameObject.SetActive(false);
            buyWeapon3.SetActive(false);
            Debug.Log("Picked up weapon3.");
        }
        if (other.gameObject.tag == "DoorOpen" && totalScore >= 2000)
        {
            totalScore -= 2000;
            other.gameObject.SetActive(false);
            Debug.Log("Opened locked door.");
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
            medAmmo += 5;
            if (usingNoGun == true && hasWeapon2 == true)
            {
                SwitchWeapon2();
            }
            other.gameObject.SetActive(false);
        }
        if (other.gameObject.tag == "BigAmmo")
        {
            bigAmmo += 20;
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
                            Debug.Log("bullet hit wall.");
                        }
                        else if (hit.collider.tag == "Enemy")
                        {
                            //deals damage to enemy 
                            Enemy enemyScript = hit.collider.GetComponent<Enemy>();
                            enemyScript.health -= damage1;
                            if (enemyScript.health <= 0)
                            {
                                totalScore += 100;
                            }
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
                    //plays gun animation 
                    shotgunAnimation.Play("ShotgunFire");
                    if (Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward, out RaycastHit hit, 200f))
                    {
                        medAmmo -= 1;
                        if (hit.collider.tag == "Environment")
                        {
                            Debug.Log("bullet hit wall.");
                        }
                        else if (hit.collider.tag == "Enemy")
                        {
                            //deals damage to enemy
                            Enemy enemyScript = hit.collider.GetComponent<Enemy>();
                            enemyScript.health -= damage2;
                            if (enemyScript.health <= 0)
                            {
                                totalScore += 100;
                            }
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
                //plays gun animation 
                arAnimation.Play("ArFire");
                if (Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward, out RaycastHit hit, 200f))
                {
                    bigAmmo -= 1;
                    if (hit.collider.tag == "Environment")
                    {
                        Debug.Log("bullet hit wall.");
                    }
                    else if (hit.collider.tag == "Enemy")
                    {
                        //deals damage to enemy
                        Enemy enemyScript = hit.collider.GetComponent<Enemy>();
                        enemyScript.health -= damage1;
                        if (enemyScript.health <= 0)
                        {
                            totalScore += 100;
                        }
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
        SceneManager.LoadScene(2);
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
    /// <summary>
    /// makes damage 4x normal for 20 seconds 
    /// </summary>
    /// <returns></returns>
    IEnumerator QuadDamage()
    {
        damage1 = 12;
        damage2 = 24;
        yield return new WaitForSeconds(20f);
        damage1 = 3;
        damage2 = 6;
    }
    IEnumerator Invulnerability()
    {
        takesDamage = false;
        yield return new WaitForSeconds(20f);
        takesDamage = true;
    }
    /// <summary>
    /// switches weapon to 1 with the appropriate animations and keeps you from shooting during switch
    /// </summary>
    /// <returns></returns>
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
    /// <summary>
    /// switches weapon to 2 with the appropriate animations and keeps you from shooting during switch
    /// </summary>
    /// <returns></returns>
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
    /// <summary>
    /// switches weapon to 1 with the appropriate animations and keeps you from shooting during switch
    /// </summary>
    /// <returns></returns>
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
    /// <summary>
    /// switches weapon to none with the appropriate animations and keeps you from shooting during switch
    /// </summary>
    /// <returns></returns>
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
