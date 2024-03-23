using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey;
using CodeMonkey.Utils;

public class StorageNode {
    
    private Vector3Int storageNodeTransform;
    private GameResources.StationType storageType;

    //Number of vehicle in the storage
    private int vehicleNum; //Resource amount field to reresent the amount of resources present in a node
    private int vehicleMax = 2;

    public int VehicleNum // Public property
    {
        get { return vehicleNum; }
        private set { vehicleNum = value; }
    }
    public bool IsClicked { get; private set; } // Property to track if the terminal is clicked



    public StorageNode (Vector3Int storageNodeTransform, GameResources.StationType storageType) {
        this.storageNodeTransform = storageNodeTransform;
        this.storageType = storageType;
        vehicleMax = 2; 
        vehicleNum = 0; //Starts with 0 vehicle

        //FunctionPeriodic.Create(RegenerateSingleVehicleAmount, 2f);  //code monkey utilities

        //CMDebug.TextUpdater(() => "" + vehicleNum, Vector3.zero, storageNodeTransform);  //Displays current vehicleNum, code monkey utilities
    }

    public Vector3 GetAPosition(){
        return storageNodeTransform; //Doing this so that we can interface directly with the object and never have to deal with transforms
    }

    public GameResources.StationType GetStorageType(){
        return storageType;
    }

    //Increases Amount of Station Passenger
    public GameResources.StationType GrabResource(){
        vehicleNum -= 1; // Increase the numner of vehicle

        //Swap sprites showing decrease visual
        // if (vehicleNum <= 0) {
        //     switch (storageType) {
        //         default:
        //         case GameResources.StationType.Red:
        //             storageNodeTransform.GetComponent<SpriteRenderer>().sprite = Assets.i.goldNodeDepletedSprite;
        //             break;
        //         case GameResources.StationType.Blue:
        //             storageNodeTransform.GetComponent<SpriteRenderer>().sprite = Assets.i.treeNodeDepletedSprite;
        //             break;
        //         case GameResources.StationType.Yellow:
        //             storageNodeTransform.GetComponent<SpriteRenderer>().sprite = Assets.i.treeNodeDepletedSprite;
        //             break;
        //     }
        // }
        return storageType;

        //CMDebug.TextPopupMouse("vehicleNum: " + vehicleNum);   //using CodeMonkey utilities
    }

    public bool HasVehicles() {
        return vehicleNum > 0;
    }

    private void ResetVehicleAmount() {
        vehicleNum = 0;
        //UpdateSprite();
    }

    //Regeneration of vehicle by 1 NOT SURE IF NEED
    private void RegenerateSingleVehicleAmount() {
        if (vehicleNum < vehicleMax) {
            vehicleNum++;
            //UpdateSprite();
        }
    }


    private void DecrementCount(int count)
    {
        VehicleNum-= count;
        Debug.Log("Current count: " + VehicleNum);
        IsClicked = false;
        //VehiclesTab.SetActive(false);
    }


}
