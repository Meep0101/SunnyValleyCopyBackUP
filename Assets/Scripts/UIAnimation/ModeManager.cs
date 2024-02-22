using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModeManager : MonoBehaviour
{
    public RoadManager roadManager;
    public ScrollAndPinch scrollAndPinch;
    

    private bool cameraMode = true;
    private int touchCount = 0;
    
  
  

    // void Update()
    // {
    //     if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
    //         {
    //             cameraMode = !cameraMode;

    //             scrollAndPinch.enabled = !cameraMode;
    //             roadManager.enabled = cameraMode;
    //         }
    // }
 
 void Update()
    {
        if (Input.touchCount > 0)
        {
            // Check for the first touch
            if (Input.GetTouch(0).phase == TouchPhase.Began)
            {
                touchCount++;

                if (touchCount == 1)
                {
                    // Disable camera movement on the first touch
                    scrollAndPinch.enabled = false;
                }
                else if (touchCount == 2)
                {
                    // Enable camera rotation on the second touch
                    EnableCameraRotation();
                    touchCount = 0; // Reset touch count after activating camera rotation
                }
            }
        }
    }

    void EnableCameraRotation()
    {
        scrollAndPinch.enabled = true;
        roadManager.enabled = false;
    }
}
