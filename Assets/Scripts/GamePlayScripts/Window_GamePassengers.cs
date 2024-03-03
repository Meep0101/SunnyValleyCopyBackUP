using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class Window_GamePassengers : MonoBehaviour
{
    private void Start(){
        GameResources.OnStationAmountChanged += delegate (object sender, EventArgs e) {
            UpdateResourceTextObject();
        };
        UpdateResourceTextObject();
    }


    private void UpdateResourceTextObject() {
        var TotalResources = GameResources.GetStationAmount(GameResources.StationType.Gold) + GameResources.GetStationAmount(GameResources.StationType.Wood);
        transform.Find("passengerAmount").GetComponent<Text>().text = "Passenger: " + TotalResources;


        // If wanted separate values
        // "GOLD: " + GameResources.GetStationAmount(GameResources.StationType.Gold) + "\n" +
        // "WOOD: " + GameResources.GetStationAmount(GameResources.StationType.Wood) + "\n";
    }


}

