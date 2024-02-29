using SimpleCity.AI;
using SVS;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
   // public CameraMovement cameraMovement;
    public RoadManager roadManager;
    public InputManager inputManager;

    public UIController uiController;

    public StructureManager structureManager;

    public ObjectDetector objectDetector;

    public PathVisualizer pathVisualizer;

    public PlacementManager placementManager;

    bool placingRoadEnabled = true;
    bool removingRoadEnabled = true;

    void Start()
    {
        uiController.OnRoadPlacement += RoadPlacementHandler;
        uiController.OnHousePlacement += HousePlacementHandler;
        uiController.OnSpecialPlacement += SpecialPlacementHandler;
        uiController.OnRemoveRoad += RemoveRoadHandler;
        //inputManager.OnEscape += HandleEscape;
    }

    private void RemoveRoadHandler()
    {
        
        ClearInputActions();

        if(removingRoadEnabled)
        {
            inputManager.OnMouseClick += (pos) =>

            {
                ProcessInputAndCall(placementManager.RemoveRoadObject, pos);
            };

            inputManager.OnMouseUp += roadManager.FinishPlacingRoad;
            inputManager.OnMouseHold += (pos) =>
            {
                ProcessInputAndCall (placementManager.RemoveRoadObject, pos);
            };
        }

        inputManager.OnEscape += HandleEscape;

        // inputManager.OnMouseClick += (pos) =>
        // {
        //     ProcessInputAndCall(placementManager.RemoveRoadObject, pos);
        // };
        // inputManager.OnMouseUp += roadManager.FinishPlacingRoad;
        // inputManager.OnMouseHold += (pos) =>
        // {
        //     ProcessInputAndCall(placementManager.RemoveRoadObject, pos);
        // };
        // inputManager.OnEscape += HandleEscape;
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

        if(placingRoadEnabled)
        {
            inputManager.OnMouseClick += (pos) =>
            {
                ProcessInputAndCall(roadManager.PlaceRoad, pos);
            };
            inputManager.OnMouseUp += roadManager.FinishPlacingRoad;
            inputManager.OnMouseHold += (pos) =>
            {
                ProcessInputAndCall (roadManager.PlaceRoad, pos);
            };
        }

        // inputManager.OnMouseClick += (pos) =>
        // {
        //     ProcessInputAndCall(roadManager.PlaceRoad, pos);
        // };
        // inputManager.OnMouseUp += roadManager.FinishPlacingRoad;
        // inputManager.OnMouseHold += (pos) =>
        // {
        //     ProcessInputAndCall(roadManager.PlaceRoad, pos);
        // };
        // inputManager.OnEscape += HandleEscape;
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
    



    
}
