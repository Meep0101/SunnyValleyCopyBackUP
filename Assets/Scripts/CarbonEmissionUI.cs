using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CarbonEmissionUI : MonoBehaviour
{
     public Text carbonText; // Reference to the UI Text displaying carbon emission
    private int carbonMeterValue = 0; // Current carbon meter value

    // Method to update the carbon meter value and UI text
    public void UpdateCarbonMeter(int value)
    {
        carbonMeterValue = value;
        carbonText.text = "Carbon Emission Meter: " + carbonMeterValue++;
    }
}
