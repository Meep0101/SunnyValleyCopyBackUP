using SVS;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StructureManager : MonoBehaviour
{
    public GameObject terminalPrefab, stationPrefab;     // removed bigStructuresPrefabs;
    public PlacementManager placementManager;

    #region 
    //private float[] terminalWeights, stationWeights; // removed bigStructureWeights;

     
    //private void Start()
    //{
        // terminalWeights = terminalPrefab.Select(prefabStats => prefabStats.weight).ToArray();
        // stationWeights = stationPrefab.Select(prefabStats => prefabStats.weight).ToArray();
        //bigStructureWeights = bigStructuresPrefabs.Select(prefabStats => prefabStats.weight).ToArray();
    //}
    #endregion

    public void PlaceTerminal(Vector3Int position)
    {
        if (CheckPositionBeforePlacement(position))
        {
            //int randomIndex = GetRandomWeightedIndex(terminalWeights);
            placementManager.PlaceObjectOnTheMap(position, terminalPrefab, CellType.Structure);
            AudioPlayer.instance.PlayPlacementSound();
        }
    }

    public void PlaceStation(Vector3Int position)
    {
        if (CheckPositionBeforePlacement(position))
        {
            //int randomIndex = GetRandomWeightedIndex(stationWeights);
            placementManager.PlaceObjectOnTheMap(position, stationPrefab, CellType.SpecialStructure);
            AudioPlayer.instance.PlayPlacementSound();
        }
    }

    private bool CheckPositionBeforePlacement(Vector3Int position)
    {
        if (placementManager.CheckIfPositionInBound(position) == false)
        {
            Debug.Log("This position is out of bounds");
            return false;
        }
        if (placementManager.CheckIfPositionIsFree(position) == false)
        {
            Debug.Log("This position is not EMPTY");
            return false;
        }

        //Should be removed
        if (placementManager.GetNeighboursOfTypeFor(position, CellType.Road).Count <= 0)
        {
            Debug.Log("Must be placed near a road");
            return false;
        }
        return true;

        #region 
        // BigStructures
        // if (DefaultCheck(position) == false)
        // {
        //     return false;
        // }

        // if (RoadCheck(position) == false)
        //     return false;
        
        // return true;
        #endregion
    }

    #region 
    // private int GetRandomWeightedIndex(float[] weights)
    // {
    //     float sum = 0f;
    //     for (int i = 0; i < weights.Length; i++)
    //     {
    //         sum += weights[i];
    //     }

    //     float randomValue = UnityEngine.Random.Range(0, sum);
    //     float tempSum = 0;
    //     for (int i = 0; i < weights.Length; i++)
    //     {
    //         //0->weihg[0] weight[0]->weight[0] + weight[1]
    //         if(randomValue >= tempSum && randomValue < tempSum + weights[i])
    //         {
    //             return i;
    //         }
    //         tempSum += weights[i];
    //     }
    //     return 0; //If something went wrong here
    // }
    #endregion



    #region 
    // internal void PlaceBigStructure(Vector3Int position)
    // {
    //     int width = 2;
    //     int height = 2;
    //     if(CheckBigStructure(position, width , height))
    //     {
    //         int randomIndex = GetRandomWeightedIndex(bigStructureWeights);
    //         placementManager.PlaceObjectOnTheMap(position, bigStructuresPrefabs[randomIndex].prefab, CellType.Structure, width, height);
    //         AudioPlayer.instance.PlayPlacementSound();
    //     }
    // }

    // private bool CheckBigStructure(Vector3Int position, int width, int height)
    // {
    //     bool nearRoad = false;
    //     for (int x = 0; x < width; x++)
    //     {
    //         for (int z = 0; z < height; z++)
    //         {
    //             var newPosition = position + new Vector3Int(x, 0, z);
                
    //             if (DefaultCheck(newPosition)==false)
    //             {
    //                 return false;
    //             }
    //             if (nearRoad == false)
    //             {
    //                 nearRoad = RoadCheck(newPosition);
    //             }
    //         }
    //     }
    //     return nearRoad;
    // }


    // private bool RoadCheck(Vector3Int position)
    // {
    //     if (placementManager.GetNeighboursOfTypeFor(position, CellType.Road).Count <= 0)
    //     {
    //         Debug.Log("Must be placed near a road");
    //         return false;
    //     }
    //     return true;
    // }

    // private bool DefaultCheck(Vector3Int position)
    // {
    //     if (placementManager.CheckIfPositionInBound(position) == false)
    //     {
    //         Debug.Log("This position is out of bounds");
    //         return false;
    //     }
    //     if (placementManager.CheckIfPositionIsFree(position) == false)
    //     {
    //         Debug.Log("This position is not EMPTY");
    //         return false;
    //     }
    //     return true;
    // }
    #endregion
}

#region 
// [Serializable]
// public struct StructurePrefab
// {
//     public GameObject prefab;
//     [Range(0,1)]
//     //public float weight;
// }
#endregion