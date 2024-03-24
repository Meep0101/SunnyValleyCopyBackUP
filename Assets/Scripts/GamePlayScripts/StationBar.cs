using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StationBar : MonoBehaviour
{
    public Slider slider;
    public int sAmountValue = 10;

    // Reference to the ResourceNode instance
    private ResourceNode resourceNode;
    
    

    public void SetResourceNode(ResourceNode node)
    {
        resourceNode = node;
        UpdateSliderValue(); // Update slider value when setting the resource node
    }

    private void UpdateSliderValue()
    {
        if (resourceNode != null && slider != null)
        {
            slider.maxValue = resourceNode.stationAmountMax;
            slider.value = resourceNode.stationAmount;
        }
    }

    public void DecreaseSliderValue (int amount)
    {
        slider.value -= amount;
    }

    // Update the slider value whenever station amount changes
    private void Update()
    {
        UpdateSliderValue();
    }

    

}
