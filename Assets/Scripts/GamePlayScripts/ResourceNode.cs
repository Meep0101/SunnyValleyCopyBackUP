using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey;
using CodeMonkey.Utils;

public class ResourceNode {

    private Transform stationNodeTransform;
    private GameResources.StationType stationType;

    //Amount of Station
    private int stationAmount; //Resource amount field to reresent the amount of resources present in a node
    private int stationAmountMax;

    public ResourceNode(Transform stationNodeTransform, GameResources.StationType stationType) {
        this.stationNodeTransform = stationNodeTransform;
        this.stationType = stationType;
        stationAmountMax = 3; //Starts with 3 passenger
        stationAmount = stationAmountMax;

        FunctionPeriodic.Create(RegenerateSinglePassengerAmount, 2f);  //code monkey utilities

        CMDebug.TextUpdater(() => "" + stationAmount, Vector3.zero, stationNodeTransform);  //Displays current stationAmount, code monkey utilities
    }

    public Vector3 GetPosition(){
        return stationNodeTransform.position; //Doing this so that we can interface directly with the object and never have to deal with transforms
    }

    public GameResources.StationType GetStationType(){
        return stationType;
    }

    //Decreases Amount of Station Passenger
    public GameResources.StationType GrabResource(){
        stationAmount -= 1; // Decrease the resource amount

        //Swap sprites showing decrease visual
        if (stationAmount <= 0) {
            switch (stationType) {
                default:
                case GameResources.StationType.Red:
                    stationNodeTransform.GetComponent<SpriteRenderer>().sprite = Assets.i.goldNodeDepletedSprite;
                    break;
                case GameResources.StationType.Blue:
                    stationNodeTransform.GetComponent<SpriteRenderer>().sprite = Assets.i.treeNodeDepletedSprite;
                    break;
                case GameResources.StationType.Yellow:
                    stationNodeTransform.GetComponent<SpriteRenderer>().sprite = Assets.i.treeNodeDepletedSprite;
                    break;
            }
        }
        return stationType;

        //CMDebug.TextPopupMouse("stationAmount: " + stationAmount);   //using CodeMonkey utilities
    }

    public bool HasPassengers() {
        return stationAmount > 0;
    }

    private void ResetResourceAmount() {
        stationAmount = stationAmountMax;
        UpdateSprite();
    }

    //Regeneration of passenger by 1
    private void RegenerateSinglePassengerAmount() {
        if (stationAmount < stationAmountMax) {
            stationAmount++;
            UpdateSprite();
        }
    }

    private void UpdateSprite() {
        if (stationAmount > 0) {
            // Node has resources
            switch(stationType){
            default:
            case GameResources.StationType.Red:
                stationNodeTransform.GetComponent<SpriteRenderer>().sprite = Assets.i.goldNodeSprite;      //Using CodeMoney Utilities - GameAssets
                break;
            case GameResources.StationType.Blue:
                stationNodeTransform.GetComponent<SpriteRenderer>().sprite = Assets.i.treeNodeSprite;      //Using CodeMoney Utilities - GameAssets
                break;
            case GameResources.StationType.Yellow:
                stationNodeTransform.GetComponent<SpriteRenderer>().sprite = Assets.i.treeNodeSprite;      //Using CodeMoney Utilities - GameAssets
                break;
            }
        } else {
            // Node is depleted
            switch(stationType){
            default:
            case GameResources.StationType.Red:
                stationNodeTransform.GetComponent<SpriteRenderer>().sprite = Assets.i.goldNodeDepletedSprite;      //Using CodeMoney Utilities - GameAssets
                break;
            case GameResources.StationType.Blue:
                stationNodeTransform.GetComponent<SpriteRenderer>().sprite = Assets.i.treeNodeDepletedSprite;      //Using CodeMoney Utilities - GameAssets
                break;
            case GameResources.StationType.Yellow:
                stationNodeTransform.GetComponent<SpriteRenderer>().sprite = Assets.i.treeNodeDepletedSprite;      //Using CodeMoney Utilities - GameAssets
                break;
            }
        }
    }

}
