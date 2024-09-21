using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    private float timer = 65f; 
    public TMP_Text timerText; 
    private bool timerActive = false; 
    private bool gameOver = false; // New flag to stop gameplay

    public GameObject FailPanel;
    public GameObject WinPanel; 
    public GameObject[] collectibles; 
    private int totalCollectibles; 
    private int collectedCount = 0; 
    public CheckpointManagers checkpointManager; 
    public GameObject player; 

    void Start()
    {
        UpdateTimerText();
        timerText.gameObject.SetActive(false); 
        FailPanel.SetActive(false); 
        WinPanel.SetActive(false); 
        totalCollectibles = collectibles.Length; 
    }

    void Update()
    {
        if (!gameOver) // Prevent gameplay after win/loss
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
                    timer = 0;
                    timerActive = false;
                    UpdateTimerText();

                    if (collectedCount < totalCollectibles)
                    {
                        MissionFailed();
                        Debug.Log("Too slow PJ. Now you're dead!!");
                    }
                }
            }

            // Check if all collectibles are collected
            if (collectedCount >= totalCollectibles)
            {
                MissionCompleted(); // Call MissionCompleted directly once all collectibles are collected
                Debug.Log("Yay PJ, you won!!!");
            }
        }
    }

    public void StartTimer()
    {
        if (!gameOver) // Ensure game hasn't ended before starting timer
        {
            timerActive = true;
            timerText.gameObject.SetActive(true); 
        }
    }

    public void CollectItem()
    {
        if (!gameOver) // Prevent collecting items after game ends
        {
            collectedCount++;
            Debug.Log("Collected item! Total: " + collectedCount);

            // Check if all items are collected
            if (collectedCount >= totalCollectibles)
            {
                MissionCompleted(); // Call MissionCompleted here
            }
        }
    }

    private void MissionFailed()
    {
        gameOver = true; // Stop the game
        FailPanel.SetActive(true);

        // Respawn player and reset timer after delay
        StartCoroutine(RespawnPlayerAndReset());
    }

    private IEnumerator RespawnPlayerAndReset()
    {
        yield return new WaitForSeconds(3f); 

        // Respawn player at the last checkpoint
        checkpointManager.StartRespawn();

        // Reset the game state
        gameOver = false; 
        timer = 65f;
        collectedCount = 0; 
        FailPanel.SetActive(false); 
        timerActive = true; 
        UpdateTimerText();
    }

    // Method to handle winning the game
    private void MissionCompleted()
    {
        gameOver = true; // Stop the game
        timerActive = false; // Stop the timer
        WinPanel.SetActive(true); // Show the win panel
        Debug.Log("Mission Completed! You win.");

        // Optionally disable player movement or other controls here
        DisablePlayerControls();
    }

    private void DisablePlayerControls()
    {
        // Example of disabling player movement by disabling relevant components
        if (player != null)
        {
            // Disable the player's movement component (replace PlayerMovement with your movement script)
            FirstPersonControls movement = player.GetComponent<FirstPersonControls>();
            if (movement != null)
            {
                movement.enabled = false; 
            }
        }
    }

    public void UpdateTimerText()
    {
        int minutes = Mathf.FloorToInt(timer / 60);
        int seconds = Mathf.FloorToInt(timer % 60);

        timerText.text = string.Format("Time Left to find Objects: {0:00}:{1:00}", minutes, seconds);
    }
}
