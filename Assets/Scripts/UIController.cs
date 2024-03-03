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

    private void Start()
    {
        buttonList = new List<Button> { placeHouseButton, placeRoadButton, placeSpecialButton };

        placeRoadButton.onClick.AddListener(() =>
        {
            ResetButtonColor();
            ModifyOutline(placeRoadButton);
            OnRoadPlacement?.Invoke();

        });
        removeRoadButton.onClick.AddListener(() =>
        {
            ResetButtonColor();
            ModifyOutline(removeRoadButton);
            //Trigger the removal process when the buttom is clicked
            OnRemoveRoad?.Invoke();
        });

        #region Other Placemnents
        // placeHouseButton.onClick.AddListener(() =>
        // {
        //     ResetButtonColor();
        //     ModifyOutline(placeHouseButton);
        //     OnHousePlacement?.Invoke();
        // });
        // placeSpecialButton.onClick.AddListener(() =>
        // {
        //     ResetButtonColor();
        //     ModifyOutline(placeSpecialButton);
        //     OnSpecialPlacement?.Invoke();
        // });
        // placeBigStructureButton.onClick.AddListener(() =>
        // {
        //     ResetButtonColor();
        //     ModifyOutline(placeBigStructureButton);
        //     OnBigStructurePlacement?.Invoke();
        // });
        #endregion
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
    }


}
