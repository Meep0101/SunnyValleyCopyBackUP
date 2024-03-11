using SVS;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadManager : MonoBehaviour
{
    public PlacementManager placementManager;

    public List<Vector3Int> temporaryPlacementPositions = new List<Vector3Int>();
    public List<Vector3Int> roadPositionsToRecheck = new List<Vector3Int>(); //To store the positions of neighbors

    private Vector3Int startPosition; //Save the position on the first clicked
    private bool placementMode = false;

    //public RoadFixer roadFixer;

    private void Start()
    {
        //roadFixer = GetComponent<RoadFixer>();
    }
}
