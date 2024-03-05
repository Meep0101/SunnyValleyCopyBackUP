using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CarbonMeter : MonoBehaviour
{
    public Text carbonMeterText; // Reference to the UI Text element

    private int carbonMeterValue = 95;

     void Start()
    {
        UpdateCarbonMeter();
    }

    // Function to update the carbon meter UI

    public void UpdateCarbonMeter()
    {
        if (carbonMeterText != null)
        {
            carbonMeterText.text = "Carbon Emission:" + carbonMeterValue +"%";

            if (carbonMeterValue >= 100)
            {
                StopGame();
            }
        
        }
    }

    // Function to increase the carbon meter value
    public void IncreaseCarbonMeter()
    {
        carbonMeterValue++;
        UpdateCarbonMeter(); // Update UI when CarbonMeter changes
    }

    public void DecreaseCarbonMeter()
    {
        carbonMeterValue--;
        UpdateCarbonMeter();
    }

    // Function to reset the carbon meter value
    public void ResetCarbonMeter()
    {
        carbonMeterValue = 95;
        UpdateCarbonMeter(); // Update UI when CarbonMeter changes
    }

    private void StopGame()
    {
        Debug.Log("STOP NA AYOKO NA");
    }
    public int GetCarbonMeterValue()
    {
        return carbonMeterValue;

    }

   
}