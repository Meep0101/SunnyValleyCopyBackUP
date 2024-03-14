﻿using SimpleCity.AI;
using SVS;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
  
    public RoadManager roadManager;
    public InputManager inputManager;
    public UIController uiController;
    public StructureManager structureManager;
    public ObjectDetector objectDetector;
    public PathVisualizer pathVisualizer;
    public PlacementManager placementManager;
    public GameObject gameOverPanel;
    private int numberOfDays = 0;
    private int numberOfTrees = 0;
    private int numberOfVehicle = 0;
    public CarbonMeter carbonMeter;


    private bool isGamePaused = false;

    // Station
    [SerializeField] private Transform[] REDNodeTransformArray;
    [SerializeField] private Transform[] BLUENodeTransformArray;
    [SerializeField] private Transform[] YELLOWNodeTransformArray;

    // Terminal
    [SerializeField] private Transform[] RedStorageArray; // New field
    [SerializeField] private Transform[] BlueStorageArray; // New field
    [SerializeField] private Transform[] YellowStorageArray; // New field

    private List<ResourceNode> resourceNodeList; //Resurce Node object
    private List<StorageNode> storageNodeList; //Storage Node object


     private enum FunctionalityState
    {
        None,
        RoadPlacement,
        RemoveRoad
    }

    private FunctionalityState currentFunctionality = FunctionalityState.None;


    void Start()
    {
        uiController.OnRoadPlacement += RoadPlacementHandler;
        uiController.OnHousePlacement += HousePlacementHandler;
        uiController.OnSpecialPlacement += SpecialPlacementHandler;
        uiController.OnRemoveRoad += RemoveRoadHandler;
        inputManager.OnEscape += HandleEscape;
        uiController.OnPause += TogglePause;
        uiController.OnPlay += ResumeGame;
        
      uiController = FindObjectOfType<UIController>();
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


    private void TogglePause()
    {
        isGamePaused = !isGamePaused;

        if(isGamePaused)
        {
            PauseGame();
        }else{
            ResumeGame();
        }


    }
    private void PauseGame()
    {
        if(gameOverPanel != null)
        {
            Time.timeScale = 0f;
        }else{
            Debug.LogError("Game Panel not assigned");
        }
    }

    private void ResumeGame()
    {
        if (gameOverPanel != null)
        {
            Time.timeScale = 1f;
        }else{
            Debug.LogError("Game Panel not assigned");
        }
    }

   private void ToggleRoadPlacement()
    {
        if (currentFunctionality == FunctionalityState.RoadPlacement)
        {
            currentFunctionality = FunctionalityState.None;
        }else{
            currentFunctionality = FunctionalityState.RoadPlacement;
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
    }
      
    
    private void RemoveRoadHandler()
    {
        ClearInputActions();

        if (uiController.RoadButtonEnabled) // Check if road placement is enabled
        {
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
    }

    private void HandleEscape()
    {
        ClearInputActions();
        uiController.ResetButtonColor();
        pathVisualizer.ResetPath();
       
    }

    private void SpecialPlacementHandler()
    {
        ClearInputActions();

        inputManager.OnMouseClick += (pos) =>
        {
            ProcessInputAndCall(structureManager.PlaceSpecial, pos);
        };
        inputManager.OnEscape += HandleEscape;
    }

    private void HousePlacementHandler()
    {
        ClearInputActions();

        inputManager.OnMouseClick += (pos) =>
        {
            ProcessInputAndCall(structureManager.PlaceHouse, pos);
        };
        inputManager.OnEscape += HandleEscape;
    }

    private void RoadPlacementHandler()
    {
        ClearInputActions();

        if (uiController.RoadButtonEnabled) // Check if road placement is enabled
        {
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

    public void StopGame()
    {
        if(gameOverPanel != null){
        gameOverPanel.SetActive(true);
        Time.timeScale = 0f;
        

        //Add another condition for the OverCrowding and BothCarbonMeter & OverCrowding
        if(carbonMeter.GetCarbonMeterValue() >= 100)
        {
            gameOverCause = GameOverCause.CarbonMeterFull;

        }
        
        UIController uiController = FindAnyObjectByType<UIController>();
        int totalCarsSpawned = BlueAI.GetTotalCarsSpawned();
        uiController.UpdateGameOverPanel(numberOfDays, GetNumberOfTrees(), totalCarsSpawned, gameOverCause);
        }
        else{
            Debug.LogError("GAME PANEL NOT ASSIGNED");
        }
    }

    public void IncrementDays()
    {
        numberOfDays++;
    }
    public void IncrementTrees()
    {
        numberOfTrees++;
    }

    public int GetNumberOfTrees()
    {
        return numberOfTrees;
    }
    public void IncrementVehicles()
    {
        numberOfVehicle++;
    }

    

    private GameOverCause gameOverCause = GameOverCause.None;
   public enum GameOverCause
   {
    None,
    CarbonMeterFull,
    OverCrowdedStations,
    BothCarbonMeterAndOverCrowded,
   }

  
}
