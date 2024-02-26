using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstVehicle : MonoBehaviour
{
    public GameObject jeepPrefab;
    public GameObject pedicabPrefab;
    public GameObject motorcyclePrefab;
    public GameObject busPrefab;
    private Terminal terminal;

    private void Start()
    {
        terminal = GameObject.Find("Terminal").GetComponent<Terminal>();
    }

    public void SpawnVehicle(string vehicleType)
    {
        GameObject prefab = null;
        switch (vehicleType)
        {
            case "Jeep":
                prefab = jeepPrefab;
                break;
            case "Pedicab":
                prefab = pedicabPrefab;
                break;
            case "Motorcycle":
                prefab = motorcyclePrefab;
                break;
            case "Bus":
                prefab = busPrefab;
                break;
        }

        if (prefab != null && terminal.currentCount >= 0)
        {
            Instantiate(prefab, terminal.transform.position, Quaternion.identity);
            terminal.DecrementCount();
        }
    }
}
