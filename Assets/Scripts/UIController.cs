﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

public class UIController : MonoBehaviour
{
    public Action OnRoadPlacement, OnRemoveRoad, OnHousePlacement, OnSpecialPlacement;
    public Button placeRoadButton, removeRoadButton, placeHouseButton, placeSpecialButton;

    public Color outlineColor;
    List<Button> buttonList;

    internal bool roadButtonEnabled = true; // Change access modifier to internal
    internal bool removeButtonEnabled = true; // Change access modifier to internal

    public RoadManager roadManager; // Reference to the RoadManager
    public GameObject gameOverPanel;
    public Text numberOfDaysText;
    public Text numberOfTreesText;
    public Text numberOfVehiclesText;
    public PlacementManager placementManager;
    public Text gameOverMessageText;
    public Button pauseButton, playButton;
    public Action OnPause, OnPlay;
    private bool isPaused = false;
    public PassengerCounter passengerCounter;
    public Text passengerText;
    public GameManager gameManager;
    public BlueAI blueAI;
    public GameObject successPanel;
    public Text SpassengerText;
    public Text SDaysText;
    public Text SnumberOfTreesText;
    public Text SnumberOfVehiclesText;
   

    


    public bool RoadButtonEnabled
    {
        get { return roadButtonEnabled; }
    }

    private void Start()
    {
        buttonList = new List<Button> { placeHouseButton, placeRoadButton, placeSpecialButton, removeRoadButton };

        placeRoadButton.onClick.AddListener(() => ToggleButton(placeRoadButton, ref roadButtonEnabled, OnRoadPlacement));
        removeRoadButton.onClick.AddListener(() => ToggleButton(removeRoadButton, ref removeButtonEnabled, OnRemoveRoad));
        placeHouseButton.onClick.AddListener(() => ToggleButton(placeHouseButton, ref roadButtonEnabled, OnHousePlacement));
        placeSpecialButton.onClick.AddListener(() => ToggleButton(placeSpecialButton, ref removeButtonEnabled, OnSpecialPlacement));

        pauseButton.onClick.AddListener(PauseGame);
        playButton.onClick.AddListener(ResumeGame);
        
        // Initialize the roadManager reference
        roadManager = FindObjectOfType<RoadManager>();
    }

    private void Update()
    {
        // Check if passenger count reaches 10
        int totalPassengers = passengerCounter.GetTotalPassengers();
        if (totalPassengers >= 10)
        {
            ShowSuccessPanel();
        }

        if (isPaused || successPanel.activeSelf || gameOverPanel.activeSelf)
        {
            Time.timeScale = 0f;
        }
        else 
        {
            Time.timeScale = 1f;
        }

        // if (isPaused)
        // {
        //     // Handle paused state
        //     Time.timeScale = 0f; // Pause the game
        // }
        // else
        // {
        //     // Handle running state
        //     Time.timeScale = 1f; // Resume normal time scale
        // }
    }

    private void PauseGame()
    {
        isPaused = true;
    }
    private void ResumeGame()
    {
        isPaused = false;
    }

    private void ShowSuccessPanel()
    {
        

        if (successPanel != null)
        {
            successPanel.SetActive(true);
            UpdateSuccessPanel(gameManager.SnumberOfDays, placementManager.GetTreeCount(), BlueAI.GetTotalCarsSpawned(), gameManager.numberOfDays);
            
        }
        else{
            Debug.LogError("WALA SIYA SA INSPECTOR");
        }
    }

    private void UpdateSuccessPanel(int SnumberOfDays, int numberOfTrees, int numberOfVehicles, int numberOfDays)
    {
       
        // Update Text UI elements with game statistics
    SpassengerText.text = passengerCounter.GetTotalPassengers().ToString();
    SDaysText.text = numberOfDays.ToString();
    SnumberOfVehiclesText.text = BlueAI.GetTotalCarsSpawned().ToString();
    SnumberOfTreesText.text = placementManager.GetTreeCount().ToString();
    }
            

       private void ToggleButton(Button button, ref bool isEnabled, Action toggleAction = null)
    {
        isEnabled = !isEnabled;
        UpdateButtonState(button, isEnabled);

        if (isEnabled && toggleAction != null)
        {
            toggleAction.Invoke(); // Invoke the corresponding action if enabled
        }
    }

    private void UpdateButtonState(Button button, bool isEnabled)
    {
        var outline = button.GetComponent<Outline>();
        outline.effectColor = isEnabled ? outlineColor : Color.black;
        outline.enabled = isEnabled;
    }

    public void ResetButtonColor()
    {
        foreach (Button button in buttonList)
        {
            button.GetComponent<Outline>().enabled = false;
        }

        roadButtonEnabled = true;
        removeButtonEnabled = true;
    }

    public void UpdateGameOverPanel(int numberOfDays, int numberOfTrees, int numberOfVehicle, GameManager.GameOverCause gameOverCause)
    {
        int actualTreeCount = placementManager.GetTreeCount();
        int totalPassengers = passengerCounter.GetTotalPassengers();
        int totalCarsSpawned = BlueAI.GetTotalCarsSpawned();


    numberOfDaysText.text = numberOfDays.ToString();
    numberOfTreesText.text = actualTreeCount.ToString();
    numberOfVehiclesText.text = totalCarsSpawned.ToString();
    passengerText.text = totalPassengers.ToString();
   
        

        


      switch (gameOverCause)
      {
        case GameManager.GameOverCause.CarbonMeterFull:
        gameOverMessageText.text = "Cabon Meter Reached 100%";
        break;

        case GameManager.GameOverCause.OverCrowdedStations:
        gameOverMessageText.text = "OverCrowded";
        break;
        //  case GameManager.GameOverCause.Both:
        //     gameOverMessageText.text = "Both Carbon Meter and Overcrowding";
        //     break;

        default:
        gameOverMessageText.text = "end";
        break;
      }
    
    }
    

    public void RestartButton()
    {
        Time.timeScale = 1f;
        BlueAI.totalCarsSpawned = 0;
       

        // Get the current active scene
        Scene currentScene = SceneManager.GetActiveScene();

        // Restart the scene based on its name
        switch (currentScene.name)
        {
            case "Map 1 (BGC)":
                SceneManager.LoadScene("Map 1 (BGC)");
                break;
            case "Map 2 (Cebu)":
                SceneManager.LoadScene("Map 2 (Cebu)");
                break;
            case "Map 3 (Bulacan)":
                SceneManager.LoadScene("Map 3 (Bulacan)");
                break;
            case "Map 4 (Davao)":
                SceneManager.LoadScene("Map 4 (Davao)");
                break;
            case "Map 5 (Baguio)":
                SceneManager.LoadScene("Map 5 (Baguio)");
                break;
           
            default:
                Debug.LogWarning("Scene not recognized for restart.");
                break;
        }
    }

    

}
