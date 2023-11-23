using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CarSpawner : MonoBehaviour
{
    public GameObject[] carPrefabs;
    private GameObject spawnedCar;

    private void Start()
    {
         Instantiate(SelectACarPrefab(), transform);
        //SpawnCar();
    }

    public void SpawnCar()
    {
        if (carPrefabs != null && carPrefabs.Length > 0)
        {
            // Select a random car prefab from the array
            GameObject selectedCarPrefab = carPrefabs[Random.Range(0, carPrefabs.Length)];

            // Instantiate the selected car prefab
            spawnedCar = Instantiate(selectedCarPrefab, transform.position, Quaternion.identity);
        }
        else
        {
            Debug.LogError("Car prefabs are not assigned to the CarSpawner!");
        }

    }
    private GameObject SelectACarPrefab()
    {
        var randomIndex = Random.Range(0, carPrefabs.Length);
        return carPrefabs[randomIndex];
    }

    public void DestroyCar()
    {
        if (spawnedCar != null)
        {
            Destroy(spawnedCar);
        }
    }
}
