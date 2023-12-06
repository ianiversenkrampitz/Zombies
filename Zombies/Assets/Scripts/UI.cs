using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
//Iversen-Krampitz, Ian 
//11/30/2023
//controls UI.

public class UI : MonoBehaviour
{
    public TMP_Text Ammo;
    public TMP_Text Round;
    public TMP_Text Score;
    public TMP_Text Cost;
    public PlayerController playerController;
    public MapController mapController;
    public int cost;
    public bool showCost;
    public int score;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //shows the cost of a weapon when going close to it 
        if (showCost == true)
        {
            Cost.text = "Press E to buy for " + cost;
            Debug.Log("showcost is true");
        }
        else
        {
            Cost.text = "";
        }
        //creates round number 
        Round.text = "Round " + mapController.RoundNumber;
        Score.text = "Score: " + score;
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
}
