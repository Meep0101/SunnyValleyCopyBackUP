using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CarSpawner : MonoBehaviour
{
    public GameObject[] carPrefabs;
    private GameObject spawnedCar;

    public float laneWidth = 5.0f; // Set the lane width in the Unity Editor

    private void Start()
    {
         Instantiate(SelectACarPrefab(), transform);
        SpawnCar(); //spawn car but dont move
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

    

//     public void SpawnCar()
//     {
//     if (carPrefabs != null && carPrefabs.Length > 0)
//         {
//             // Select a random car prefab from the array
//             GameObject selectedCarPrefab = carPrefabs[Random.Range(0, carPrefabs.Length)];

//             // Determine the spawn position based on the dynamic lane width
//             Vector3 spawnPosition = DetermineSpawnPosition();

//             // Instantiate the selected car prefab at the determined position
//             spawnedCar = Instantiate(selectedCarPrefab, spawnPosition, Quaternion.identity);
//         }
//         else
//         {
//             Debug.LogError("Car prefabs are not assigned to the CarSpawner!");
//         }
// }

private Vector3 DetermineSpawnPosition()
{
    // Implement your logic here to determine the spawn position based on the condition
    // Example: return the position on the left side of the road
    return transform.position - transform.right * laneWidth;
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
