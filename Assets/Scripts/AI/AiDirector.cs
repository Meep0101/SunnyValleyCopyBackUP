using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

namespace SimpleCity.AI
{
    public class AiDirector : MonoBehaviour // spawn car and get direction
    {
        public PlacementManager placementManager; //Contains a grid/graph which stores all the diff. data of the map (Where is the road, structures) thus, get access on map and data on map // Performs A* algorithm
        public GameObject carPrefab;

        AdjacencyGraph carGraph = new AdjacencyGraph();

        List<Vector3> carPath = new List<Vector3>();

        public void SpawnACar() // Will REMOVE since it will spawn at terminals
        {
            Debug.Log("Clicked: SpawnACar");
            foreach (var structureObject in placementManager.GetAllHouses())    // Gets the Terminal points
            {
                Debug.Log("There is a structureObject in GetAllHouses");
                //TrySpawninACar(structureObject, placementManager.GetRandomSpecialStrucutre());    //Gets ONE Station point
                 // Gets house then special structure
            }
        }

        //Will REMOVE since the Vehicle has its own State
        private void TrySpawninACar(StructureModel startStructure, StructureModel endStructure) // No need hanapin start and end
        {
            if (startStructure != null && endStructure != null)
            {
                var startRoadPosition = startStructure.RoadPosition;
                var endRoadPosition = endStructure.RoadPosition;

                var path = placementManager.GetPathBetween(startRoadPosition, endRoadPosition, true);
                path.Reverse(); // Since the A* returns the path from the end pos to start pos

                if (path.Count == 0 && path.Count>2)
                    return;

                var startMarkerPosition = placementManager.GetStructureAt(startRoadPosition).GetCarSpawnMarker(path[1]);    // Gets outgoing carmarker to spawn the car

                var endMarkerPosition = placementManager.GetStructureAt(endRoadPosition).GetCarEndMarker(path[path.Count-2]);

                carPath = GetCarPath(path, startMarkerPosition.Position, endMarkerPosition.Position);   //Path of the car

                if(carPath.Count > 0)
                {
                    var car = Instantiate(carPrefab, startMarkerPosition.Position, Quaternion.identity);
                    car.GetComponent<CarAI>().SetPath(carPath);
                }
            }
            else{
                Debug.LogError("startStructure or endStructure is null!");
            }
        }

        //NOT SURE - Will Remove rin sine connected sa taas lang
        private List<Vector3> GetCarPath(List<Vector3Int> path, Vector3 startPosition, Vector3 endPosition)
        {
            carGraph.ClearGraph();
            CreatACarGraph(path);
            Debug.Log(carGraph);

            //Maybe relevant to stay
            return AdjacencyGraph.AStarSearch(carGraph, startPosition, endPosition);
        }

        private void CreatACarGraph(List<Vector3Int> path)
        {
            Dictionary<Marker, Vector3> tempDictionary = new Dictionary<Marker, Vector3>();
            for (int i = 0; i < path.Count; i++)
            {
                var currentPosition = path[i];
                var roadStructure = placementManager.GetStructureAt(currentPosition);
                var markersList = roadStructure.GetCarMarkers();
                var limitDistance = markersList.Count > 3;
                tempDictionary.Clear();

                foreach (var marker in markersList)
                {
                    carGraph.AddVertex(marker.Position);
                    foreach (var markerNeighbour in marker.adjacentMarkers)
                    {
                        carGraph.AddEdge(marker.Position, markerNeighbour.Position);
                    }
                    if(marker.OpenForconnections && i + 1 < path.Count)
                    {
                        var nextRoadPosition = placementManager.GetStructureAt(path[i + 1]);
                        if (limitDistance)
                        {
                            tempDictionary.Add(marker, nextRoadPosition.GetNearestCarMarkerTo(marker.Position));
                        }
                        else
                        {
                            carGraph.AddEdge(marker.Position, nextRoadPosition.GetNearestCarMarkerTo(marker.Position));
                        }
                    }
                }
                if (limitDistance && tempDictionary.Count > 2)
                {
                    var distanceSortedMarkers = tempDictionary.OrderBy(x => Vector3.Distance(x.Key.Position, x.Value)).ToList();
                    foreach (var item in distanceSortedMarkers)
                    {
                        Debug.Log(Vector3.Distance(item.Key.Position, item.Value));
                    }
                    for (int j = 0; j < 2; j++)
                    {
                        carGraph.AddEdge(distanceSortedMarkers[j].Key.Position, distanceSortedMarkers[j].Value);
                    }
                }
            }
        }

        private void Update()
        {
            //DrawGraph(carGraph);
            for (int i = 1; i < carPath.Count; i++)
            {
                Debug.DrawLine(carPath[i - 1] + Vector3.up, carPath[i] + Vector3.up, Color.magenta);
            }
        }

        private void DrawGraph(AdjacencyGraph graph)
        {
            foreach (var vertex in graph.GetVertices())
            {
                foreach (var vertexNeighbour in graph.GetConnectedVerticesTo(vertex))
                {
                    Debug.DrawLine(vertex.Position + Vector3.up, vertexNeighbour.Position + Vector3.up, Color.red);
                }
            }
        }

    }
}

