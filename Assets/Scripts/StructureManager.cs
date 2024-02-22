using SVS;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StructureManager : MonoBehaviour //Created for on mouse click placing buildings only
{
    public GameObject terminalPrefab, stationPrefab;     // removed bigStructuresPrefabs;
    public PlacementManager placementManager;



    //To be removed 
    // // Handler to the input manager on Mouse click
    // public void PlaceTerminal(Vector3Int position)
    // {
    //     if (CheckPositionBeforePlacement(position))
    //     {
    //         //int randomIndex = GetRandomWeightedIndex(terminalWeights);
    //         placementManager.PlaceObjectOnTheMap(position, terminalPrefab, CellType.Structure);
    //         AudioPlayer.instance.PlayPlacementSound();
    //     }
    // }

    // public void PlaceStation(Vector3Int position)
    // {
    //     if (CheckPositionBeforePlacement(position))
    //     {
    //         //int randomIndex = GetRandomWeightedIndex(stationWeights);
    //         placementManager.PlaceObjectOnTheMap(position, stationPrefab, CellType.SpecialStructure);
    //         AudioPlayer.instance.PlayPlacementSound();
    //     }
    // }

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


    }



}

