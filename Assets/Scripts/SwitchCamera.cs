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
//
    public void OnButtonClick()
    {
        if (inputManager != null)
        {
            // Instead of checking for ActiveCamera, let's directly switch cameras
            if (inputManager.IsCurrentCamera(isometricCamera))
            {
                inputManager.SetActiveCamera(topViewCamera);
            }
            else
            {
                inputManager.SetActiveCamera(isometricCamera);
            }
        }
    }
}
