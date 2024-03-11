using SimpleCity.AI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StructureModel : MonoBehaviour
{
    float yHeight = 0;
    public Vector3Int RoadPosition { get; set; }

    //Road Fixer
    public void CreateModel(GameObject model) // Handles instantiating the road model
    {
        var structure = Instantiate(model, transform); //Named structure only with no relevance
        yHeight = structure.transform.position.y;
    }

    //Road Fixer
    public void SwapModel(GameObject model, Quaternion rotation)
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
        var structure = Instantiate(model, transform);
        structure.transform.localPosition = new Vector3(0, yHeight, 0);
        structure.transform.localRotation = rotation;
    }

    internal List<Marker> GetCarMarkers()
    {
        return transform.GetChild(0).GetComponent<RoadHelper>().GetAllCarMarkers();
    }

    public Vector3 GetNearestCarMarkerTo(Vector3 position)
    {
        return transform.GetChild(0).GetComponent<RoadHelper>().GetClosestCarMarkerPosition(position);
    }

    public Marker GetCarSpawnMarker(Vector3Int nextPathPosition)    //Will Remove
    {
        return transform.GetChild(0).GetComponent<RoadHelper>().GetPositioForCarToSpawn(nextPathPosition);
    }

    public Marker GetCarEndMarker(Vector3Int previousPathPosition)  //Will Remove
    {
        return transform.GetChild(0).GetComponent<RoadHelper>().GetPositioForCarToEnd(previousPathPosition);
    }

    
}
