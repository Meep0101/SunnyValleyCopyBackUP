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

    [SerializeField] private Transform[] REDNodeTransformArray;
    [SerializeField] private Transform[] BLUENodeTransformArray;
    [SerializeField] private Transform[] YELLOWNodeTransformArray;
    //[SerializeField] private Transform storageTransform;


    [SerializeField] private Transform[] RedStorageArray; // New field
    [SerializeField] private Transform[] BlueStorageArray; // New field
    [SerializeField] private Transform[] YellowStorageArray; // New field

    private List<ResourceNode> resourceNodeList; //Resurce Node object
    private List<StorageNode> storageNodeList; //Storage Node object


    void Start()
    {
        uiController.OnRoadPlacement += RoadPlacementHandler;
        uiController.OnRemoveRoad += RemoveRoadHandler;
        inputManager.OnEscape += HandleEscape;
    }

    private void Awake() {
        instance = this;

        GameResources.Init();

        resourceNodeList = new List<ResourceNode>();

        foreach (Transform REDNodeTransformArray in REDNodeTransformArray){
            resourceNodeList.Add(new ResourceNode(REDNodeTransformArray, GameResources.StationType.Red));
        }
        foreach (Transform BLUENodeTransformArray in BLUENodeTransformArray){
            resourceNodeList.Add(new ResourceNode(BLUENodeTransformArray, GameResources.StationType.Blue));
        }
        foreach (Transform YELLOWNodeTransformArray in YELLOWNodeTransformArray){
            resourceNodeList.Add(new ResourceNode(YELLOWNodeTransformArray, GameResources.StationType.Yellow));
        }

        storageNodeList = new List<StorageNode>();

        foreach (Transform RedStorageArray in RedStorageArray){
            storageNodeList.Add(new StorageNode(RedStorageArray, GameResources.StationType.Red));
        }
        foreach (Transform BlueStorageArray in BlueStorageArray){
            storageNodeList.Add(new StorageNode(BlueStorageArray, GameResources.StationType.Blue));
        }
        foreach (Transform YellowStorageArray in YellowStorageArray){
            storageNodeList.Add(new StorageNode(YellowStorageArray, GameResources.StationType.Yellow));
        }

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
            if (!tmpResourceNodeList[i].HasPassengers()){
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

    // private Transform GetStorage() {
    //     return storageTransform;
    // }

    // public static Transform GetStorage_Static() {
    //     return instance.GetStorage();
    // }

    private StorageNode GetStorageNodeType(GameResources.StationType storageType)
    {
        foreach (StorageNode storageNode in storageNodeList)
        {
            if (storageNode.GetStorageType() == storageType)
            {
                return storageNode;
            }
        }
        return null;
        
        // if (storageType == GameResources.StationType.Red)
        // {
        //     return RedStorageArray[UnityEngine.Random.Range(0, RedStorageArray.Length)];
        // }
        // else if (storageType == GameResources.StationType.Blue)
        // {
        //     return BlueStorageArray[UnityEngine.Random.Range(0, BlueStorageArray.Length)];
        // }
        // else if (storageType == GameResources.StationType.Yellow)
        // {
        //     return YellowStorageArray[UnityEngine.Random.Range(0, YellowStorageArray.Length)];
        // }
        // else
        // {
        //     Debug.LogError("Unknown resource type!");
        //     return null;
        // }
    }

    public static StorageNode GetStorageNodeType_Static(GameResources.StationType storageType)
    {
        return instance.GetStorageNodeType(storageType);
    }

}


