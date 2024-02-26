using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class VehicleSelection : MonoBehaviour
{
    public GameObject jeepPrefab, pedicabPrefab, motorcyclePrefab, busPrefab;

    public Terminal terminal;
    public Button jeepButton;
    public Button pedicabButton;
    public Button motorcycleButton;
    public Button busButton;

    private void Start()
    {
        jeepButton.onClick.AddListener(() => 
        {
            Debug.Log("Jeep button clicked");
            SelectVehicle("Jeep");
        }
        );
        pedicabButton.onClick.AddListener(() => SelectVehicle("Pedicab"));
        motorcycleButton.onClick.AddListener(() => SelectVehicle("Motorcycle"));
        busButton.onClick.AddListener(() => SelectVehicle("Bus"));
    }

    private void SelectVehicle(string vehicleType)
    {
        Debug.Log("SelectVehicle method called with vehicle type: " + vehicleType);
        switch (vehicleType)
        {
            case "Jeep":
                //if (terminal.currentCount >= 0)
                //{
                    SpawnVehicle(jeepPrefab);
                    terminal.DecrementCount();
                    Debug.Log("Spawn Jeep");
                    Debug.Log("Terminal count: "+ terminal.currentCount);
                //}
                break;
            case "Pedicab":
                if (terminal.currentCount > 0)
                {
                    SpawnVehicle(pedicabPrefab);
                    terminal.DecrementCount();
                }
                break;
            case "Motorcycle":
                if (terminal.currentCount > 0)
                {
                    SpawnVehicle(motorcyclePrefab);
                    terminal.DecrementCount();
                }
                break;
            case "Bus":
                if (terminal.currentCount > 0)
                {
                    SpawnVehicle(busPrefab);
                    terminal.DecrementCount();
                }
                break;
        }
    }

    private void SpawnVehicle(GameObject vehiclePrefab)
    {
        // Instantiate the selected vehicle at the terminal position
        Instantiate(vehiclePrefab, terminal.transform.position, Quaternion.identity);
    }
}
