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

    //     index++;
    // if (index >= path.Count)
    // {
    //     // Check if the car has completed a round trip and arrived back at the starting point
    //     if (completeRoundTrip && IsThisLastPathIndex())
    //     {
    //         carbonMeter.IncreaseCarbonMeter();
    //         // Destroy the car when it completes the round trip
    //         Destroy(gameObject);
    //         Debug.Log("Car destroyed at the starting point.");
    //         return;
    //     }

    //     // Reverse the path to make the car go back
    //     List<Vector3> reversedPath = new List<Vector3>(path);
    //     reversedPath.Reverse();

    //     // Set the reversed path as the new path for the car
    //     SetPath(reversedPath);

    //     // Reset the index to start from the beginning of the reversed path
    //     index = 0;
    //     completeRoundTrip = true;

       
        
    //     carbonMeter.IncreaseCarbonMeter();

    //     // Display debug message
    //     Debug.Log("Car is going back to the original starting point.");
    //     // Change the rotation of the car when going back to the starting point
        
    // }
    // else
    // {
    //     currentTargetPosition = path[index];

    //      if (completeRoundTrip && IsThisLastPathIndex())
    //     {
    //         carbonMeter.IncreaseCarbonMeter();
    //         Destroy(gameObject);
    //         Debug.Log("Car destroyed at the starting point.");
            
    //     }
        
      index++;

        if (index >= path.Count)
        {
            // Destroy the car when it completes the round trip
            Destroy(gameObject);
            Debug.Log("Car destroyed at the end point.");

            // Respawn the car at the end of the road
            RespawnCarAtEnd();
            

            return;
        }

        currentTargetPosition = path[index];

        if (completeRoundTrip && IsThisLastPathIndex())
        {
            carbonMeter.IncreaseCarbonMeter();
            Destroy(gameObject);
            Debug.Log("Car destroyed at the end point.");
        }
    }
    

    private void RespawnCarAtEnd()
    {
        
      // Get the mirrored end point
    Vector3 mirroredEndPoint = GetMirroredEndPoint();

    // Ensure that the mirrored end point is within the bounds of the road
    //RoadHelper roadHelper = GetComponent<RoadHelper>();
   // mirroredEndPoint = roadHelper.GetAdjacentLanePosition(mirroredEndPoint);

    // Instantiate a new car at the corrected end point of the road
    GameObject newCar = Instantiate(carPrefab, mirroredEndPoint, Quaternion.identity);

    // Set the path for the new car to drive back to the starting point
    List<Vector3> reversePath = new List<Vector3>(path);
    reversePath.Reverse();
    
    // Adjust the new car's position to be slightly above the road
    newCar.transform.position += Vector3.up * 0.1f;

    CarAI newCarAI = newCar.GetComponent<CarAI>();
    if (newCarAI != null)
    {
        newCarAI.SetPath(reversePath);
    }
    else
    {
        Debug.LogError("CarAI component not found on the spawned car.");
    }
    }


        private Vector3 GetMirroredEndPoint()
    {
         // Assuming there is a RoadHelper component attached to the road GameObject
    RoadHelper roadHelper = GetComponent<RoadHelper>();

    // Get the mirrored endpoint
    Vector3 mirroredEndPoint = roadHelper.GetAdjacentLanePosition(transform.position);

    // Get the rotation of the road at the endpoint
    Quaternion roadRotation = transform.rotation;

    // Reverse the original path to create the mirrored path
    List<Vector3> mirroredPath = new List<Vector3>(path);
    mirroredPath.Reverse();

    // Spawn the car at the mirrored endpoint with the correct rotation
    GameObject newCar = Instantiate(carPrefab, mirroredEndPoint, roadRotation);

    // Set the reversed path for the mirrored road
    CarAI carAI = newCar.GetComponent<CarAI>();
    if (carAI != null)
    {
        carAI.SetPath(mirroredPath);
    }
    else
    {
        Debug.LogError("CarAI component not found on the spawned car.");
    }

    // Return the mirrored endpoint
    return mirroredEndPoint;
    }
}



        


    

   

