using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StationNode
{

    private Transform stationNodeTransform;

    //Amount of Station
    private int stationAmount;
    private int stationAmountMax;

    public StationNode(Transform stationNodeTransform)
    {
        this.stationNodeTransform = stationNodeTransform;
        stationAmountMax = 3;
        stationAmount = stationAmountMax;


    }

    public Vector3 GetPosition(){
        return stationNodeTransform.position;
    }

    // Decreases Amount of Station Passenger
    public void GrabPassenger(){
        stationAmount -= 1;
        Debug.Log("Passenger left: " + stationAmount);
    }


    public bool HasResources() {
        return stationAmount > 0;
    }

    private void ResetResourceAmount() {
        stationAmount = stationAmountMax;
        //UpdateSprite();
    }

    //Regeneration of passenger by 1
    private void RegenerateSingleResourceAmount() {
        if (stationAmount < stationAmountMax) {
            stationAmount++;
            //UpdateSprite();
        }
    }



}
