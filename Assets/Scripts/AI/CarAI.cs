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

         if (completeRoundTrip && IsThisLastPathIndex())
        {
            carbonMeter.IncreaseCarbonMeter();
            Destroy(gameObject);
            Debug.Log("Car destroyed at the starting point.");
            
        }

        
    }     
    // Change the rotation of the car when going back to the starting point
        RotateCarForLeftTurn(); //wants to go left but cant :<  line 219-227 (left turn car)
    
}
        private void RotateCarForLeftTurn()
{
   
    transform.Rotate(Vector3.up, 90f);
    Debug.Log("Turning left");
}

    // private bool ShouldChangeLane()
    // {
    //     float changeLaneDistance = 10f; // Adjust this value based on your requirements

    // return Vector3.Distance(transform.position, path[0]) < changeLaneDistance;

    // }

//     private void ChangeLaneAndTurnLeft()
//     {
//         // Assuming you have a method to get the position of the adjacent lane
//     Vector3 targetLanePosition = GetAdjacentLanePosition();

//     // Set the target position for the lane change
//     laneChangeTarget = targetLanePosition;

//     // Rotate the car for a left turn
//    //RotateCarForLeftTurn();

//     // Indicate that the car is changing lanes
//     changingLane = true;
//     }

    // private Vector3 GetAdjacentLanePosition()
    // {
    //     // Assuming lanes are spaced by a constant distance (laneWidth)
    // float laneWidth = 3.0f; // Adjust this value based on your road setup

    // // Get the current lane position
    // Vector3 currentLanePosition = transform.position;

    // // Get the direction of the road (you might need to adjust this based on your road setup)
    // Vector3 roadDirection =  path[0] - path[1];
    // roadDirection.Normalize();

    // // Calculate the position of the adjacent lane
    // Vector3 adjacentLanePosition = currentLanePosition + Vector3.Cross(Vector3.up, roadDirection) * laneWidth;

    // return adjacentLanePosition;
    // }


    //     index++;

        

    // if (index >= path.Count)
    // {
    //     Stop = true;

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

        
    // }
        
    // else
    // {
    //     currentTargetPosition = path[index];

    //     // Check if the car should change lanes when going back to the starting point
        
    //     if (completeRoundTrip && IsThisLastPathIndex())
    // {
    //     carbonMeter.IncreaseCarbonMeter();
    //     // Destroy the car when it completes the round trip
    //     Destroy(gameObject);
    //     Debug.Log("Car destroyed at the starting point.");

    //     if (path != null && path.Count > 0){
    // }

    //      // Reverse the path to make the car go back
    //     List<Vector3> reversedPath = new List<Vector3>(path);
    //     reversedPath.Reverse();

    //     // Set the reversed path as the new path for the car
    //     SetPath(reversedPath);

    //     // Reset the index to start from the beginning of the reversed path
    //     index = 0;
    // }
        
//         index++;
//     if (index >= path.Count)
//     {
//         //CarbonMeter++;
//         Stop = true;
    
//         // Reverse the path to make the car go back  //line 208-213 reverse car code
//         List<Vector3> reversedPath = new List<Vector3>(path);
//         reversedPath.Reverse();

//         // Set the reversed path as the new path for the car
//         SetPath(reversedPath);

//         // Reset the index to start from the beginning of the reversed path
//         index = 0;
//         completeRoundTrip = true;

      
//         carbonMeter.IncreaseCarbonMeter();

       

//         // Display debug message
//         Debug.Log("Car is going back to the original starting point.");
        
//    Debug.Log("Car is going back to the original starting point.");
// }
// else
// {
//     currentTargetPosition = path[index];

//     //dito dapat yung code for the change lanes

//     // Check if the car has completed a round trip and arrived back at the starting point
//     if (completeRoundTrip && IsThisLastPathIndex())
//     {
//         carbonMeter.IncreaseCarbonMeter();
//         // Destroy the car when it completes the round trip
//         Destroy(gameObject);
//         Debug.Log("Car destroyed at the starting point.");
//     }

    
        
//     }

        
}

    

   

