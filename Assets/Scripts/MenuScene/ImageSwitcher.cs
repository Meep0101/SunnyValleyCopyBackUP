using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageSwitcher : MonoBehaviour
{
    public Image[] vehicleImages;
    public float switchInterval = 2.5f; // Switch images every 5 seconds

    private int currentIndex = 0; //For in order spawn

    void Start()
    {
        StartCoroutine(SwitchImages());
    }

    IEnumerator SwitchImages()
    {
        while (true)
        {
            // Change the sprite of the Image component (RANDOM SPAWN)
            //GetComponent<Image>().sprite = vehicleImages[Random.Range(0, vehicleImages.Length)].sprite;

            // Change the sprite of the Image component
            GetComponent<Image>().sprite = vehicleImages[currentIndex].sprite;

            // Move to the next sprite in order
            currentIndex = (currentIndex + 1) % vehicleImages.Length;

            // Wait for the specified interval before switching to another image
            yield return new WaitForSeconds(switchInterval);
        }
    }
}
