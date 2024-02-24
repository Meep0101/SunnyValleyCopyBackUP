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

    public CarbonMeter carbonMeter;
    
    [SerializeField]
    public AiDirector aiDirector;

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
        if (aiDirector == null)
        {
            aiDirector = FindObjectOfType<AiDirector>();
            if (aiDirector == null)
            {
                Debug.LogError("AiDirector script not found in the scene!");
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



    //private bool home = false;
    private void SetNextTargetIndex()
    {
        
        index++;      
        if(index >= path.Count) //&& home == false)
        {
            carbonMeter.IncreaseCarbonMeter(); // Increase carbon meter value
            Stop = true;
            Destroy(gameObject);
            Debug.Log("Nagdestroy na ba ang ferson?");
            
            aiDirector.RespawnACar();
            //Stop = true;
            //Destroy(gameObject);
            //carbonMeter.IncreaseCarbonMeter(); // Increase carbon meter value
            //Debug.Log("MAGDESTROY KA");
                    
            //home = true;
                

        }    

        else
        {
            currentTargetPosition = path[index];
        }


    }
}
