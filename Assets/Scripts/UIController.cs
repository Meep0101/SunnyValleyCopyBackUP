using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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

        // Initialize the roadManager reference
        roadManager = FindObjectOfType<RoadManager>();
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

    public void UpdateGameOverPanel(int numberOfDays, int numberOfTrees, int numberOfVehicle)
    {
        
        numberOfDaysText.text = "Number of Days: " + numberOfDays.ToString();
        numberOfTreesText.text = "Number of Tree: " + numberOfTrees.ToString();
        numberOfVehiclesText.text = "Number of Vehicles: " + numberOfVehicle.ToString();
    }

    public void RestartButton()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("TownScene");
    }
}
