using System.Runtime.CompilerServices;
//using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;

public class PinchZoom : MonoBehaviour
{
    public float perspectiveZoomSpeed = 100f;        // The rate of change of the field of view in perspective mode.
    public float orthoZoomSpeed = 100f;        // The rate of change of the orthographic size in orthographic mode.
    
    public float panSpeed = 1f;
    public Camera cam;

    private Vector2 lastPanPosition;
   
 void Start()
 {
  cam = GetComponent<Camera>();
 }

    void Update()
    {
       // If there are two touches on the device...
        if (Input.touchCount == 2)
        {
            // Store both touches.
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            // Find the position in the previous frame of each touch.
            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

            // Find the magnitude of the vector (the distance) between the touches in each frame.
            float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

            // Find the difference in the distances between each frame.
            float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

        

            // If the camera is orthographic...
            if (cam.orthographic)
            {
                // ... change the orthographic size based on the change in distance between the touches.
                cam.orthographicSize += deltaMagnitudeDiff * orthoZoomSpeed;

                // Make sure the orthographic size never drops below zero.
                cam.orthographicSize = Mathf.Max(cam.orthographicSize, 0.1f);
            }
            else
            {
                // Otherwise change the field of view based on the change in distance between the touches.
                cam.fieldOfView += deltaMagnitudeDiff * perspectiveZoomSpeed;

                // Clamp the field of view to make sure it's between 0 and 180.
                cam.fieldOfView = Mathf.Clamp(cam.fieldOfView, 0.1f, 179.9f);
            }
        }

        else if (Input.touchCount == 1)
        {
            // Get the touch
            Touch touch = Input.GetTouch(0);

            // Check if the touch has just begun
            if (touch.phase == TouchPhase.Began)
            {
                lastPanPosition = touch.position;
            }
            // Check if the touch is moving
            else if (touch.phase == TouchPhase.Moved)
            {
                // Calculate the difference between the current touch position and the last one
                Vector2 delta = touch.position - lastPanPosition;

                // Update the camera position based on the touch movement
                Vector3 panDelta = new Vector3(delta.x, delta.y, 0) * panSpeed * Time.deltaTime;
                cam.transform.Translate(-panDelta);

                // Update the last touch position for the next frame
                lastPanPosition = touch.position;
            }
        
    }

    
        
    }
}
   
