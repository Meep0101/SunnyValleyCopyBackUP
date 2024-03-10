using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class ClockSettings : MonoBehaviour
{
   [Header ("space between menu items")]
   [SerializeField] Vector2 spacing;

   Button clockButton;
   ClockSettingsItem[] clockItems;
   bool isExpanded = false;

   Vector2 clockSettingPosition;
   int itemsCount;

   void Start ()
   {
    itemsCount = transform.childCount - 1;
    clockItems = new ClockSettingsItem[itemsCount];

    for (int i = 0; i < itemsCount; i++)
    {
        clockItems [i] = transform.GetChild(i + 1).GetComponent <ClockSettingsItem>();
    }

    clockButton = transform.GetChild(0).GetComponent <Button>();
    clockButton.transform.SetAsLastSibling();
    clockButton.onClick.AddListener(ToggleClock);

    clockSettingPosition = clockButton.transform.position;

    ResetPosition ();
   }

     void ResetPosition()
    {
       for (int i = 0; i < itemsCount; i++)
       {
        clockItems [i].trans.position = clockSettingPosition;
       }
    }

    void ToggleClock()
    {
        isExpanded = !isExpanded;

        if (isExpanded)
        {
            for (int i = 0; i < itemsCount; i++)
            {
                clockItems [i].trans.position = clockSettingPosition + spacing * (i+1);
            }
        }

        else 
        {
            for (int i = 0; i < itemsCount; i++)
            {
                clockItems [i].trans.position = clockSettingPosition;
            }
        }
    }

    void OnDestroy()
    {
        clockButton.onClick.RemoveListener(ToggleClock);


    }
}
