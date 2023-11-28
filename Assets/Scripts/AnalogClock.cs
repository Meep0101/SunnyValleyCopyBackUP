using System;
using UnityEngine;
using UnityEngine.UI;

public class AnalogClock : MonoBehaviour
{
    // Public variables to reference clock hands and day of the week text in the Unity Editor
    
    public RectTransform minuteHand;
    public Text dayOfWeekText;

    // Variables to store current time, time scale, days of the week, and current day index
    private DateTime currentTime;
    private float secondsPerGameMinute = 0.1f; // Adjust this based on your game's time scale
    private string[] daysOfWeek = {"Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday"};
    private int currentDayIndex = 0;

    // Start is called before the first frame update
    private void Start()
    {

        // Update initial clock hands and day of the week text
        UpdateClockHands();
        UpdateDayOfWeekText();

        // Invoke the UpdateGameTime method every specified number of seconds
        InvokeRepeating("UpdateGameTime", 1f, secondsPerGameMinute);
    }

    // UpdateGameTime is called at a regular interval to simulate the passage of time in the game
    private void UpdateGameTime()
    {
        // Increment the current time by one minute
        currentTime = currentTime.AddMinutes(1);

        // Update clock hands based on the new time
        UpdateClockHands();

        // Check if the minute hand completed a full circle (hour changed)
        if (currentTime.Minute == 0)
        {
            // Update the day of the week and reset the index if it reaches the end of the week
            currentDayIndex = (currentDayIndex + 1) % daysOfWeek.Length;
            UpdateDayOfWeekText();
        }
    }

    // UpdateClockHands rotates the clock hands based on the current time
    private void UpdateClockHands()
    {
      
        float minutes = currentTime.Minute;

        // Rotate the clock hands
        
       minuteHand.rotation = Quaternion.Euler(0f, 0f, 90f - minutes * 6f); // 6 degrees per minute
    }

    // UpdateDayOfWeekText updates the displayed day of the week text
    private void UpdateDayOfWeekText()
    {
        // Set the text to the current day of the week based on the index
        dayOfWeekText.text = daysOfWeek[currentDayIndex];
        Debug.Log("The day today is " + daysOfWeek[currentDayIndex]);
    }
}
