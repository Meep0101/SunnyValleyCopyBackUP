using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;
using UnityEngine.UIElements;


public class PlacementManager : MonoBehaviour
{
    private static PlacementManager instance;

    public int width, height;
    public float cellSize;
    Grid placementGrid;
    public CarbonMeter carbonMeter;
   
    public GameObject treePrefab;
    public int numberOfTrees = 1; //kung ilan ang spawn

    

    public RoadFixer roadFixer;

    public float treeSpawnIntervalMin = 2f;
    public float treeSpawnIntervalMax = 5f;
   
    private int treeCount = 0;
    public Text treeCountText;
    private int vehicleCount = 0;

   

    private Dictionary<Vector3Int, StructureModel> temporaryRoadobjects = new Dictionary<Vector3Int, StructureModel>();
    private Dictionary<Vector3Int, StructureModel> structureDictionary = new Dictionary<Vector3Int, StructureModel>();

    // Array (at first) of Terminal prefabs to spawn
    public GameObject RedTerminal; 
    public GameObject BlueTerminal; 
    public GameObject YellowTerminal; 

    // Array of Station prefabs to spawn
    public GameObject RedStation; 
    public GameObject BlueStation; 
    public GameObject YellowStation; 

    // Array of positions for Terminals
    public Vector3Int[] RedTerminalPositions; 
    public Vector3Int[] BlueTerminalPositions; 
    public Vector3Int[] YellowTerminalPositions; 

    // Array of positions for Stations
    public Vector3Int[] RedStationPositions; 
    public Vector3Int[] BlueStationPositions; 
    public Vector3Int[] YellowStationPositions; 

    public int structureSpawnInterval;  // Timer spawn interval

    private List<ResourceNode> resourceNodeList; //Resurce Node object
    private List<StorageNode> storageNodeList; //Storage Node object

    private void Start()
    {
        placementGrid = new Grid(width, height, cellSize);

        carbonMeter = FindObjectOfType<CarbonMeter>();

        StartCoroutine(SpawnTreesRandomly());

        // SpawnREDWithInterval();
        // SpawnBLUEWithInterval();
        // SpawnYELLOWWithInterval();
        // //StartCoroutine(SpawnStructuresWithInterval());
    }

    private void Awake()
    {
        instance = this;
        GameResources.Init();
        resourceNodeList = new List<ResourceNode>();

        SpawnREDWithInterval();
        SpawnBLUEWithInterval();
        SpawnYELLOWWithInterval();
        //StartCoroutine(SpawnStructuresWithInterval());
    }

    void Update()
    {
        if (treeCountText != null)
        {
            treeCountText.text =treeCount.ToString();
        }
    }

    
    private void SpawnREDWithInterval()  // private IEnumerator SpawnStructureWithInterval()
    {
        foreach (Vector3Int RedPosition in RedTerminalPositions)
        {
            SpawnStructure(RedTerminal, RedPosition);
            storageNodeList.Add(new StorageNode(RedPosition, GameResources.StationType.Red));
            //SpawnStructure(RedTerminal[UnityEngine.Random.Range(0, RedTerminal.Length)], housePosition);
            //yield return new WaitForSeconds(structureSpawnInterval);
        }

        foreach (Vector3Int RStationPosition in RedStationPositions)
        {
            SpawnSpecialStructure(RedStation, RStationPosition);
            //SpawnStructure(RedStation[UnityEngine.Random.Range(0, RedStation.Length)], specialStructurePosition);
            //yield return new WaitForSeconds(structureSpawnInterval);
        }
    }

    private void SpawnBLUEWithInterval()  // private IEnumerator SpawnStructureWithInterval()
    {
        foreach (Vector3Int BluePosition in BlueTerminalPositions)
        {
            SpawnStructure(BlueTerminal, BluePosition);
            storageNodeList.Add(new StorageNode(BluePosition, GameResources.StationType.Blue));
        }

        foreach (Vector3Int BStationPosition in BlueStationPositions)
        {
            SpawnSpecialStructure(BlueStation, BStationPosition);
        }
    }

    private void SpawnYELLOWWithInterval()  // private IEnumerator SpawnStructureWithInterval()
    {
        foreach (Vector3Int YellowPosition in YellowTerminalPositions)
        {
            SpawnStructure(YellowTerminal, YellowPosition);
            storageNodeList.Add(new StorageNode(YellowPosition, GameResources.StationType.Yellow));
        }

        foreach (Vector3Int YStationPosition in YellowStationPositions)
        {
            SpawnSpecialStructure(YellowStation, YStationPosition);
        }
    }



    // Method to spawn a structure (house or special structure) at a specific position
    private void SpawnStructure(GameObject structurePrefab, Vector3Int position)
    {
        if (CheckIfPositionInBound(position) && CheckIfPositionIsFree(position))
        {
            GameObject structure = Instantiate(structurePrefab, position, Quaternion.identity);
            RecordObjectOnTheMap(position, structure, CellType.Structure); // Record the structure on the map
        }
    }

    private void SpawnSpecialStructure(GameObject structurePrefab, Vector3Int position)
    {
        if (CheckIfPositionInBound(position) && CheckIfPositionIsFree(position))
        {
            GameObject structure = Instantiate(structurePrefab, position, Quaternion.identity);
            RecordObjectOnTheMap(position, structure, CellType.SpecialStructure); // Record the structure on the map
        }
    }


    private ResourceNode GetResourceNodeType(GameResources.StationType stationType) {
        //List<Transform> resourceNodeList = new List<Transform>() { goldNode1Transform, goldNode2Transform, goldNode3Transform };
        
        List<ResourceNode> tmpResourceNodeList = new List<ResourceNode>(resourceNodeList);      //Clone List only for the use of cycle
        for (int i = 0; i < tmpResourceNodeList.Count; i++){
            if (!tmpResourceNodeList[i].HasPassengers() || tmpResourceNodeList[i].GetStationType() != stationType){
                //No more Resources/Passengers or different type
                tmpResourceNodeList.RemoveAt(i);
                i--;
            }
        }
        if (tmpResourceNodeList.Count > 0){
            return tmpResourceNodeList[UnityEngine.Random.Range(0, tmpResourceNodeList.Count)];     //Return that have resources or passengers
        } else {
            return null;
        }
    }

    public static ResourceNode GetResourceNodeType_Static(GameResources.StationType stationType) {
        return instance.GetResourceNodeType(stationType);
    }

    private StorageNode GetStorageNodeType(GameResources.StationType storageType)
    {
        List<StorageNode> tmpStorageNodeList = new List<StorageNode>(storageNodeList);      //Clone List only for the use of cycle
        for (int i = 0; i < tmpStorageNodeList.Count; i++){
            if (tmpStorageNodeList[i].GetStorageType() != storageType){
                //No more Resources/Passengers or different type
                tmpStorageNodeList.RemoveAt(i);
                i--;
            }
        }
        if (tmpStorageNodeList.Count > 0){
            return tmpStorageNodeList[UnityEngine.Random.Range(0, tmpStorageNodeList.Count)];     //Return that have resources or passengers
        } else {
            return null;
        }
        // foreach (StorageNode storageNode in storageNodeList)
        // {
        //     if (storageNode.GetStorageType() == storageType)
        //     {
        //         return storageNode;
        //     }
        // }
        // return null;
        
    }

    public static StorageNode GetStorageNodeType_Static(GameResources.StationType storageType)
    {
        return instance.GetStorageNodeType(storageType);
    }



    
       private IEnumerator SpawnTreesRandomly()
    {
        while (true)
        {

            if (carbonMeter.GetCarbonMeterValue() >= 100)
            {
                StopSpawningTrees(); //make tree stop 
                yield break ;
            }

        Vector3Int randomPosition = new Vector3Int(UnityEngine.Random.Range(0,width), 0, UnityEngine.Random.Range(0, height)); //random
        

        if (CheckIfPositionInBound(randomPosition) && CheckIfPositionIsFree(randomPosition))
        {
            randomPosition = new Vector3Int(Mathf.RoundToInt(randomPosition.x), 0, Mathf.RoundToInt(randomPosition.z)); //problem - it spawns out of bounds in x axis
            
                       
            GameObject nature = Instantiate(treePrefab, randomPosition, Quaternion.identity);
            Spawntrees(nature.transform);


            carbonMeter.DecreaseCarbonMeter();  //change this to decrease once the vehicle is attached
            FindObjectOfType<GameManager>().IncrementTrees();
            
        }
        

        float nextSpawnInterval = UnityEngine.Random.Range(treeSpawnIntervalMin, treeSpawnIntervalMax);
        yield return new WaitForSeconds(nextSpawnInterval);
        }
    }

    private void StopSpawningTrees() //makes tree stop
    {
       //Debug.Log("TREES STOP!");

       FindObjectOfType<GameManager>().StopGame();
    }

    private bool CheckIfTreeWillSpawnInBounds(Vector3Int position)
    {
        return position.x >= 0 && position.x < width;
    }

    private void UpdateTreeCOuntUI()
    {
        if (treeCountText != null)
        {
            treeCountText.text =  treeCount.ToString();
        }
    }

    private void Spawntrees(Transform parent)
    {
        for (int i = 0; i < numberOfTrees; i++)
        {

            Vector3 treeOffset = new Vector3(UnityEngine.Random.Range(-1f, 1f), 0, UnityEngine.Random.Range(-1f, 1f));
            
            
            treeCount++;

            UpdateTreeCOuntUI();
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

    internal void RecordObjectOnTheMap(Vector3Int position, GameObject structurePrefab, CellType type)
    {
       
        placementGrid[position.x, position.z] = type;
         StructureModel structure = CreateANewStructureModel(position, structurePrefab, type);
        structureDictionary.Add(position, structure);

        // var structureNeedingRoad = structure.GetComponent<INeedingRoad>();
        // if (structureNeedingRoad != null)
        // {
        //     structureNeedingRoad.RoadPosition = GetNearestRoad(position, width, height).Value;
        //     Debug.Log("My nearest road position is: " + structureNeedingRoad.RoadPosition);
        // }

       
        

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

            UpdateTreeCOuntUI();
        }
    }

    public void RemoveNatureAndDecreaseCarbon(Vector3Int position)
    {
        DestroyNatureAt(position);
        carbonMeter.DecreaseCarbonMeter();
        carbonMeter.UpdateCarbonMeter();

        treeCount++;
    }

    public void PlaceVehicleAndIncreaseCarbon(Vector3Int position, GameObject vehiclePrefab)
    {
        GameObject newVehicle =Instantiate(vehiclePrefab, position, Quaternion.identity);
        CarAI carAI = newVehicle.GetComponent<CarAI>();
        carbonMeter.IncreaseCarbonMeter();

       FindObjectOfType<GameManager>().IncrementVehicles();
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
        //structureModel.CreateModel(structurePrefab);
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

    //Road fixer
    public void ModifyStructureModel(Vector3Int position, GameObject newModel, Quaternion rotation)
    {
        if (temporaryRoadobjects.ContainsKey(position))
            temporaryRoadobjects[position].SwapModel(newModel, rotation);
        else if (structureDictionary.ContainsKey(position))
            structureDictionary[position].SwapModel(newModel, rotation);
    }

    // public StructureModel GetRandomRoad()
    // {
    //     var point = placementGrid.GetRandomRoadPoint();
    //     return GetStructureAt(point);
    // }

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

    public int GetTreeCount()
    {
        return treeCount;
    }
    private int GetVehicleCount()
    {
        return vehicleCount;
    }
}
