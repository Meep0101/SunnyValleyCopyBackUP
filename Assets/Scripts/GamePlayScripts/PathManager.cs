using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathManager : MonoBehaviour
{
    public static PathManager main;

    private Transform startPoint;
    private Transform[] VehiclePath;

    private void Awake(){
        main = this;

    }

}
