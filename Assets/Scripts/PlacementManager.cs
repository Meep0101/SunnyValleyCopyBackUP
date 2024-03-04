using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;


public class PlacementManager : MonoBehaviour
{
    public int width, height;
    Grid placementGrid;
    public CarbonMeter carbonMeter;
   
    public GameObject treePrefab;
    public int numberOfTrees = 1; //kung ilan ang spawn

    private StructureModel selectedRoadObject;

    public RoadFixer roadFixer;

    public float treeSpawnIntervalMin = 2f;
    public float treeSpawnIntervalMax = 5f;
    public GameObject roadPrefab;

    private int treeCount = 0;
    public Text treeCountText;


   

    private Dictionary<Vector3Int, StructureModel> temporaryRoadobjects = new Dictionary<Vector3Int, StructureModel>();
    private Dictionary<Vector3Int, StructureModel> structureDictionary = new Dictionary<Vector3Int, StructureModel>();


     private bool placingRoadEnabled = true;

     
    private void Start()
    {
        placementGrid = new Grid(width, height);
        carbonMeter = FindObjectOfType<CarbonMeter>();

        StartCoroutine(SpawnTreesRandomly());
    }

    void Update()
    {
        if (treeCountText != null)
        {
            treeCountText.text =treeCount.ToString();
        }
    }
    
    public void TogglePlacingRoad(bool isEnabled)
     {
        placingRoadEnabled = isEnabled;
     }

     public void PlaceRoadObject(Vector3Int position)
     {
        if(placingRoadEnabled)
        {
            PlaceObjectOnTheMap(position, roadPrefab, CellType.Road, 1, 1);
        }
     }
    private IEnumerator SpawnTreesRandomly()
    {
        while (true)
        {

        Vector3Int randomPosition = new Vector3Int(UnityEngine.Random.Range(0,width), 0, UnityEngine.Random.Range(0, height)); //random
        

        if (CheckIfPositionInBound(randomPosition) && CheckIfPositionIsFree(randomPosition))
        {
            randomPosition = new Vector3Int(Mathf.RoundToInt(randomPosition.x), 0, Mathf.RoundToInt(randomPosition.z)); //problem - it spawns out of bounds in x axis
            
            if(!CheckIfTreeWillSpawnInBounds(randomPosition))
            {
                Debug.Log("Tree would spawn out of bounds at position (X-Axis): " + randomPosition);
                continue;
            }
            
            GameObject nature = Instantiate(treePrefab, randomPosition, Quaternion.identity);
            Spawntrees(nature.transform);


            carbonMeter.DecreaseCarbonMeter();
            Debug.Log("Trees spawned at position: " + randomPosition);
        }
        

        float nextSpawnInterval = UnityEngine.Random.Range(treeSpawnIntervalMin, treeSpawnIntervalMax);
        yield return new WaitForSeconds(nextSpawnInterval);
        }
    }

    private bool CheckIfTreeWillSpawnInBounds(Vector3Int position)
    {
        return position.x >= 0 && position.x < width;
    }

    private void Spawntrees(Transform parent)
    {
        for (int i = 0; i < numberOfTrees; i++)
        {

            Vector3 treeOffset = new Vector3(UnityEngine.Random.Range(-1f, 1f), 0, UnityEngine.Random.Range(-1f, 1f));
            
            GameObject tree = Instantiate(treePrefab, parent.position + treeOffset, Quaternion.identity);
            tree.transform.SetParent(parent);
            treeCount++;
        }
    }

    internal CellType[] GetNeighbourTypesFor(Vector3Int position)
    {
        return placementGrid.GetAllAdjacentCellTypes(position.x, position.z);
    }

    internal bool CheckIfPositionInBound(Vector3Int position)
    {
        if (position.x >= 0 && position.x < width && position.z >= 0 && position.z < height)
        {
            return true;
        }
        return false;
    }

    internal void PlaceObjectOnTheMap(Vector3Int position, GameObject structurePrefab, CellType type, int width = 1, int height = 1)
    {
        StructureModel structure = CreateANewStructureModel(position, structurePrefab, type);

        var structureNeedingRoad = structure.GetComponent<INeedingRoad>();
        if (structureNeedingRoad != null)
        {
            structureNeedingRoad.RoadPosition = GetNearestRoad(position, width, height).Value;
            Debug.Log("My nearest road position is: " + structureNeedingRoad.RoadPosition);
        }

        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                var newPosition = position + new Vector3Int(x, 0, z);
                placementGrid[newPosition.x, newPosition.z] = type;
                structureDictionary.Add(newPosition, structure);
                DestroyNatureAt(newPosition);
            }
        }
        

    }


    public void RemoveRoadObject(Vector3Int position)
    {
        
        if (structureDictionary.ContainsKey(position))
        {
            // 1. Remove the road object from the grid.
            placementGrid[position.x, position.z] = CellType.Empty;
           

            // 2. Remove the road object from the structureDictionary.
            StructureModel removedStructure = structureDictionary[position];
            structureDictionary.Remove(position); //Here, position is a Vector3Int that represents the position you want to remove from the dictionary. After executing this line, the entry for positionToRemove will be removed from the structureDictionary, effectively deleting the association between that position and the structure.

            // 3. Optionally, destroy the associated GameObject.
            Destroy(removedStructure.gameObject);

            

            // 4. Fix the neighboring road prefabs.
            FixRoadPrefabs(position, roadFixer);

            // Update the grid and UI to reflect the changes.
            // Example: Handle visual updates here.
        }
        
    }

    private void FixRoadPrefabs(Vector3Int position, RoadFixer roadFixer)
    {
        // Get the neighboring road positions around the deleted road position
        var neighbors = GetNeighboursOfTypeFor(position, CellType.Road);

        foreach (var neighborPosition in neighbors)
        {
            roadFixer.FixRoadAtPosition(this, neighborPosition);
        }
    }

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
        foreach (var hit in hits)
        {
            Destroy(hit.collider.gameObject);
            treeCount--;
            Debug.Log("Tree Destroy");
        }
    }

    public void RemoveNatureAndDecreaseCarbon(Vector3Int position)
    {
        DestroyNatureAt(position);
        carbonMeter.DecreaseCarbonMeter();
        carbonMeter.UpdateCarbonMeter();
    }

    public void PlaceVehicleAndIncreaseCarbon(Vector3Int position, GameObject vehiclePrefab)
    {
        GameObject newVehicle =Instantiate(vehiclePrefab, position, Quaternion.identity);
        CarAI carAI = newVehicle.GetComponent<CarAI>();
        carbonMeter.IncreaseCarbonMeter();
    }

    internal bool CheckIfPositionIsFree(Vector3Int position)
    {
        return CheckIfPositionIsOfType(position, CellType.Empty);
    }

    private bool CheckIfPositionIsOfType(Vector3Int position, CellType type)
    {
        return placementGrid[position.x, position.z] == type;
    }

    internal void PlaceTemporaryStructure(Vector3Int position, GameObject structurePrefab, CellType type)
    {
        placementGrid[position.x, position.z] = type;
        StructureModel structure = CreateANewStructureModel(position, structurePrefab, type);
        temporaryRoadobjects.Add(position, structure);
    }

    internal List<Vector3Int> GetNeighboursOfTypeFor(Vector3Int position, CellType type)
    {
        var neighbourVertices = placementGrid.GetAdjacentCellsOfType(position.x, position.z, type);
        List<Vector3Int> neighbours = new List<Vector3Int>();
        foreach (var point in neighbourVertices)
        {
            neighbours.Add(new Vector3Int(point.X, 0, point.Y));
        }
        return neighbours;
    }

    private StructureModel CreateANewStructureModel(Vector3Int position, GameObject structurePrefab, CellType type)
    {
        GameObject structure = new GameObject(type.ToString());
        structure.transform.SetParent(transform);
        structure.transform.localPosition = position;

        StructureModel structureModel = structure.AddComponent<StructureModel>();
        structureModel.CreateModel(structurePrefab);
        return structureModel;
    }

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

    internal void AddtemporaryStructuresToStructureDictionary()
    {
        foreach (var structure in temporaryRoadobjects)
        {
            structureDictionary.Add(structure.Key, structure.Value);
            DestroyNatureAt(structure.Key);
        }
        temporaryRoadobjects.Clear();
    }

    public void ModifyStructureModel(Vector3Int position, GameObject newModel, Quaternion rotation)
    {
        if (temporaryRoadobjects.ContainsKey(position))
            temporaryRoadobjects[position].SwapModel(newModel, rotation);
        else if (structureDictionary.ContainsKey(position))
            structureDictionary[position].SwapModel(newModel, rotation);
    }

    public StructureModel GetRandomRoad()
    {
        var point = placementGrid.GetRandomRoadPoint();
        return GetStructureAt(point);
    }

    public StructureModel GetRandomSpecialStrucutre()
    {
        var point = placementGrid.GetRandomSpecialStructurePoint();
        return GetStructureAt(point);
    }

    public StructureModel GetRandomHouseStructure()
    {
        var point = placementGrid.GetRandomHouseStructurePoint();
        return GetStructureAt(point);
    }

    public List<StructureModel> GetAllHouses()
    {
        List<StructureModel> returnList = new List<StructureModel>();
        var housePositions = placementGrid.GetAllHouses();
        foreach (var point in housePositions)
        {
            returnList.Add(structureDictionary[new Vector3Int(point.X, 0, point.Y)]);
        }
        return returnList;
    }

    internal List<StructureModel> GetAllSpecialStructures()
    {
        List<StructureModel> returnList = new List<StructureModel>();
        var housePositions = placementGrid.GetAllSpecialStructure();
        foreach (var point in housePositions)
        {
            returnList.Add(structureDictionary[new Vector3Int(point.X, 0, point.Y)]);
        }
        return returnList;
    }


    private StructureModel GetStructureAt(Point point)
    {
        if (point != null)
        {
            return structureDictionary[new Vector3Int(point.X, 0, point.Y)];
        }
        return null;
    }

    public StructureModel GetStructureAt(Vector3Int position)
    {
        if (structureDictionary.ContainsKey(position))
        {
            return structureDictionary[position];
        }
        return null;
    }
}
