
using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    private int goldInventoryAmount;
    // TextMeshPro inventoryTextMesh;

    private void Awake() {
        unit = gameObject.GetComponent<IUnit>();
        state = State.Idle;

       
        //inventoryTextMesh = transform.Find("inventoryTextMesh").GetComponent<TextMeshPro>();
        //UpdateInventoryText();
    }

    // private void UpdateInventoryText(){
    //     if (goldInventoryAmount > 0 ) {
    //         inventoryTextMesh.text = "" + goldInventoryAmount;
    //     } else {
    //         inventoryTextMesh.text = "";
    //     }
    // }

    private void Update() {
        switch (state) {
        case State.Idle:
            resourceNode = GameHandler.GetResourceNode_Static();    // Finds resources available from the GameHandler
            if (resourceNode != null) {
                state = State.MovingToResourceNode;
            }
            break;
        case State.MovingToResourceNode:
            if (unit.IsIdle()) {
                unit.MoveTo(resourceNode.GetPosition(), 1f, () => {
                    state = State.GatheringResources;
                });
            }
            break;
        case State.GatheringResources:
            if (unit.IsIdle() || !resourceNode.HasResources()) {
                if (goldInventoryAmount >= 3) {
                    // Move to storage
                    storageTransform = GameHandler.GetStorage_Static();
                    state = State.MovingToStorage;
                } else {
                    // Gather resources
                    resourceNode.GrabResource();
                    goldInventoryAmount++;
                    //UpdateInventoryText();

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
                    GameResources.AddStationAmount(GameResources.StationType.Gold, goldInventoryAmount);

                    Debug.Log("Gold Amount: " + GameResources.GetStationAmount(GameResources.StationType.Gold));

                    goldInventoryAmount = 0;
                    //UpdateInventoryText();
                    state = State.Idle;
                });
            }
            break;
        }
    }
}
