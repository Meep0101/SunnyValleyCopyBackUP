using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class GathererAI : MonoBehaviour {

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
    private int maxPassengerHold = 3;
    [SerializeField]
    private float carbonEmit = 0.2f;


    private Dictionary<GameResources.StationType, int> inventoryAmountDictionary;
    private TextMeshPro inventoryTextMesh; // besides the AI

    private void Awake() {
        unit = gameObject.GetComponent<IUnit>();
        state = State.Idle;

        inventoryAmountDictionary = new Dictionary<GameResources.StationType, int>();
        foreach (GameResources.StationType stationType in System.Enum.GetValues(typeof(GameResources.StationType))) {
            inventoryAmountDictionary[stationType] = 0;
        }

        inventoryTextMesh = transform.Find("inventoryTextMesh").GetComponent<TextMeshPro>();
        UpdateInventoryText();
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
        if (inventoryamount > 0 ) {
            inventoryTextMesh.text = "" + inventoryamount;
        } else {
            inventoryTextMesh.text = ""; // if 0 passenger get
        }

        // if (goldInventoryAmount > 0 ) {
        //     inventoryTextMesh.text = "" + goldInventoryAmount;
        // } else {
        //     inventoryTextMesh.text = ""; // if 0 passenger get
        // }
    }

    private void Update() {
        switch (state) {
        case State.Idle:
            resourceNode = GameManager.GetResourceNodeType_Static(GameResources.StationType.Red);    // Finds resources available from the GameHandler, Removed on player click
            if (resourceNode != null) { //If there is a station pasengger avalible
                state = State.MovingToResourceNode;
            }
            break;
        case State.MovingToResourceNode:
            if (unit.IsIdle()) {
                //resourceNode = GameHandler.GetResourceNodeType_Static(resourceNode.GetStationType());
                unit.MoveTo(resourceNode.GetPosition(), 1f, () => {
                    state = State.GatheringResources;
                });
            }
            break;
        case State.GatheringResources:
            if (unit.IsIdle() || !resourceNode.HasPassengers()) {
                //if (goldInventoryAmount >= 3)
                if (IsInventoryFull() || !resourceNode.HasPassengers()) { 
                    // Move to storage when have more than 3 units/passenger
                    GameResources.StationType storageType = resourceNode.GetStationType();
                    storageNode = GameManager.GetStorageNodeType_Static(storageType);
                    
                    state = State.MovingToStorage;
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

                unit.MoveTo(storageNode.GetAPosition(), 1f, () => {
    
                    DropInventoryAmountIntoGameResources();

                    Debug.Log("RedAmount: " + GameResources.GetStationAmount(GameResources.StationType.Red));
                    Debug.Log("Blue Amount: " + GameResources.GetStationAmount(GameResources.StationType.Blue));
                    Debug.Log("Yellow Amount: " + GameResources.GetStationAmount(GameResources.StationType.Yellow));

                    UpdateInventoryText(); // passenger count na nakuha, pabalik na
                    state = State.Idle;
                });
            }
            break;
        }
    }

    private void GrabResourceFromNode() {
        GameResources.StationType stationType = resourceNode.GrabResource();
        inventoryAmountDictionary[stationType]++;
        UpdateInventoryText(); // Text beside the character
        // resourceNode.GrabResource();
        // inventoryAmountDictionary[GameResources.StationType.Gold]++;
    }

}