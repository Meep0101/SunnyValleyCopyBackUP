// using System;
// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.UI;

// public class UIController : MonoBehaviour
// {
//     public Action OnRoadPlacement, OnRemoveRoad, OnHousePlacement, OnSpecialPlacement;
//     public Button placeRoadButton, removeRoadButton, placeHouseButton, placeSpecialButton;

//     public Color outlineColor;
//     List<Button> buttonList;

//     bool roadButtonEnabled = true;
//     bool removeButtonEnabled = true;

//     public bool RoadButtonEnabled
//     {
//         get { return roadButtonEnabled; }
//     }

//     private void Start()
//     {
//         buttonList = new List<Button> { placeHouseButton, placeRoadButton, placeSpecialButton };

//         placeRoadButton.onClick.AddListener(() =>
//         {

//             roadButtonEnabled =!roadButtonEnabled;
//             ToggleButton(placeRoadButton, roadButtonEnabled);

//             if (roadButtonEnabled)
//             {
//                 OnRoadPlacement?.Invoke();
//             }
            

//         });
//         removeRoadButton.onClick.AddListener(() =>
//         {
//             removeButtonEnabled = !removeButtonEnabled;
//             UpdateButtonState(removeRoadButton, removeButtonEnabled);

//             if (removeButtonEnabled)
//                 OnRemoveRoad?.Invoke();


//                });

//                foreach (Button button in buttonList)
//                {
//                 ToggleButton(button, false);
//                }
       
//     }

//     private void ToggleButton(Button button, ref, bool isEnabled, Action toggleAction = null)
//     {

//         isEnabled = !isEnabled;
//         UpdateButtonState (button, isEnabled);

//         toggleAction?.Invoke();
//         // var outline = button.GetComponent<Outline>();
//         // outline.effectColor = isEnabled ? outlineColor : Color.black;
//         // outline.enabled = isEnabled;
//     }

//     private void UpdateButtonState(Button button, bool isEnabled)
//     {
//        var outline = button.GetComponent<Outline>();
//        outline.effectColor = isEnabled ? outlineColor : Color.black;
//        outline.enabled = isEnabled;

//     }

//     private void ModifyOutline(Button button)
//     {
//         var outline = button.GetComponent<Outline>();
//         outline.effectColor = outlineColor;
//         outline.enabled = true;
//     }

//     public void ResetButtonColor()
//     {
//         foreach (Button button in buttonList)
//         {
//             button.GetComponent<Outline>().enabled = false;
//         }

//         roadButtonEnabled = true;
//         removeButtonEnabled = true;
//     }

//     public void ToggleRoadPlacement(bool isEnabled)
//     {
//         roadButtonEnabled = isEnabled;
//         ToggleButton (placeRoadButton, roadButtonEnabled);
//     }
//     public void ToggleRemoveRoad(bool isEnabled)
//     {
//         removeButtonEnabled = isEnabled;
//         ToggleButton(removeRoadButton, removeButtonEnabled);
//     }

// }
// UIController.cs
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public Action OnRoadPlacement, OnRemoveRoad, OnHousePlacement, OnSpecialPlacement;
    public Button placeRoadButton, removeRoadButton, placeHouseButton, placeSpecialButton;

    public Color outlineColor;
    List<Button> buttonList;

    internal bool roadButtonEnabled = true; // Change access modifier to internal
    internal bool removeButtonEnabled = true; // Change access modifier to internal

    public RoadManager roadManager; // Reference to the RoadManager

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
}
