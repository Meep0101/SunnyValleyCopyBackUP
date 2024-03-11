using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementManager : MonoBehaviour
{
    public int width, height;
    Grid placementGrid;

    //List for temporary roadobjects
    private Dictionary<Vector3Int, StructureModel> temporaryRoadobjects = new Dictionary<Vector3Int, StructureModel>();
    // Allow us to access objects that are already placed on the map
    private Dictionary<Vector3Int, StructureModel> structureDictionary = new Dictionary<Vector3Int, StructureModel>(); 

    public struct PlacedObject //Manually placed objects
    {
        public Vector3 position;
        public CellType cellType;
    }

    private void Start()
    {
        placementGrid = new Grid(width, height);
        //RetrieveManuallyPlacedObjects(); //Manually placed objects
    }


    internal bool CheckIfPositionInBound(Vector3Int position)
    {
        if (position.x >= 0 && position.x < width && position.z >= 0 && position.z < height)
        {
            return true;
        }
        return false;
    }
    internal bool CheckIfPositionIsFree(Vector3Int position)
    {
        return CheckIfPositionIsOfType(position, CellType.Empty);
    }

    private bool CheckIfPositionIsOfType(Vector3Int position, CellType type)
    {
        return placementGrid[position.x, position.z] == type;
    }


    internal CellType[] GetNeighbourTypesFor(Vector3Int position)
    {
        return placementGrid.GetAllAdjacentCellTypes(position.x, position.z);
    }


    //StructurePlacement to be removed
    internal void PlaceObjectOnTheMap(Vector3Int position, GameObject structurePrefab, CellType type) //Removed int width = 1, int height = 1
    {
        placementGrid[position.x, position.z] = type;
        StructureModel structure = CreateANewStructureModel(position, structurePrefab, type);
        structureDictionary.Add(position, structure);
        //DestroyNatureAt(position);

        // Sets NEAREST ROAD position for each structure so that we can access it when finding a path betweeb each structure 
        // var structureNeedingRoad = structure.GetComponent<INeedingRoad>();
        // if (structureNeedingRoad != null)
        // {
        //     structureNeedingRoad.RoadPosition = GetNearestRoad(position, width, height).Value;  //Where the structure requires a near ROAD
        //     Debug.Log("My nearest road position is: " + structureNeedingRoad.RoadPosition);
        // }
    }

    //RoadFixer
    internal void PlaceTemporaryStructure(Vector3Int position, GameObject structurePrefab, CellType type)
    {
        placementGrid[position.x, position.z] = type;
        StructureModel structure = CreateANewStructureModel(position, structurePrefab, type);
        temporaryRoadobjects.Add(position, structure);
    }

    //RoadFixer
    private StructureModel CreateANewStructureModel(Vector3Int position, GameObject structurePrefab, CellType type)
    {
        GameObject structure = new GameObject(type.ToString());
        structure.transform.SetParent(transform);
        structure.transform.localPosition = position;

        StructureModel structureModel = structure.AddComponent<StructureModel>();
        structureModel.CreateModel(structurePrefab);
        return structureModel;
    }

    //RoadFixer
    internal List<Vector3Int> GetNeighboursOfTypeFor(Vector3Int position, CellType type) //CellType Road used
    {
        var neighbourVertices = placementGrid.GetAdjacentCellsOfType(position.x, position.z, type);
        List<Vector3Int> neighbours = new List<Vector3Int>();
        foreach (var point in neighbourVertices)
        {
            neighbours.Add(new Vector3Int(point.X, 0, point.Y));
        }
        return neighbours;
    }


    //Create a path between two structures
    private Vector3Int? GetNearestRoad(Vector3Int position, int width, int height)
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                var newPosition = position + new Vector3Int(x, 0, y);
                var roads = GetNeighboursOfTypeFor(newPosition, CellType.Road);
                if (roads.Count > 0)
                {
                    return roads[0];
                }
            }
        }
        return null;
    }

    private void DestroyNatureAt(Vector3Int position)
    {
        RaycastHit[] hits = Physics.BoxCastAll(position + new Vector3(0, 0.5f, 0), new Vector3(0.5f, 0.5f, 0.5f), transform.up, Quaternion.identity, 1f, 1 << LayerMask.NameToLayer("Nature"));
        foreach (var item in hits)
        {
            Destroy(item.collider.gameObject);
        }
    }

    //A*
    internal List<Vector3Int> GetPathBetween(Vector3Int startPosition, Vector3Int endPosition, bool isAgent = false)
    {
        var resultPath = GridSearch.AStarSearch(placementGrid, new Point(startPosition.x, startPosition.z), new Point(endPosition.x, endPosition.z), isAgent);
        List<Vector3Int> path = new List<Vector3Int>();
        foreach (Point point in resultPath)
        {
            path.Add(new Vector3Int(point.X, 0, point.Y));
        }
        return path;
    }

    //A*
    internal void RemoveAllTemporaryStructures()
    {
        foreach (var structure in temporaryRoadobjects.Values)
        {
            var position = Vector3Int.RoundToInt(structure.transform.position);
            placementGrid[position.x, position.z] = CellType.Empty;
            Destroy(structure.gameObject);
        }
        temporaryRoadobjects.Clear();
    }

    //A*
    internal void AddtemporaryStructuresToStructureDictionary()
    {
        foreach (var structure in temporaryRoadobjects)
        {
            structureDictionary.Add(structure.Key, structure.Value);
            DestroyNatureAt(structure.Key);
        }
        temporaryRoadobjects.Clear();
    }

    internal Vector3Int WorldToGridPosition(Vector3 worldPosition)
    {
        // Convert world position to grid position based on grid cell size
        int x = Mathf.FloorToInt(worldPosition.x);
        int z = Mathf.FloorToInt(worldPosition.z);
        return new Vector3Int(x, 0 ,z);
    }
    
    public Vector3Int WorldToCell(Vector3 position)
    {
        return new Vector3Int(
            Mathf.FloorToInt(position.x),
            Mathf.FloorToInt(position.y),
            Mathf.FloorToInt(position.z)
        );
    }


    #region GetPositions NoExplanation

    public StructureModel GetStructureAt(Vector3Int position)
    {
        if (structureDictionary.ContainsKey(position))
        {
            return structureDictionary[position];
        }
        return null;
    }
    
    public List<StructureModel> GetAllHouses()
    {
        List<StructureModel> returnList = new List<StructureModel>();
        // Find all GameObjects with the "Structure" tag
        GameObject[] structureObjects = GameObject.FindGameObjectsWithTag("Structure");
        foreach (var structureObject in structureObjects)
        {
            // Get the StructureModel component from the GameObject
            StructureModel structureModel = structureObject.GetComponent<StructureModel>();
            if (structureModel != null)
            {
                returnList.Add(structureModel);
            }
        }
        return returnList;

        // List<StructureModel> returnList = new List<StructureModel>();
        // var housePositions = placementGrid.GetAllHouses();
        // foreach (var point in housePositions)
        // {
        //     returnList.Add(structureDictionary[new Vector3Int(point.X, 0, point.Y)]);
        // }
        // return returnList;
    }

    internal List<StructureModel> GetAllSpecialStructures()
    {

        List<StructureModel> returnList = new List<StructureModel>();
        // Find all GameObjects with the "SpecialStructure" tag
        GameObject[] specialStructureObjects = GameObject.FindGameObjectsWithTag("SpecialStructure");
        foreach (var specialStructureObject in specialStructureObjects)
        {
            // Get the StructureModel component from the GameObject
            StructureModel structureModel = specialStructureObject.GetComponent<StructureModel>();
            if (structureModel != null)
            {
                returnList.Add(structureModel);
            }
        }
        return returnList;

        // List<StructureModel> returnList = new List<StructureModel>();
        // var housePositions = placementGrid.GetAllSpecialStructure();
        // foreach (var point in housePositions)
        // {
        //     returnList.Add(structureDictionary[new Vector3Int(point.X, 0, point.Y)]);
        // }
        // return returnList;
    }

    //Will REMOVE
    // public StructureModel GetRandomRoad()   //Will Remove
    // {
    //     var point = placementGrid.GetRandomRoadPoint();
    //     return GetStructureAt(point);
    // }

    // public StructureModel GetRandomSpecialStrucutre()   //Will Remove
    // {
    //     var point = placementGrid.GetRandomSpecialStructurePoint();
    //     return GetStructureAt(point);
    // }

    // public StructureModel GetRandomHouseStructure() //Will Remove
    // {
    //     var point = placementGrid.GetRandomHouseStructurePoint();
    //     return GetStructureAt(point);
    // }

    //Will REMOVE
    // private StructureModel GetStructureAt(Point point) //Ask  the PlacementManager what is in the given structure
    // {
    //     if (point != null)
    //     {
    //         return structureDictionary[new Vector3Int(point.X, 0, point.Y)];
    //     }
    //     Debug.Log("No GetStructureAt Point");
    //     return null;
    // }
    #endregion



}
