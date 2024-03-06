using UnityEngine;
using UnityEngine.EventSystems;
public class ScrollAndPinch : MonoBehaviour
{
#if  UNITY_ANDROID
    public Camera Camera;
    public bool Rotate;
    protected Plane Plane;
    public float DecreaseCameraPanSpeed = 2; //Default speed is 1
    public float CameraUpperHeightBound; //Zoom out
    public float CameraLowerHeightBound; //Zoom in
   

    private Vector3 cameraStartPosition;

  
    private void Awake()
    {
        if (Camera == null)
            Camera = Camera.main;

        cameraStartPosition = Camera.transform.position;
    }

    public void Update()
    {
    

        //Update Plane
        for (int i = 0; i<= Input.touchCount; i++)
            Plane.SetNormalAndPosition(transform.up, transform.position);

        var Delta1 = Vector3.zero;
        var Delta2 = Vector3.zero;

        //Scroll (Pan function)
        if (Input.touchCount >= 1)
        {
            //Get distance camera should travel
            Delta1 = PlanePositionDelta(Input.GetTouch(0))/DecreaseCameraPanSpeed;
            if (Input.GetTouch(0).phase == TouchPhase.Moved)
                Camera.transform.Translate(Delta1, Space.World);
        }

        //Pinch (Zoom Function)
        if (Input.touchCount >= 2)
        {
            var pos1 = PlanePosition(Input.GetTouch(0).position);
            var pos2 = PlanePosition(Input.GetTouch(1).position);
            var pos1b = PlanePosition(Input.GetTouch(0).position - Input.GetTouch(0).deltaPosition);
            var pos2b = PlanePosition(Input.GetTouch(1).position - Input.GetTouch(1).deltaPosition);

        
            
            // //Rotation Function
            if (Rotate && pos2b != pos2)
               Camera.transform.RotateAround(pos1, Plane.normal, Vector3.SignedAngle(pos2 - pos1, pos2b - pos1b, Plane.normal));
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

            float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

            float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

            // Adjust the pinch zoom speed according to your preference
            float pinchZoomSpeed = 0.01f;

            // Use the deltaMagnitudeDiff to scale the camera size or field of view
            if (Camera.orthographic)
            {
                Camera.orthographicSize += deltaMagnitudeDiff * pinchZoomSpeed;
                Camera.orthographicSize = Mathf.Max(Camera.orthographicSize, 0.1f);
            }
            else
            {
                Camera.fieldOfView += deltaMagnitudeDiff * pinchZoomSpeed;
                Camera.fieldOfView = Mathf.Clamp(Camera.fieldOfView, 0.1f, 179.9f);
            }
                
        }

    }

    //Returns the point between first and final finger position
    protected Vector3 PlanePositionDelta(Touch touch)
    {
        //not moved
        if (touch.phase != TouchPhase.Moved)
            return Vector3.zero;

        
        //delta
        var rayBefore = Camera.ScreenPointToRay(touch.position - touch.deltaPosition);
        var rayNow = Camera.ScreenPointToRay(touch.position);
        if (Plane.Raycast(rayBefore, out var enterBefore) && Plane.Raycast(rayNow, out var enterNow))
            return rayBefore.GetPoint(enterBefore) - rayNow.GetPoint(enterNow);

        //not on plane
        return Vector3.zero;
    }

    protected Vector3 PlanePosition(Vector2 screenPos)
    {
        //position
        var rayNow = Camera.ScreenPointToRay(screenPos);
        if (Plane.Raycast(rayNow, out var enterNow))
            return rayNow.GetPoint(enterNow);

        return Vector3.zero;
    }



    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, transform.position + transform.up);
    }

   
#endif
}

