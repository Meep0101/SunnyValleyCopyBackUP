using System;
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

        pauseButton.onClick.AddListener(PauseButtonClicked);
        playButton.onClick.AddListener(PlayButtonClicked);


        // Initialize the roadManager reference
        roadManager = FindObjectOfType<RoadManager>();
    }

    private void PlayButtonClicked()
    {
       if(OnPlay != null)
       {
        OnPlay.Invoke();
       }
    }

    private void PauseButtonClicked()
    {
       if(OnPause != null)
       {
        OnPause.Invoke();
       }
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

        numberOfDaysText.text = numberOfDays.ToString();
        numberOfTreesText.text = actualTreeCount.ToString();
        numberOfVehiclesText.text =numberOfVehicle.ToString();
        
    
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
        SceneManager.LoadScene("TownScene");
    }
}
