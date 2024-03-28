﻿using System;
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
    //private bool completeRoundTrip = false;
    //public CarbonMeter carbonMeter;

    [SerializeField]
    public AiDirector aiDirector;
    //private CarSpawner carSpawner;

    //private float laneChangeTimer = 0f;

    //public GameObject carPrefab;
    //private static CarAI currentCarAI;

   
// [SerializeField]
// private float laneChangeDuration = 2f;

  // Add a variable to track whether the car is changing lanes
// private bool changingLane = false;
// [SerializeField]
// private float laneChangeDistance = 3f;

// Add a variable to store the target position during a lane change
//private Vector3 laneChangeTarget;

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

    // private void OnDestroy()
    // {
    //     // Clear the current car reference when the car is destroyed
    //     if (currentCarAI == this)
    //     {
    //         currentCarAI = null;
    //     }
    // }

    public void SetPath(List<Vector3> path)
    {
        if(path.Count == 0)
        {
            Destroy(gameObject);    // Set to State Idle
            return;
        }

        this.path = path;
        index = 0;
        currentTargetPosition = this.path[index];

        // Sets car position to face on the next target position
        Vector3 relativepoint = transform.InverseTransformPoint(this.path[index + 1]);  //this.path[index + 1] is the next position on the path
        float angle = Mathf.Atan2(relativepoint.x, relativepoint.z) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, angle, 0); // Sets the facing angle of spawned car. Rotates on the Y-axis
        Stop = false;
    }

    private void Update()
    {
        //Movements
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
            OnDrive?.Invoke(Vector2.zero);  // Car will stop , "?" is a listener to avoid Error exception
        }
        else    //Move toward nex target point
        {
            Vector3 relativepoint = transform.InverseTransformPoint(currentTargetPosition);
            float angle = Mathf.Atan2(relativepoint.x, relativepoint.z) * Mathf.Rad2Deg;
            var rotateCar = 0;  // x value, rotates the car. If zero = no rotation
            if(angle > turningAngleOffset)
            {
                rotateCar = 1;  //Rotate RIGHT
            }else if(angle < -turningAngleOffset)
            {
                rotateCar = -1; //Rotate LEFT
            }
            //Whether there will be rotation or none:
            OnDrive?.Invoke(new Vector2(rotateCar, 1)); // (x, y), 1 since go forward
        }
    }

    private void CheckIfArrived()
    {
        if(Stop == false)
        {
            var distanceToCheck = arriveDistance;
            if(index == path.Count - 1) // Last Point in the Path
            {
                distanceToCheck = lastPointArriveDistance;  // Switched
            }
            if(Vector3.Distance(currentTargetPosition,transform.position) < distanceToCheck)
            {
                SetNextTargetIndex();
            }
        }
    }


    private void SetNextTargetIndex() 
    {
        index++;
        if (index >= path.Count)
        {
            //carbonMeter.IncreaseCarbonMeter(); // Increase carbon meter value
            Stop = true;
            Destroy(gameObject);            //Possible to change: State.Idle()
            Debug.Log("Nagdestroy na ba ang ferson?");
            
        }
        else
        {
            currentTargetPosition = path[index];
        }
    }
}



        


    

   
