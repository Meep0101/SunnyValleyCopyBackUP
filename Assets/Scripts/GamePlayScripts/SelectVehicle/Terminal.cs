using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Terminal : MonoBehaviour
{
    // public Color emptyColor = Color.white;
    // public Color currentColor = Color.white;
    private int _currentCount = 5; // Private backing field
    //public int currentCount; 
    public int CurrentCount // Public property
    {
        get { return _currentCount; }
        private set { _currentCount = value; }
    }
    public bool IsClicked { get; private set; } // Property to track if the terminal is clicked

    public GameObject VehiclesTab; // Reference to the pop-up tab GameObject
    private InstVehicle vehicleSpawner;
    public CarbonMeter carbonMeter;

    // public void ChangeColor(Color newColor)
    // {
    //     currentColor = newColor;
    // }

    private void Start()
    {
        //CurrentCount = 2;
        Debug.Log("Initial current count: " + CurrentCount);
        //VehiclesTab = GameObject.Find("VehiclesTab");
        // Ensure that the pop-up tab is initially inactive
        //VehiclesTab.SetActive(false);
        vehicleSpawner = GetComponent<InstVehicle>();
    }

    private void OnMouseDown()
    {
        // Activate the pop-up tab when the player clicks on the terminal
        VehiclesTab.SetActive(true);
        IsClicked = true; // Set IsClicked to true when the terminal is clicked
    }
    public void SpawnVehicle(string vehicleType)
    {
        if (vehicleSpawner != null) // Check if the vehicle spawner reference is not null
        {
            vehicleSpawner.SpawnVehicle(vehicleType, this);
            DecrementCount(1);
            // IsClicked = false;
            // VehiclesTab.SetActive(false);
            IncrementCarbonMeter();
        }
    }

    private void IncrementCarbonMeter()
    {
        if (carbonMeter != null)
        {
            carbonMeter.IncreaseCarbonMeter();
        }
    }

    private void DecrementCount(int count)
    {
        CurrentCount-= count;
        Debug.Log("Current count: " + CurrentCount);
        IsClicked = false;
        VehiclesTab.SetActive(false);
    }

}
