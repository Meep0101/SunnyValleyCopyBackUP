using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstVehicle : MonoBehaviour
{
    public GameObject jeepPrefab;
    public GameObject pedicabPrefab;
    public GameObject motorcyclePrefab;
    public GameObject busPrefab;
    //private Terminal terminal;
    //private Terminal terminalInstance;


    public void SpawnVehicle(string vehicleType, Terminal terminal)
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

        if (prefab != null && terminal.CurrentCount > 0)
        {
            //var spawnMark = GetCarSpawnMarker;
            Instantiate(prefab, terminal.transform.position, Quaternion.identity);
            //terminal.DecrementCount();
            // if (terminalInstance == null)
            // {
            //     terminalInstance = terminal;
            // }
            // int Minuscount = 1;
            // terminalInstance.DecrementCounter(Minuscount);
        }
    }


}


