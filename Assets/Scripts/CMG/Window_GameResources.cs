using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Window_GameResources : MonoBehaviour
{
    private void Start(){
        GameResources.OnStationAmountChanged += delegate (object sender, EventArgs e) {
            UpdateResourceTextObject();
        };
        UpdateResourceTextObject();
    }

    private void UpdateResourceTextObject() {
        transform.Find("goldAmount").GetComponent<Text>().text = "GOLD: " + GameResources.GetStationAmount(GameResources.StationType.Gold);
    }

}
