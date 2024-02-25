using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using CodeMonkey;
//using CodeMonkey.Utils;

public class ResourceNode {

    private Transform stationNodeTransform;
    private PeriodicFunction periodicFunction;

    //Amount of Station
    private int stationAmount;
    private int stationAmountMax;

    public ResourceNode(Transform stationNodeTransform) {
        
        this.stationNodeTransform = stationNodeTransform;
        stationAmountMax = 3;
        stationAmount = stationAmountMax;

        //FunctionPeriodic.Create(RegenerateSingleResourceAmount, 2f);  //code monkey utilities
        // Initialize periodic function with the action and interval
        periodicFunction = new PeriodicFunction(RegenerateSingleResourceAmount, 2f);
        periodicFunction.Start();

        //CMDebug.TextUpdater(() => "" + stationAmount, Vector3.zero, stationNodeTransform);  //Displays current stationAmount, code monkey utilities
    }
    

    public Vector3 GetPosition(){
        return stationNodeTransform.position;
    }

    //Decreases Amount of Station Passenger
    public void GrabResource(){
        stationAmount -= 1;

        //Swap sprites showing decrease visual
        if (stationAmount <= 0) {
            // Node is depleted
            // stationNodeTransform.GetComponent<SpriteRenderer>().sprite = GameAssets.i.goldNodeDepletedSprite;      // NILIPAT sa UpdateSprite() , Using CodeMoney Utilities - GameAssets
            //UpdateSprite();
            Debug.Log("Ubos na");
        }

        //FunctionTimer.Create(ResetResourceAmount, 5f);     //using CodeMonkey utilities

        Debug.Log("stationAmount: " + stationAmount);   //using CodeMonkey utilities
    }

    public bool HasResources() {
        return stationAmount > 0;
    }

    private void ResetResourceAmount() {
        Debug.Log("Regenerating resource");
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

    // private void UpdateSprite() {
    //     if (stationAmount > 0) {
    //         // Node has resources
    //         stationNodeTransform.GetComponent<SpriteRenderer>().sprite = GameAssets.i.goldNodeSprite;      //Using CodeMoney Utilities - GameAssets
    //     } else {
    //         // Node is depleted
    //         stationNodeTransform.GetComponent<SpriteRenderer>().sprite = GameAssets.i.goldNodeDepletedSprite;      //Using CodeMoney Utilities - GameAssets
    //     }
    // }

}
