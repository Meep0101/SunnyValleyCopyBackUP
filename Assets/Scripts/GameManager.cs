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

    public PathVisualizer pathVisualizer;

    public PlacementManager placementManager;




    [SerializeField] private Transform[] redNodeTransformArray;
    [SerializeField] private Transform terminalTransform;
    private List<StationNode> stationNodelist;


    void Start()
    {
        uiController.OnRoadPlacement += RoadPlacementHandler;
        //uiController.OnHousePlacement += HousePlacementHandler;
        //uiController.OnSpecialPlacement += SpecialPlacementHandler;
        //uiController.OnBigStructurePlacement += BigStructurePlacement;
        uiController.OnRemoveRoad += RemoveRoadHandler;
        inputManager.OnEscape += HandleEscape;
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
        pathVisualizer.ResetPath();
        inputManager.OnMouseClick += TrySelectingAgent;
    }

    private void TrySelectingAgent(Ray ray)
    {
        GameObject hitObject = objectDetector.RaycastAll(ray);
        if(hitObject != null)
        {
            var agentScript = hitObject.GetComponent<AiAgent>();
            agentScript?.ShowPath();
        }
    }

    // private void BigStructurePlacement()
    // {
    //     ClearInputActions();

    //     inputManager.OnMouseClick += (pos) =>
    //     {
    //         ProcessInputAndCall(structureManager.PlaceBigStructure, pos);
    //     };
    //     inputManager.OnEscape += HandleEscape;
    // }

    // private void SpecialPlacementHandler()
    // {
    //     ClearInputActions();

    //     inputManager.OnMouseClick += (pos) =>
    //     {
    //         ProcessInputAndCall(structureManager.PlaceStation, pos);
    //     };
    //     inputManager.OnEscape += HandleEscape;
    // }

    // private void HousePlacementHandler()
    // {
    //     ClearInputActions();

    //     inputManager.OnMouseClick += (pos) =>
    //     {
    //         ProcessInputAndCall(structureManager.PlaceTerminal, pos);
    //     };
    //     inputManager.OnEscape += HandleEscape;
    // }

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
}
