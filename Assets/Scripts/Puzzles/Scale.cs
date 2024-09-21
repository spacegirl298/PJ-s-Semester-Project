using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class Scale : MonoBehaviour
{
    public int requiredWeight = 4; 
    public TMP_Text weightDisplay; 
    public GameObject drawer; 
    public GameObject weightPanel; // Reference to the panel or UI element that contains the weight display

    private float currentWeight = 0f; 
    private List<GameObject> blocksOnScale = new List<GameObject>(); // List of blocks currently on the scale
    private bool isPlayerOnScale = false; // To track if the player is within the trigger

    private void Start()
    {
        weightPanel.SetActive(false); // Make sure the weight display panel is off at the start
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Assuming "Player" tag is assigned to the player
        {
            isPlayerOnScale = true;
            weightPanel.SetActive(true); // Show the panel when the player steps on the scale
            UpdateWeightDisplay();
        }

        if (other.CompareTag("PickUp") && !blocksOnScale.Contains(other.gameObject))
        {
            Weights block = other.GetComponent<Weights>();
            if (block != null)
            {
                blocksOnScale.Add(other.gameObject);
                currentWeight += block.weight;
                UpdateWeightDisplay();
                CheckWeight();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) 
        {
            isPlayerOnScale = false;

            if (currentWeight != requiredWeight)
            {
                weightPanel.SetActive(false); // Hide the panel when the player leaves and weight is not correct
            }
        }

        if (other.CompareTag("PickUp") && blocksOnScale.Contains(other.gameObject))
        {
            Weights block = other.GetComponent<Weights>();
            if (block != null)
            {
                currentWeight -= block.weight;
                blocksOnScale.Remove(other.gameObject);
                UpdateWeightDisplay();
            }
        }
    }

    private void UpdateWeightDisplay()
    {
        if (isPlayerOnScale) // Only update display if the player is on the scale
        {
            weightDisplay.text = "Requied Weight = 4kg" + "\n" +  "Current Weight: " + currentWeight.ToString("F2") + "kg"; // Format with 2 decimal places
        }
    }

    private void CheckWeight()
    {
        if (currentWeight == requiredWeight) 
        {
            OpenDrawer();
            weightPanel.SetActive(false); // Hide the panel when the correct weight is reached
        }
    }

    private void OpenDrawer()
    {
        drawer.SetActive(false); // This would open the drawer using an animation or transform
        Debug.Log("Drawer opened!");
    }
}
