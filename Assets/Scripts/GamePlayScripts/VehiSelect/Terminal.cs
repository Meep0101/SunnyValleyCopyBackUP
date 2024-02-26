using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Terminal : MonoBehaviour
{
    public Color emptyColor = Color.white;
    public Color currentColor = Color.white;
    public int currentCount; 

    public GameObject VehiclesTab; // Reference to the pop-up tab GameObject

    public void ChangeColor(Color newColor)
    {
        currentColor = newColor;
    }

    private void Start()
    {
        currentCount = 2;
        Debug.Log("Initial current count: " + currentCount);
        //VehiclesTab = GameObject.Find("VehiclesTab");
        // Ensure that the pop-up tab is initially inactive
        VehiclesTab.SetActive(false);
    }

    private void OnMouseDown()
    {
        // Activate the pop-up tab when the player clicks on the terminal
        VehiclesTab.SetActive(true);
    }

    public void DecrementCount()
    {
        currentCount--;
        Debug.Log("Current count: " + currentCount);
    }
}
