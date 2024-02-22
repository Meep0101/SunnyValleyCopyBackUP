using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.UI;

public class CameraSettingsItem : MonoBehaviour
{
   [HideInInspector] public Image img;
   [HideInInspector] public Transform trans;

   CameraSettings cameraSettings;
   Button button;
   int index;

   void Awake ()
   {
        img = GetComponent<Image> ();
        trans = transform;

        cameraSettings = trans.parent.GetComponent<CameraSettings>();
        index = trans.GetSiblingIndex () -1;

        button = GetComponent<Button> ();
        button.onClick.AddListener (OnItemClick);
   }

   void OnItemClick()
   {
      cameraSettings.OnItemClick(index);
   }

   void OnDestroy()
   {
      button.onClick.RemoveListener (OnItemClick);
   }
}
