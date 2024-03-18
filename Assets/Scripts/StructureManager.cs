// using SVS;
// using System;
// using System.Collections;
// using System.Collections.Generic;
// using System.Linq;
// using UnityEngine;

// public class StructureManager : MonoBehaviour
// {
//     public StructurePrefabWeighted[] housesPrefabe, specialPrefabs;   //Removed bigStructurePrefabs
//     public PlacementManager placementManager;

//     private float[] houseWeights, specialWeights; //Removed bigStructureWeights

//     private void Start()
//     {
//         houseWeights = housesPrefabe.Select(prefabStats => prefabStats.weight).ToArray();
//         specialWeights = specialPrefabs.Select(prefabStats => prefabStats.weight).ToArray();
//         //bigStructureWeights = bigStructuresPrefabs.Select(prefabStats => prefabStats.weight).ToArray();
//     }

//     public void PlaceHouse(Vector3Int position)
//     {
//         if (CheckPositionBeforePlacement(position))
//         {
//             int randomIndex = GetRandomWeightedIndex(houseWeights);
//             placementManager.RecordObjectOnTheMap(position, housesPrefabe[randomIndex].prefab, CellType.Structure);
//             AudioPlayer.instance.PlayPlacementSound();
//         }
//     }

//     public void PlaceSpecial(Vector3Int position)
//     {
//         if (CheckPositionBeforePlacement(position))
//         {
//             int randomIndex = GetRandomWeightedIndex(specialWeights);
//             placementManager.RecordObjectOnTheMap(position, specialPrefabs[randomIndex].prefab, CellType.SpecialStructure);
//             AudioPlayer.instance.PlayPlacementSound();
//         }
//     }

//     private int GetRandomWeightedIndex(float[] weights)
//     {
//         float sum = 0f;
//         for (int i = 0; i < weights.Length; i++)
//         {
//             sum += weights[i];
//         }

//         float randomValue = UnityEngine.Random.Range(0, sum);
//         float tempSum = 0;
//         for (int i = 0; i < weights.Length; i++)
//         {
//             //0->weihg[0] weight[0]->weight[1]
//             if(randomValue >= tempSum && randomValue < tempSum + weights[i])
//             {
//                 return i;
//             }
//             tempSum += weights[i];
//         }
//         return 0;
//     }

//     private bool CheckPositionBeforePlacement(Vector3Int position)
//     {
//         if (DefaultCheck(position) == false)
//         {
//             return false;
//         }

//         // if (RoadCheck(position) == false)
//         //     return false;
        
//         return true;
//     }

//     // private bool RoadCheck(Vector3Int position)
//     // {
//     //     if (placementManager.GetNeighboursOfTypeFor(position, CellType.Road).Count <= 0)
//     //     {
//     //         Debug.Log("Must be placed near a road");
//     //         return false;
//     //     }
//     //     return true;
//     // }

//     private bool DefaultCheck(Vector3Int position)
//     {
//         if (placementManager.CheckIfPositionInBound(position) == false)
//         {
//             Debug.Log("This position is out of bounds");
//             return false;
//         }
//         if (placementManager.CheckIfPositionIsFree(position) == false)
//         {
//             Debug.Log("This position is not EMPTY");
//             return false;
//         }
//         return true;
//     }
// }

// [Serializable]
// public struct StructurePrefabWeighted
// {
//     public GameObject prefab;
//     [Range(0,1)]
//     public float weight;
// }

//     // internal void PlaceBigStructure(Vector3Int position)
//     // {
//     //     int width = 2;
//     //     int height = 2;
//     //     if(CheckBigStructure(position, width , height))
//     //     {
//     //         int randomIndex = GetRandomWeightedIndex(bigStructureWeights);
//     //         placementManager.RecordObjectOnTheMap(position, bigStructuresPrefabs[randomIndex].prefab, CellType.Structure, width, height);
//     //         AudioPlayer.instance.PlayPlacementSound();
//     //     }
//     // }

//     // private bool CheckBigStructure(Vector3Int position, int width, int height)
//     // {
//     //     bool nearRoad = false;
//     //     for (int x = 0; x < width; x++)
//     //     {
//     //         for (int z = 0; z < height; z++)
//     //         {
//     //             var newPosition = position + new Vector3Int(x, 0, z);
                
//     //             if (DefaultCheck(newPosition)==false)
//     //             {
//     //                 return false;
//     //             }
//     //             if (nearRoad == false)
//     //             {
//     //                 nearRoad = RoadCheck(newPosition);
//     //             }
//     //         }
//     //     }
//     //     return nearRoad;
//     // }

//using SVS;
// using System;
// using System.Collections;
// using System.Collections.Generic;
// using System.Linq;
using UnityEngine;

public class StructureManager : MonoBehaviour
{
    public StructurePrefabWeighted[] housesPrefabe, specialPrefabs;
    public PlacementManager placementManager;
    public float instantiationInterval = 10f; // Interval in seconds

    private float nextInstantiationTime;

    private void Start()
    {
        // Set the initial time for the next instantiation
        nextInstantiationTime = Time.time + instantiationInterval;
    }

    private void Update()
    {
        // Check if it's time to instantiate a house or special prefab
        if (Time.time >= nextInstantiationTime)
        {
            // Instantiate either a house or a special prefab
            if (Random.value < 0.5f)
            {
                InstantiateRandomPrefab(housesPrefabe);
            }
            else
            {
                InstantiateRandomPrefab(specialPrefabs);
            }

            // Set the next instantiation time
            nextInstantiationTime = Time.time + instantiationInterval;
        }
    }

    private void InstantiateRandomPrefab(StructurePrefabWeighted[] prefabs)
    {
        if (prefabs.Length == 0)
            return;

        // Select a random prefab
        int randomIndex = Random.Range(0, prefabs.Length);
        GameObject prefab = prefabs[randomIndex].prefab;
        //CellType cellType = (Random.value < 0.5f) ? CellType.Structure : CellType.SpecialStructure; // Randomly select cell type

        // Generate a random position (you may adjust this based on your game's requirements)
        Vector3Int position = new Vector3Int(Random.Range(10, 20), 0, Random.Range(10, 20)); // Example position range

        // Instantiate the prefab at the generated position
        GameObject newObject = Instantiate(prefab, position, Quaternion.identity);

        // Record the object on the map with the selected cell type
        placementManager.RecordObjectOnTheMap(position, newObject, CellType.Structure);

        // Play the placement sound (if applicable)
        // AudioPlayer.instance.PlayPlacementSound();
    }
}

[System.Serializable]
public struct StructurePrefabWeighted
{
    public GameObject prefab;
    [Range(0,1)]
    public float weight;
}
