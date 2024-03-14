using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class PassengerCounter : MonoBehaviour
{
    private void Start(){
        GameResources.OnStationAmountChanged += delegate (object sender, EventArgs e) {
            UpdateResourceTextObject();
        };
        UpdateResourceTextObject();
    }


    private void UpdateResourceTextObject() {
        var TotalResources = GameResources.GetStationAmount(GameResources.StationType.Red) + GameResources.GetStationAmount(GameResources.StationType.Blue) + GameResources.GetStationAmount(GameResources.StationType.Yellow);
        transform.Find("passengerAmount").GetComponent<Text>().text =  TotalResources.ToString();


        // If wanted separate values
        // "RED: " + GameResources.GetStationAmount(GameResources.StationType.Red) + "\n" +
        // "BLUE: " + GameResources.GetStationAmount(GameResources.StationType.Blue) + "\n";
        // "YELLOW: " + GameResources.GetStationAmount(GameResources.StationType.Yellow) + "\n";
    }

    public int GetTotalPassengers()
    {
       int totalPassengers = GameResources.GetStationAmount(GameResources.StationType.Red) +
                              GameResources.GetStationAmount(GameResources.StationType.Blue) +
                              GameResources.GetStationAmount(GameResources.StationType.Yellow);

        return totalPassengers; 
    }

    


}