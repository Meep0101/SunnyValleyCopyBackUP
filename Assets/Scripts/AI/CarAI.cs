using System;
using System.Collections;
using System.Collections.Generic;
using SimpleCity.AI;
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
    private bool completeRoundTrip = false;
    public CarbonMeter carbonMeter;
    [SerializeField]
    private CarSpawner carSpawner;

    private float laneChangeTimer = 0f;

    public GameObject carPrefab;
     private static CarAI currentCarAI;

   
[SerializeField]
private float laneChangeDuration = 2f;

  // Add a variable to track whether the car is changing lanes
private bool changingLane = false;
[SerializeField]
private float laneChangeDistance = 3f;

// Add a variable to store the target position during a lane change
private Vector3 laneChangeTarget;

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

         // You can find the CarbonMeter script in the scene if it's not assigned in the Unity Editor
        if (carbonMeter == null)
        {
            carbonMeter = FindObjectOfType<CarbonMeter>();
            if (carbonMeter == null)
            {
                Debug.LogError("CarbonMeter script not found in the scene!");
            }
        }

        currentCarAI = this;
    }

         private void OnDestroy()
    {
        // Clear the current car reference when the car is destroyed
        if (currentCarAI == this)
        {
            currentCarAI = null;
        }
    }

    public void SetPath(List<Vector3> path)
    {
        if(path.Count == 0)
        {
            Destroy(gameObject);
            return;
        }

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



    private void SetNextTargetIndex() //line 139-199 orig code
    {

        index++;
    if (index >= path.Count)
    {
        // Check if the car has completed a round trip and arrived back at the starting point
        if (completeRoundTrip && IsThisLastPathIndex())
        {
            carbonMeter.IncreaseCarbonMeter();
            // Destroy the car when it completes the round trip
            Destroy(gameObject);
            Debug.Log("Car destroyed at the starting point.");
            return;
        }

        // Reverse the path to make the car go back
        List<Vector3> reversedPath = new List<Vector3>(path);
        reversedPath.Reverse();

        // Set the reversed path as the new path for the car
        SetPath(reversedPath);

        // Reset the index to start from the beginning of the reversed path
        index = 0;
        completeRoundTrip = true;

       
        
        carbonMeter.IncreaseCarbonMeter();

        // Display debug message
        Debug.Log("Car is going back to the original starting point.");
        // Change the rotation of the car when going back to the starting point
        
    }
    else
    {
        currentTargetPosition = path[index];
    }
    }


}



        


    

   
