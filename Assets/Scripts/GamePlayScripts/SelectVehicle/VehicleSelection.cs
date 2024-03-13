using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class VehicleSelection : MonoBehaviour
{
    //public GameObject jeepPrefab, pedicabPrefab, motorcyclePrefab, busPrefab;

    //public Terminal terminal;
    public List<Terminal> terminals = new List<Terminal>(); // List of terminals
    public Button jeepButton;
    public Button pedicabButton;
    public Button motorcycleButton;
    public Button busButton;

    private void Start()
    {
        Terminal[] terminals = GameObject.FindObjectsOfType<Terminal>(); // Find all Terminal objects in the scene
        // Add listeners to the vehicle selection buttons for each terminal
        jeepButton.onClick.AddListener(() => SpawnVehicle("Jeep"));
        pedicabButton.onClick.AddListener(() => SpawnVehicle("Pedicab"));
        motorcycleButton.onClick.AddListener(() => SpawnVehicle("Motorcycle"));
        busButton.onClick.AddListener(() => SpawnVehicle("Bus"));
    }

    private void SpawnVehicle(string vehicleType)
    {
        // Iterate through each terminal and spawn the vehicle at the clicked terminal
        foreach (var terminal in terminals)
        {
            if (terminal != null && terminal.IsClicked) // Check if the terminal reference is not null and it is clicked
            {
                if (terminal.CurrentCount > 0)
                {
                    terminal.SpawnVehicle(vehicleType);
                    break; // Exit the loop after spawning the vehicle at the clicked terminal
                }
            }
        }
    }

}

