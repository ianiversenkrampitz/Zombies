using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
//Iversen-Krampitz, Ian 
//12/5/2023
//controls UI.

public class UI : MonoBehaviour
{
    public TMP_Text Ammo;
    public TMP_Text Round;
    public TMP_Text Score;
    public TMP_Text Cost;
    public TMP_Text Invulnerability;
    public TMP_Text QuadDamage;
    public TMP_Text NoAmmo;
    public PlayerController playerController;
    public MapController mapController;
    public Enemy enemy;
    public int cost;
    public bool showCost;
    public bool showPowerUp1;
    public bool showPowerUp2;

    // Update is called once per frame
    void Update()
    {
        //shows the cost of a weapon when going close to it 
        if (showCost == true)
        {
            Cost.text = "Interact to buy for " + cost;
            StartCoroutine(BuyText());
        }
        else
        {
            Cost.text = "";
        }
        //shows text for invulnerability 
        if (showPowerUp1 == true)
        {
            Invulnerability.text = "Invulnerability Active for 20 Seconds.";
            StartCoroutine(InvulnerabilityTextCooldown());
        }
        else
        {
            Invulnerability.text = "";
        }
        //shows text for quad damage
        if (showPowerUp2 == true)
        {
            QuadDamage.text = "Quad Damage Active for 20 Seconds.";
            StartCoroutine(QuadDamageTextCooldown());
        }
        else
        {
            QuadDamage.text = "";
        }
        if (playerController.usingNoGun == true)
        {
            NoAmmo.text = "No Ammo";
        }
        else
        {
            NoAmmo.text = "";
        }
        //creates round number 
        Round.text = "Round " + mapController.RoundNumber;
        Score.text = "Score: " + playerController.totalScore;
        //creates ammo count based on weapon being used 
        if (playerController.usingWeapon1 == true)
        {
            Ammo.text = "Ammo: " + playerController.smallAmmo;
        }
        if (playerController.usingWeapon2 == true)
        {
            Ammo.text = "Ammo: " + playerController.medAmmo;
        }
        if (playerController.usingWeapon3 == true)
        {
            Ammo.text = "Ammo: " + playerController.bigAmmo;
        }
        if (playerController.usingNoGun == true)
        {
            Ammo.text = "";
        }
    }
    /// <summary>
    /// displays buy text for 2 seconds 
    /// </summary>
    /// <returns></returns>
    IEnumerator BuyText()
    {
        yield return new WaitForSeconds(2f);
        showCost = false;
    }
    /// <summary>
    /// cooldown for invulnerability text
    /// </summary>
    /// <returns></returns>
    IEnumerator InvulnerabilityTextCooldown()
    {
        showPowerUp1 = true;
        yield return new WaitForSeconds(3f);
        showPowerUp1 = false;
    }
    /// <summary>
    /// cooldown for quad damage text
    /// </summary>
    /// <returns></returns>
    IEnumerator QuadDamageTextCooldown()
    {
        showPowerUp2 = true;
        yield return new WaitForSeconds(3f);
        showPowerUp2 = false;
    }
}
