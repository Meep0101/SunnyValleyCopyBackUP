using SimpleCity.AI;
using SVS;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
  
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
      
    }

    private void TogglePause()
    {
        isGamePaused = !isGamePaused;

        if(isGamePaused)
        {
            PauseGame();
        }
        else
        {
            ResumeGame();
        }


    }
    private void PauseGame()
    {
        if(gameOverPanel != null)
        {
            
            Time.timeScale = 0f;
        }

        else{
            Debug.LogError("Game Panel not assigned");
        }
    }

    private void ResumeGame()
    {
        if (gameOverPanel != null)
        {
            
            Time.timeScale = 1f;
        }
        else
        {
            Debug.LogError("Game Panel not assigned");
        }
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
        uiController.UpdateGameOverPanel(numberOfDays, GetNumberOfTrees(), GetNumberOfVehicles(), gameOverCause);
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
   
   public int GetNumberOfVehicles()
   {
        return numberOfVehicle;
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
