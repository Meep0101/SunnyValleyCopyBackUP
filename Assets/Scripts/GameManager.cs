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
    }

   private void ToggleRoadPlacement()
    {
        if (currentFunctionality == FunctionalityState.RoadPlacement)
        {
            currentFunctionality = FunctionalityState.None;
        }
        else
        {
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



   
}
