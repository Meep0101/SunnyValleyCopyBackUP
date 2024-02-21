using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StationSpawner : MonoBehaviour
{

    public PlacementManager placementManager;

    void Start()
    {
        foreach (Transform stationTransform in transform)
        {
            //Get the position of the station prefabs in the scene
            Vector3Int position = placementManager.WorldToGridPosition(stationTransform.position);

            //Set the cell type of the grid at the building's position to Structure 
            placementManager.PlaceObjectOnTheMap(position, stationTransform.gameObject, CellType.SpecialStructure);
        }
    }

}
