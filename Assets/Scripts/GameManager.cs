using SimpleCity.AI;
using SVS;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;

    public CameraMovement cameraMovement;
    public RoadManager roadManager;
    public InputManager inputManager;

    public UIController uiController;

    public StructureManager structureManager;

    public ObjectDetector objectDetector;
    //public PathVisualizer pathVisualizer;
    public PlacementManager placementManager;

    [SerializeField] private GathererAI[] gathererAIArray;
    // [SerializeField] private Miner[] minerUnits;
    // [SerializeField] private Woodcutter[] woodcutterUnits;
    [SerializeField] private Transform[] goldNodeTransformArray;
    [SerializeField] private Transform[] treeNodeTransformArray;
    //[SerializeField] private Transform storageTransform;
    [SerializeField] private Transform[] goldStorageArray; // New field
    [SerializeField] private Transform[] treeStorageArray; // New field
    private List<ResourceNode> resourceNodeList; //Resurce Node object


    void Start()
    {
        uiController.OnRoadPlacement += RoadPlacementHandler;
        //uiController.OnHousePlacement += HousePlacementHandler;
        //uiController.OnSpecialPlacement += SpecialPlacementHandler;
        //uiController.OnBigStructurePlacement += BigStructurePlacement;
        uiController.OnRemoveRoad += RemoveRoadHandler;
        inputManager.OnEscape += HandleEscape;
    }

    private void Awake() {
        instance = this;

        GameResources.Init();

        resourceNodeList = new List<ResourceNode>();

        foreach (Transform goldNodeTransform in goldNodeTransformArray){
            resourceNodeList.Add(new ResourceNode(goldNodeTransform, GameResources.StationType.Gold));
        }
        foreach (Transform treeNodeTransform in treeNodeTransformArray){
            resourceNodeList.Add(new ResourceNode(treeNodeTransform, GameResources.StationType.Wood));
        }

        // // Initialize miner units
        // foreach (Miner miner in minerUnits)
        // {
        //     // Example initialization for each miner unit
        //     miner.speed = 5f; // Set the speed
        //     miner.collectionCapacity = 2; // Set the collection capacity
        //     // Add more initialization if needed
        // }

        // // Initialize woodcutter units
        // foreach (Woodcutter woodcutter in woodcutterUnits)
        // {
        //     // Example initialization for each woodcutter unit
        //     woodcutter.speed = 7f; // Set the speed
        //     woodcutter.collectionCapacity = 3; // Set the collection capacity
        //     // Add more initialization if needed
        // }
    }

    private void RemoveRoadHandler()
    {
        
        ClearInputActions();

        inputManager.OnMouseClick += (pos) =>
        {
            ProcessInputAndCall(placementManager.RemoveRoadObject, pos);
        };
        inputManager.OnMouseUp += roadManager.FinishPlacingRoad;
        inputManager.OnMouseHold += (pos) =>
        {
            ProcessInputAndCall(placementManager.RemoveRoadObject, pos);
        };
        inputManager.OnEscape += HandleEscape;
    }

    private void HandleEscape()
    {
        ClearInputActions();
        uiController.ResetButtonColor();
        //pathVisualizer.ResetPath();
        //inputManager.OnMouseClick += TrySelectingAgent;
    }


    private void RoadPlacementHandler()
    {
        ClearInputActions();

        inputManager.OnMouseClick += (pos) =>
        {
            ProcessInputAndCall(roadManager.PlaceRoad, pos);
        };
        inputManager.OnMouseUp += roadManager.FinishPlacingRoad;
        inputManager.OnMouseHold += (pos) =>
        {
            ProcessInputAndCall(roadManager.PlaceRoad, pos);
        };
        inputManager.OnEscape += HandleEscape;
    }

    private void ClearInputActions()
    {
        inputManager.ClearEvents();
    }

    private void ProcessInputAndCall(Action<Vector3Int> callback, Ray ray)
    {
        Vector3Int? result = objectDetector.RaycastGround(ray);
        if (result.HasValue)
            callback.Invoke(result.Value);
    }

    private void Update()
    {
        cameraMovement.MoveCamera(new Vector3(inputManager.CameraMovementVector.x, 0, inputManager.CameraMovementVector.y));
    }

    private ResourceNode GetResourceNode() {
        //List<Transform> resourceNodeList = new List<Transform>() { goldNode1Transform, goldNode2Transform, goldNode3Transform };
        
        List<ResourceNode> tmpResourceNodeList = new List<ResourceNode>(resourceNodeList);      //Clone List only for the use of cycle
        for (int i = 0; i < tmpResourceNodeList.Count; i++){
            if (!tmpResourceNodeList[i].HasResources()){
                //No more Resources or Passengers
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

    public static ResourceNode GetResourceNode_Static() {
        return instance.GetResourceNode();
    }

    private ResourceNode GetResourceNodeType(GameResources.StationType stationType) {
        //List<Transform> resourceNodeList = new List<Transform>() { goldNode1Transform, goldNode2Transform, goldNode3Transform };
        
        List<ResourceNode> tmpResourceNodeList = new List<ResourceNode>(resourceNodeList);      //Clone List only for the use of cycle
        for (int i = 0; i < tmpResourceNodeList.Count; i++){
            if (!tmpResourceNodeList[i].HasResources() || tmpResourceNodeList[i].GetStationType() != stationType){
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


    // private Transform GetStorage() {
    //     return storageTransform;
    // }

    // public static Transform GetStorage_Static() {
    //     return instance.GetStorage();
    // }

    private Transform GetStorage(GameResources.StationType stationType)
    {
        if (stationType == GameResources.StationType.Gold)
        {
            return goldStorageArray[UnityEngine.Random.Range(0, goldStorageArray.Length)];
        }
        else if (stationType == GameResources.StationType.Wood)
        {
            return treeStorageArray[UnityEngine.Random.Range(0, treeStorageArray.Length)];
        }
        else
        {
            Debug.LogError("Unknown resource type!");
            return null;
        }
    }

    public static Transform GetStorage_Static(GameResources.StationType stationType)
    {
        return instance.GetStorage(stationType);
    }
}


