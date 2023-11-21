using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CarbonMeter : MonoBehaviour
{
    public Text carbonMeterText; // Reference to the UI Text element

    private int carbonMeterValue = 0;

    // Function to update the carbon meter UI
    public void UpdateCarbonMeter()
    {
        if (carbonMeterText != null)
        {
            carbonMeterText.text = "Carbon Emission:" + carbonMeterValue +"%";
        }
    }

    // Function to increase the carbon meter value
    public void IncreaseCarbonMeter()
    {
        carbonMeterValue++;
        UpdateCarbonMeter(); // Update UI when CarbonMeter changes
    }

    // Function to reset the carbon meter value
    public void ResetCarbonMeter()
    {
        carbonMeterValue = 0;
        UpdateCarbonMeter(); // Update UI when CarbonMeter changes
    }
}