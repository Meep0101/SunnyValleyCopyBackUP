using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchCamera : MonoBehaviour
{
    [SerializeField]
    private InputManager inputManager;
    [SerializeField]
    private Camera isometricCamera;
    [SerializeField]
    private Camera topViewCamera;

    [SerializeField]
    private GameObject Cam;

    [SerializeField]
    private ScrollAndPinch scrollAndPinchScript;

    [SerializeField]
    private PinchZoom pinchZoomScript;
   
//

    private void Start()
    {
        pinchZoomScript = topViewCamera.GetComponent<PinchZoom>();
        scrollAndPinchScript = Cam.GetComponent<ScrollAndPinch>();
    }


    public void OnButtonClick()
    {
        if (inputManager != null)
        {
            // Instead of checking for ActiveCamera, let's directly switch cameras
            if (inputManager.IsCurrentCamera(isometricCamera))
            {
                topViewCamera.gameObject.SetActive(true);
                isometricCamera.gameObject.SetActive(false);
                Cam.gameObject.SetActive(false);
                
                inputManager.SetActiveCamera(topViewCamera);

              
            }

            else
            {
                topViewCamera.gameObject.SetActive(false);
                isometricCamera.gameObject.SetActive(true);
                Cam.gameObject.SetActive(true);
                inputManager.SetActiveCamera(isometricCamera);

           
            }
        }
    }
}
