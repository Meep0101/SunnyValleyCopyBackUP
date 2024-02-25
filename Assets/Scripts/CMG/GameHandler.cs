using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using CodeMonkey;
//using CodeMonkey.Utils;
//using CodeMonkey.MonoBehaviours;

public class GameHandler : MonoBehaviour {

    private static GameHandler instance;
    
    [SerializeField] private Transform goldNode1Transform;
    [SerializeField] private Transform goldNode2Transform;
    [SerializeField] private Transform goldNode3Transform;
    [SerializeField] private Transform storageTransform;

    private List<ResourceNode> resourceNodeList;

    private void Awake() {
        instance = this;

        GameResources.Init();

        resourceNodeList = new List<ResourceNode>();
        resourceNodeList.Add(new ResourceNode(goldNode1Transform));
        resourceNodeList.Add(new ResourceNode(goldNode2Transform));
        resourceNodeList.Add(new ResourceNode(goldNode3Transform));
    }

    private ResourceNode GetResourceNode() {
        //List<Transform> resourceNodeList = new List<Transform>() { goldNode1Transform, goldNode2Transform, goldNode3Transform };
        
        List<ResourceNode> tmpResourceNodeList = new List<ResourceNode>(resourceNodeList);      //Clone List only for the use of cycle
        for (int i = 0; i < tmpResourceNodeList.Count; i++){
            if (!tmpResourceNodeList[i].HasResources()){
                //No more Resources or Passengers
                tmpResourceNodeList.RemoveAt(i);
                i--;
            }
        }
        if (tmpResourceNodeList.Count > 0){
            return tmpResourceNodeList[UnityEngine.Random.Range(0, tmpResourceNodeList.Count)];     //Return that have resources or passengers
        } else {
            return null;
        }
    }

    public static ResourceNode GetResourceNode_Static() {
        return instance.GetResourceNode();
    }

    private Transform GetStorage() {
        return storageTransform;
    }

    public static Transform GetStorage_Static() {
        return instance.GetStorage();
    }
}
