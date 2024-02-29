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

    bool roadButtonEnabled = true;
    bool removeButtonEnabled = true;

    private void Start()
    {
        buttonList = new List<Button> { placeHouseButton, placeRoadButton, placeSpecialButton };

        placeRoadButton.onClick.AddListener(() =>
        {

            roadButtonEnabled =!roadButtonEnabled;
            UpdateButtonState(placeRoadButton, roadButtonEnabled);

            if (roadButtonEnabled)
            {
                OnRoadPlacement?.Invoke();
            }
            

        });
        removeRoadButton.onClick.AddListener(() =>
        {
            removeButtonEnabled = !removeButtonEnabled;
            UpdateButtonState(removeRoadButton, removeButtonEnabled);

            if (removeButtonEnabled)
                OnRemoveRoad?.Invoke();


               });
       
    }

    private void UpdateButtonState(Button button, bool isEnabled)
    {
       var outline = button.GetComponent<Outline>();
       outline.effectColor = isEnabled ? outlineColor : Color.black;
       outline.enabled = isEnabled;

    }

    private void ModifyOutline(Button button)
    {
        var outline = button.GetComponent<Outline>();
        outline.effectColor = outlineColor;
        outline.enabled = true;
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
