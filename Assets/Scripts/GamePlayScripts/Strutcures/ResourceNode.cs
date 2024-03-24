using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey;
using CodeMonkey.Utils;
using System;

public class ResourceNode {

    private Transform stationNodeTransform;
    private GameResources.StationType stationType;

    //Amount of Station
    public int stationAmount; //Resource amount field to represent the amount of resources present in a node
    public int stationAmountMax;
    private StationBar stationBar;

    
    
    public ResourceNode(Transform stationNodeTransform, GameResources.StationType stationType, StationBar stationBar) 
    {
        this.stationNodeTransform = stationNodeTransform;
        this.stationType = stationType;
        stationAmountMax = 10; //Starts with 3 passenger
        stationAmount = stationAmountMax;
        stationAmount = 0;

        IncreaseStationAmount();


        FunctionPeriodic.Create(RegenerateSinglePassengerAmount, 5f);  //code monkey utilities

        CMDebug.TextUpdater(() => "" + stationAmount, Vector3.zero, stationNodeTransform);  //Displays current stationAmount, code monkey utilities
    
        this.stationBar = stationBar;
        stationBar.SetResourceNode(this);
    
    }

    private void IncreaseStationAmount()
    {
        FunctionPeriodic.Create(() => 
        {
            if (stationAmount < stationAmountMax)
            {
                stationAmount++;
            }
        }, 5f); //3f is speed of the passenger spawn
    }
    // private void DecreaseStationAmount()
    // {
    //     FunctionPeriodic.Create(() => 
    //     {
    //         if (stationAmount < stationAmountMax)
    //         {
    //             stationAmount--;
    //         }
    //     }, 5f); //3f is speed of the passenger spawn
    // }

    public Vector3 GetPosition(){
        return stationNodeTransform.position; //Doing this so that we can interface directly with the object and never have to deal with transforms
    }

    public GameResources.StationType GetStationType(){
        return stationType;
    }

    //Decreases Amount of Station Passenger
    public GameResources.StationType GrabResource(StationBar stationBar, int maxPassengerHold){
        stationAmount -= maxPassengerHold; // Decrease the resource amount by maxPassengerHold
    stationBar.DecreaseSliderValue(maxPassengerHold);
        //Swap sprites showing decrease visual
        // if (stationAmount <= 0) {
        //     switch (stationType) {
        //         default:
        //         case GameResources.StationType.Red:
        //             stationNodeTransform.GetComponent<SpriteRenderer>().sprite = Assets.i.goldNodeDepletedSprite;
        //             break;
        //         case GameResources.StationType.Blue:
        //             stationNodeTransform.GetComponent<SpriteRenderer>().sprite = Assets.i.treeNodeDepletedSprite;
        //             break;
        //         case GameResources.StationType.Yellow:
        //             stationNodeTransform.GetComponent<SpriteRenderer>().sprite = Assets.i.treeNodeDepletedSprite;
        //             break;
        //     }
        // }
        // Debug.Log("May nakuha na");
        return stationType;
        

        //CMDebug.TextPopupMouse("stationAmount: " + stationAmount);   //using CodeMonkey utilities
    }

    public bool HasPassengers() {
        return stationAmount > 0;
    }

    private void ResetResourceAmount() {
        stationAmount = stationAmountMax;
       //UpdateSprite();
    }

    //Regeneration of passenger by 1
    private void RegenerateSinglePassengerAmount() {
        if (stationAmount < stationAmountMax) {
            stationAmount++;
            //UpdateSprite();
        }
    }

    private void UpdateSprite() {
        // if (stationAmount > 0) {
        //     // Node has resources
        //     switch(stationType){
        //     default:
        //     case GameResources.StationType.Red:
        //         stationNodeTransform.GetComponent<SpriteRenderer>().sprite = Assets.i.goldNodeSprite;      //Using CodeMoney Utilities - GameAssets
        //         break;
        //     case GameResources.StationType.Blue:
        //         stationNodeTransform.GetComponent<SpriteRenderer>().sprite = Assets.i.treeNodeSprite;      //Using CodeMoney Utilities - GameAssets
        //         break;
        //     case GameResources.StationType.Yellow:
        //         stationNodeTransform.GetComponent<SpriteRenderer>().sprite = Assets.i.treeNodeSprite;      //Using CodeMoney Utilities - GameAssets
        //         break;
        //     }
        // } else {
        //     // Node is depleted
        //     switch(stationType){
        //     default:
        //     case GameResources.StationType.Red:
        //         stationNodeTransform.GetComponent<SpriteRenderer>().sprite = Assets.i.goldNodeDepletedSprite;      //Using CodeMoney Utilities - GameAssets
        //         break;
        //     case GameResources.StationType.Blue:
        //         stationNodeTransform.GetComponent<SpriteRenderer>().sprite = Assets.i.treeNodeDepletedSprite;      //Using CodeMoney Utilities - GameAssets
        //         break;
        //     case GameResources.StationType.Yellow:
        //         stationNodeTransform.GetComponent<SpriteRenderer>().sprite = Assets.i.treeNodeDepletedSprite;      //Using CodeMoney Utilities - GameAssets
        //         break;
        //     }
        // }
    }

}
