using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    private float timer = 65f; 
    public TMP_Text timerText; 
    private bool timerActive = false; 

    public GameObject FailPanel; 
    public GameObject[] collectibles; // array of collectible 
    private int totalCollectibles; // total num of collectibles
    private int collectedCount = 0; // how many collectibles player has gotten
    public CheckpointManagers checkpointManager; 
    public GameObject player; 

    void Start()
    {
        
        UpdateTimerText();
        timerText.gameObject.SetActive(false); 
        FailPanel.SetActive(false); 
        totalCollectibles = collectibles.Length; // Count total collectibles
    }

    void Update()
    {
        if (timerActive)
        {
            if (timer > 0)
            {
                timer -= Time.deltaTime; // so timer counts down
                UpdateTimerText();
            }
            else
            {
                // Timer has run out, pplayer loset if not all collectibles are collected
                timer = 0;
                timerActive = false;
                UpdateTimerText();

                if (collectedCount < totalCollectibles)
                {
                    
                    MissionFailed();
                    Debug.Log("too slow PJ. Now youre dead!!");
                }
            }
        }

        // check if all collectibles are collected
        if (collectedCount >= totalCollectibles)
        {
            Debug.Log("yay PJ you won!!!");
            timerActive = false; // Stop the timer
           
        }
    }

    public void StartTimer()
    {
        // Start the timer when the player enters the level
        timerActive = true;
        timerText.gameObject.SetActive(true); 
    }

    public void CollectItem()
    {
        // Call this when the player collects an item
        collectedCount++;
        Debug.Log("Collected item! Total: " + collectedCount);

        // Check if all items are collected immediately after picking up
        if (collectedCount >= totalCollectibles && timerActive)
        {
            Debug.Log("Mission Completed! You win.");
            timerActive = false; // Stop the timer
            // Handle win logic (e.g., proceed to next level)
        }
    }

    private void MissionFailed()
    {
       
        FailPanel.SetActive(true);

        // Respawn player and reset timer after delay
        StartCoroutine(RespawnPlayerAndReset());
    }

    private IEnumerator RespawnPlayerAndReset()
    {
        yield return new WaitForSeconds(3f); // Delay for 3 seconds before respawn

        // Respawn player at the last checkpoint
        checkpointManager.StartRespawn();

        // Reset the timer
        timer = 65f;
        collectedCount = 0; // Reset collectibles count
        FailPanel.SetActive(false); 
        timerActive = true; // start again the timer
        UpdateTimerText();
    }

    public void UpdateTimerText()
    {
        int minutes = Mathf.FloorToInt(timer / 60);
        int seconds = Mathf.FloorToInt(timer % 60);

        timerText.text = string.Format("Time Left to find Objects: {0:00}:{1:00}", minutes, seconds);
    }
}
