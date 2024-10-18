using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class Scale : MonoBehaviour
{
    public int requiredWeight = 4; 
    public TMP_Text weightDisplay; 
    public GameObject drawer; 
    public GameObject weightPanel;
    public GameObject scaleTrigger;

    public Animator drawerOpen;

    private float currentWeight = 0f; 
    private List<GameObject> blocksOnScale = new List<GameObject>(); 
    private bool isPlayerOnScale = false; 

    private void Start()
    {
        weightPanel.SetActive(false); 
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) 
        {
            isPlayerOnScale = true;
            weightPanel.SetActive(true); 
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
                weightPanel.SetActive(false); 
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
        if (isPlayerOnScale) 
        {
            weightDisplay.text = "Requied Weight = 4kg" + "\n" +  "Current Weight: " + currentWeight.ToString("F2") + "kg"; // Format with 2 decimal places
        }
    }

    private void CheckWeight()
    {
        if (currentWeight == requiredWeight)
        {
            OpenDrawer();
           // weightPanel.SetActive(false); 
            Destroy(scaleTrigger);
        }
    }

    private void OpenDrawer()
    {
        drawerOpen.SetBool("DrawOpen", true);
        //drawer.SetActive(false); 
        weightPanel.SetActive(false); 
        Debug.Log("Drawer opened!");
    }
}
