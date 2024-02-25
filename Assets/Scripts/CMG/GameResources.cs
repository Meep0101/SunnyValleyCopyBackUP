using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameResources{

    public static event EventHandler OnStationAmountChanged;

    public enum StationType {
        Gold,
        Wood,
    }

    //private static int goldAmount; removed since may dictionary na

    // To make the code adaptable for the resource amount using a DICTIONARY<key, value>:
    private static Dictionary<StationType, int> stationAmountDictionary;

    //Initialization of dictionary
    public static void Init() {
        stationAmountDictionary = new Dictionary<StationType, int>();
        
        foreach (StationType stationType in System.Enum.GetValues(typeof(StationType))) {
            stationAmountDictionary[stationType] = 0;
        }
    }

    public static void AddStationAmount (StationType stationType, int amount){
        stationAmountDictionary[stationType] += amount;
        if (OnStationAmountChanged != null) OnStationAmountChanged(null, EventArgs.Empty);
    }

    public static int GetStationAmount(StationType stationType) {
        return stationAmountDictionary[stationType];
    }

}
