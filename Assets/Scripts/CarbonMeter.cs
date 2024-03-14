
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CarbonMeter : MonoBehaviour
{
    public Slider carbonMeterSlider; // Reference to the UI Slider element
    public Text carbonMeterText;     // Reference to the UI Text element

    private int carbonMeterValue = 80;

    void Start()
    {
        InitializeCarbonMeter();
    }

    // Function to initialize the carbon meter UI
    void InitializeCarbonMeter()
    {
        if (carbonMeterSlider != null)
        {
            carbonMeterSlider.maxValue = 100;
            carbonMeterSlider.value = carbonMeterValue;
        }

        UpdateCarbonMeterText();
    }

    // Function to update the carbon meter UI text
    void UpdateCarbonMeterText()
    {
        if (carbonMeterText != null)
        {
            carbonMeterText.text = "Carbon Emission: " + carbonMeterValue + "%";

            if (carbonMeterValue >= 100)
            {
                StopGame();
            }
        }
    }

    // Function to update the carbon meter UI slider
    public void UpdateCarbonMeter()
    {
        if (carbonMeterSlider != null)
        {
            carbonMeterSlider.value = carbonMeterValue;
        }

        UpdateCarbonMeterText();
    }

    // Function to increase the carbon meter value
    public void IncreaseCarbonMeter()
    {
        carbonMeterValue++;
        UpdateCarbonMeter(); // Update UI when CarbonMeter changes
    }

    // Function to decrease the carbon meter value
    public void DecreaseCarbonMeter()
    {
        carbonMeterValue--;
        UpdateCarbonMeter();
    }

    // Function to reset the carbon meter value
    public void ResetCarbonMeter()
    {
        carbonMeterValue = 80;
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
