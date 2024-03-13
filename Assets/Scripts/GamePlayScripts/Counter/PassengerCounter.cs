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
        var TotalResources = GameResources.GetStationAmount(GameResources.StationType.Red) + GameResources.GetStationAmount(GameResources.StationType.Blue) + GameResources.GetStationAmount(GameResources.StationType.Yellow);
        transform.Find("passengerAmount").GetComponent<Text>().text = "Passenger: " + TotalResources;


        // If wanted separate values
        // "RED: " + GameResources.GetStationAmount(GameResources.StationType.Red) + "\n" +
        // "BLUE: " + GameResources.GetStationAmount(GameResources.StationType.Blue) + "\n";
        // "YELLOW: " + GameResources.GetStationAmount(GameResources.StationType.Yellow) + "\n";
    }


}