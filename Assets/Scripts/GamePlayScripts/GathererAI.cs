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

    //private Transform resourceNodeTransform;
    private ResourceNode resourceNode;

    private Transform storageTransform;

    //private int goldInventoryAmount;
    //Dictionary for resourcetype/stationtype
    private Dictionary<GameResources.StationType, int> inventoryAmountDictionary;
    private TextMeshProUGUI inventoryTextMesh; // besides the AI

    private void Awake() {
        unit = gameObject.GetComponent<IUnit>();
        state = State.Idle;

        inventoryAmountDictionary = new Dictionary<GameResources.StationType, int>();
        foreach (GameResources.StationType stationType in System.Enum.GetValues(typeof(GameResources.StationType))) {
            inventoryAmountDictionary[stationType] = 0;
        }

        inventoryTextMesh = transform.Find("inventoryTextMesh").GetComponent<TextMeshProUGUI>();
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
        return GetTotalInventoryAmount() >= 3;
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
            resourceNode = GameManager.GetResourceNodeType_Static(GameResources.StationType.Gold);    // Finds resources available from the GameHandler, Removed on player click
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
            if (unit.IsIdle() || !resourceNode.HasResources()) {
                //if (goldInventoryAmount >= 3)
                if (IsInventoryFull() || !resourceNode.HasResources()) { 
                    // Move to storage when have more than 3 units/passenger
                    storageTransform = GameManager.GetStorage_Static();
                    //resourceNode = GameHandler.GetResourceNodeType_Static(resourceNode.GetStationType()); 
                    state = State.MovingToStorage;
                } else {
                    // Gather resources
                    switch (resourceNode.GetStationType()) {
                    case GameResources.StationType.Gold:
                        GrabResourceFromNode();
                        break;
                    case GameResources.StationType.Wood:
                        GrabResourceFromNode();
                        break;
                    }
                    
                    //goldInventoryAmount++;

                    // unit.PlayAnimationMine(resourceNode.GetPosition(), () => {
                    //     resourceNode.GrabResource();     
                    //     goldInventoryAmount++;
                    //     UpdateInventoryText();
                    // });
                }
            }
            break;
        case State.MovingToStorage:
            if (unit.IsIdle()) {
                unit.MoveTo(storageTransform.position, 1f, () => {
                    //GameResources.AddStationAmount(GameResources.StationType.Gold, goldInventoryAmount);
                    DropInventoryAmountIntoGameResources();

                    Debug.Log("Gold Amount: " + GameResources.GetStationAmount(GameResources.StationType.Gold));
                    Debug.Log("Wood Amount: " + GameResources.GetStationAmount(GameResources.StationType.Wood));

                    //goldInventoryAmount = 0;
                    //DropInventoryAmountIntoGameResources();
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