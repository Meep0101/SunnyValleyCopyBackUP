using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CarAI : MonoBehaviour
{
    [SerializeField]
    private List<Vector3> path = null;
    [SerializeField]
    private float arriveDistance = .3f, lastPointArriveDistance = .1f;
    [SerializeField]
    private float turningAngleOffset = 5;
    [SerializeField]
    private Vector3 currentTargetPosition;

    [SerializeField]
    private GameObject raycastStartingPoint = null;
    [SerializeField]
    private float collisionRaycastLength = 0.1f;

    private int CarbonMeter = 0;



    internal bool IsThisLastPathIndex()
    {
        return index >= path.Count-1;
    }

    private int index = 0;

    private bool stop;
    private bool collisionStop = false;

    public bool Stop
    {
        get { return stop || collisionStop; }
        set { stop = value; }
    }

    [field: SerializeField]
    public UnityEvent<Vector2> OnDrive { get; set; }

    private void Start()
    {
        if(path == null || path.Count == 0)
        {
            Stop = true;
        }
        else
        {
            currentTargetPosition = path[index];
        }
    }

    public void SetPath(List<Vector3> path)
    {
        if(path.Count == 0)
        {
            Destroy(gameObject);
            return;
        }

        //path.Reverse(); //pabalik na car
        this.path = path;
        index = 0;
        currentTargetPosition = this.path[index];

        Vector3 relativepoint = transform.InverseTransformPoint(this.path[index + 1]);

        float angle = Mathf.Atan2(relativepoint.x, relativepoint.z) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0, angle, 0);
        Stop = false;
    }

    private void Update()
    {
        CheckIfArrived();
        Drive();
        CheckForCollisions();
    }

    private void CheckForCollisions()
    {
        if(Physics.Raycast(raycastStartingPoint.transform.position, transform.forward,collisionRaycastLength, 1 << gameObject.layer))
        {
            collisionStop = true;
        }
        else
        {
            collisionStop = false;
        }
    }

    private void Drive()
    {
        if (Stop)
        {
            OnDrive?.Invoke(Vector2.zero);
        }
        else
        {
            Vector3 relativepoint = transform.InverseTransformPoint(currentTargetPosition);
            float angle = Mathf.Atan2(relativepoint.x, relativepoint.z) * Mathf.Rad2Deg;
            var rotateCar = 0;
            if(angle > turningAngleOffset)
            {
                rotateCar = 1;
            }else if(angle < -turningAngleOffset)
            {
                rotateCar = -1;
            }
            OnDrive?.Invoke(new Vector2(rotateCar, 1));
        }
    }

    private void CheckIfArrived()
    {
        if(Stop == false)
        {
            var distanceToCheck = arriveDistance;
            if(index == path.Count - 1)
            {
                distanceToCheck = lastPointArriveDistance;
            }
            if(Vector3.Distance(currentTargetPosition,transform.position) < distanceToCheck)
            {
                SetNextTargetIndex();
            }
        }
    }

    private void SetNextTargetIndex()
    {
        // index++;
        // if(index >= path.Count)
        // {
        //     CarbonMeter++;
        //     Stop = true;
        //     Destroy(gameObject);
        //     Debug.Log("Nagdestroy na ba ang ferson?");
        //     Debug.Log("Carbon Emission Meter: " + CarbonMeter);
        //      CarbonEmissionUI uiManager = FindObjectOfType<CarbonEmissionUI>();
        //     if (uiManager != null)
        //     {
        //         uiManager.UpdateCarbonMeter(CarbonMeter);
        //     }
        //     else
        //     {
        //         Debug.LogError("UIManager not found in the scene.");
        //     }
        // }
        // else
        // {
        //     currentTargetPosition = path[index];
        // }


        index++;
        // if (index >= path.Count)
        // {
        //     CarbonMeter++;
        //     Stop = true;

        //     // Instantiate a new car on the other side of the road
        //     bool isGoingBack = InstantiateNewCarOnOtherSide();

        //     Destroy(gameObject);
        //     Debug.Log("Nagdestroy na ba ang ferson?");
        //     Debug.Log("Carbon Emission Meter: " + CarbonMeter);

        //     CarbonEmissionUI uiManager = FindObjectOfType<CarbonEmissionUI>();
        //     if (uiManager != null)
        //     {
        //         uiManager.UpdateCarbonMeter(CarbonMeter);
        //     }
        //     else
        //     {
        //         Debug.LogError("UIManager not found in the scene.");
        //     }

        //     // Display debug message based on whether the car is going back or not
        //     if (isGoingBack)
        //     {
        //         Debug.Log("New car instantiated on the other side, going back to the original starting point.");
        //     }
        //     else
        //     {
        //         Debug.Log("Car is not set to go back.");
        //     }
        // }
        // else
        // {
        //     currentTargetPosition = path[index];
        // }

        index++;
    if (index >= path.Count)
    {
        CarbonMeter++;
        Stop = true;

        // Reverse the path to make the car go back
        List<Vector3> reversedPath = new List<Vector3>(path);
        reversedPath.Reverse();

        // Set the reversed path as the new path for the car
        SetPath(reversedPath);

        // Reset the index to start from the beginning of the reversed path
        index = 0;

        Debug.Log("Nagdestroy na ba ang ferson?");
        Debug.Log("Carbon Emission Meter: " + CarbonMeter);
        CarbonEmissionUI uiManager = FindObjectOfType<CarbonEmissionUI>();
        if (uiManager != null)
        {
            uiManager.UpdateCarbonMeter(CarbonMeter);
        }
        else
        {
            Debug.LogError("UIManager not found in the scene.");
        }

        // Display debug message
        Debug.Log("Car is going back to the original starting point.");
    }
    else
    {
        currentTargetPosition = path[index];
    }
    }

     private bool InstantiateNewCarOnOtherSide()
    {
         // Get the last point position
    Vector3 lastPointPosition = path[path.Count - 1];

    // Calculate the position on the other side of the road
    Vector3 otherSidePosition = CalculateOtherSidePosition(lastPointPosition);

    // Debug log for checking the position
    Debug.Log("Other Side Position: " + otherSidePosition);

    // Instantiate the new car at the other side
    GameObject newCar = Instantiate(gameObject, otherSidePosition, Quaternion.identity);

    // Debug log to check if instantiation is successful
    Debug.Log("New Car Instantiated: " + newCar);

    // Set a new path for the spawned car (reverse the original path)
    List<Vector3> reversedPath = new List<Vector3>(path);
    reversedPath.Reverse();

    // Get the CarAI component from the spawned car
    CarAI newCarAI = newCar.GetComponent<CarAI>();

    // Debug log to check if CarAI component is found
    Debug.Log("New Car AI Component: " + newCarAI);

    // Check if the CarAI component is found
    if (newCarAI != null)
    {
        // Set the reversed path for the spawned car
        newCarAI.SetPath(reversedPath);
        return true; // Going back
    }
    else
    {
        // Log an error if CarAI component is not found
        Debug.LogError("CarAI component not found on the spawned car.");
        return false;
    }
}

    private Vector3 CalculateOtherSidePosition(Vector3 lastPointPosition)
    {
         return new Vector3(lastPointPosition.x, lastPointPosition.y, -lastPointPosition.z);
    }
}

