using System;
using UnityEngine;
using UnityEngine.UI;

public class AnalogClock : MonoBehaviour
{
   
    
    public RectTransform minuteHand;
    public Text dayOfWeekText;

    
    private DateTime currentTime;
    private float secondsPerGameMinute = 0.2f; // 0.03f speed for fast forward
    private string[] daysOfWeek = {"Mon", "Mon","Tue", "Wed", "Thu",  "Fri", "Sat", "Sun"};  
    
    private int currentDayIndex = 0;
    private int daysPassed = 0;

    public CarbonMeter carbonMeter;

    private bool isGamePaused = false;

   
    private void Start()
    {   
        

        // Update initial clock hands and day of the week text
        UpdateClockHands();
        UpdateDayOfWeekText();

        // Invoke the UpdateGameTime method every specified number of seconds
        InvokeRepeating("UpdateGameTime", 1f, secondsPerGameMinute);
    }

    private void UpdateGameTime()
    {
       if(isGamePaused || carbonMeter.GetCarbonMeterValue() >= 100)
       {
        StopClock();
        return;
       }
        currentTime = currentTime.AddMinutes(1);

        
        UpdateClockHands();

        // Check if the minute hand completed a full circle (hour changed)
        if (currentTime.Minute == 0)
        {
            // Update the day of the week and reset the index if it reaches the end of the week
            currentDayIndex = (currentDayIndex + 1) % daysOfWeek.Length;
            daysPassed++;
            UpdateDayOfWeekText();

            FindObjectOfType<GameManager>().IncrementDays();
        }


    }

    private void StopClock()
    {
        Debug.Log("Clock Stop");
        enabled = false;
       
    }

    // UpdateClockHands rotates the clock hands based on the current time
    private void UpdateClockHands()
    {
      
        float minutes = currentTime.Minute;

        // Rotate the clock hands
        
       minuteHand.rotation = Quaternion.Euler(0f, 0f, 0f - minutes * 6f); // 6 degrees per minute
    }

    // UpdateDayOfWeekText updates the displayed day of the week text
    private void UpdateDayOfWeekText()
    {
        // Set the text to the current day of the week based on the index
        dayOfWeekText.text = daysOfWeek[currentDayIndex];
        //Debug.Log("The day today is " + daysOfWeek[currentDayIndex]);
    }
}