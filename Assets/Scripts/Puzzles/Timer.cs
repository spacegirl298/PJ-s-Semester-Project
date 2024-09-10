using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI; 

public class Timer : MonoBehaviour
{
    private float timer = 65f; 
    public TMP_Text timerText; 
    private bool timerActive = false;

    void Start()
    {
        UpdateTimerText();
        timerText.gameObject.SetActive(false); // Initially hide the timer text
    }

    public void Update()
    {
        if (timerActive)
        {
            if (timer > 0)
            {
                timer -= Time.deltaTime;
                UpdateTimerText();
            }
            else
            {
                Debug.Log("Time's up!");
                timer = 0;
                timerActive = false;
                UpdateTimerText();
            }
        }
    }

    public void StartTimer()
    {
        timerActive = true;
        timerText.gameObject.SetActive(true); 
    }

    public void UpdateTimerText()
    {
        int minutes = Mathf.FloorToInt(timer / 60);
        int seconds = Mathf.FloorToInt(timer % 60);

        timerText.text = string.Format("Time Left to find Objects: {0:00}:{1:00}", minutes, seconds);
    }
}