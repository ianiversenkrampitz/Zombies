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
    public PlayerController playerController;
    public MapController mapController;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //creates round number 
        Round.text = "Round " + mapController.RoundNumber;

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
