using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
public class BlueAI : MonoBehaviour
{
    private enum State {
        Idle,
        MovingToResourceNode,
        GatheringResources,
        MovingToStorage,
    }

    private IUnit unit;
    private State state;

    private ResourceNode resourceNode;
    private StorageNode storageNode;

    [SerializeField]
    private int maxPassengerHold = 2;
    [SerializeField]
    private float carbonEmit = 0.2f;
    [SerializeField]
    private float stopDistance = 1f;
    public static int totalCarsSpawned = 0;
    private StationBar stationBar;
    public GameObject StationBar;

    private Dictionary<GameResources.StationType, int> inventoryAmountDictionary;
    //private TextMeshPro inventoryTextMesh; // besides the AI



    private void Awake() {
        unit = gameObject.GetComponent<IUnit>();
        state = State.Idle;

        inventoryAmountDictionary = new Dictionary<GameResources.StationType, int>();
        foreach (GameResources.StationType stationType in System.Enum.GetValues(typeof(GameResources.StationType))) {
            inventoryAmountDictionary[stationType] = 0;
        }

        // inventoryTextMesh = transform.Find("inventoryTextMesh").GetComponent<TextMeshPro>();
        // UpdateInventoryText();

        totalCarsSpawned++;
        IncrementCarbonMeter();

       stationBar = FindObjectOfType<StationBar>();
       if (stationBar == null)
    {
        Debug.LogError("StationBar object not found in the scene!");
    }
    }

    private void IncrementCarbonMeter()
    {
        CarbonMeter carbonMeter = FindObjectOfType<CarbonMeter>();
        if (carbonMeter != null)
        {
            carbonMeter.IncreaseCarbonMeter();
        }
    }

    

    public static void ResetTotalCarsSpawned()
    {
        totalCarsSpawned = 0;
    }

    public static int GetTotalCarsSpawned()
    {
        return totalCarsSpawned;
    }

    private int GetTotalInventoryAmount(){
        int total = 0;
        foreach (GameResources.StationType stationType in System.Enum.GetValues(typeof(GameResources.StationType))) {
            total += inventoryAmountDictionary[stationType];    //Increase total by what is actually carrying
        }
        return total;
    }

    private bool IsInventoryFull(){
        return GetTotalInventoryAmount() >= maxPassengerHold; // Max amount of vehicle capacity
    }

    private void DropInventoryAmountIntoGameResources() {
        foreach (GameResources.StationType stationType in System.Enum.GetValues(typeof(GameResources.StationType))) {
            GameResources.AddStationAmount(stationType, inventoryAmountDictionary[stationType]);
            inventoryAmountDictionary[stationType] = 0;
        }
    }

    private void UpdateInventoryText(){ //Updates the text beside the player
        int inventoryamount = GetTotalInventoryAmount();
        // if (inventoryamount > 0 ) {
        //     inventoryTextMesh.text = "" + inventoryamount;
        // } else {
        //     inventoryTextMesh.text = ""; // if 0 passenger get
        // }
    }

    private void Update() {
        switch (state) {
        case State.Idle:
            resourceNode = PlacementManager.GetResourceNodeType_Static(GameResources.StationType.Blue);    // Finds resources available from the GameHandler
            if (resourceNode != null) { //If there is a station pasengger avalible
                state = State.MovingToResourceNode;
            }
            break;
        case State.MovingToResourceNode:
            if (unit.IsIdle()) {
                unit.MoveTo(resourceNode.GetPosition(), stopDistance, () => {
                    state = State.GatheringResources;
                });
            }
            break;
        case State.GatheringResources:
            if (unit.IsIdle() || !resourceNode.HasPassengers()) {
                //if (goldInventoryAmount >= 3)
                if (IsInventoryFull() || !resourceNode.HasPassengers()) { 
                    storageNode = PlacementManager.GetStorageNodeType_Static(GameResources.StationType.Blue);
                    if (storageNode != null){
                        state = State.MovingToStorage;
                    }
                    
                } else {
                    // Gather resources
                    switch (resourceNode.GetStationType()) {
                    case GameResources.StationType.Red:
                        GrabResourceFromNode();
                        break;
                    case GameResources.StationType.Blue:
                        GrabResourceFromNode();
                        break;
                    case GameResources.StationType.Yellow:
                        GrabResourceFromNode();
                        break;
                    }
                }
            }
            break;
        case State.MovingToStorage:
            if (unit.IsIdle()) {

                unit.MoveTo(storageNode.GetAPosition(), stopDistance, () => {
    
                    DropInventoryAmountIntoGameResources();

                    Debug.Log("Blue Amount: " + GameResources.GetStationAmount(GameResources.StationType.Blue));

                    UpdateInventoryText(); // passenger count na nakuha, pabalik na
                    state = State.Idle;
                });
            }
            break;
        }
    }

    

    private void GrabResourceFromNode() 
    {
         GameResources.StationType stationType = resourceNode.GrabResource(stationBar, maxPassengerHold);
    inventoryAmountDictionary[stationType]++;
    UpdateInventoryText(); // Text beside the character
    Debug.Log("LUMBAS KA");
    }
}



