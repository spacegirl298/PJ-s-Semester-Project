using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class Scale : MonoBehaviour
{
    public int requiredWeight = 4; 
    public TMP_Text weightDisplay; 
    public GameObject drawer; 
    public GameObject weightPanel; 

    private float currentWeight = 0f; 
    private List<GameObject> blocksOnScale = new List<GameObject>(); // List of weights currently on the scale

    private void Start()
    {
        weightPanel.SetActive(false); // Make sure the weight display panel is off at the start
        UpdateWeightDisplay();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PickUp") && !blocksOnScale.Contains(other.gameObject))
        {
            Weights block = other.GetComponent<Weights>();
            if (block != null)
            {
                blocksOnScale.Add(other.gameObject);
                currentWeight += block.weight;
                weightPanel.SetActive(true); // Show the panel when an object is placed on the scale
                UpdateWeightDisplay();
                CheckWeight();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("PickUp") && blocksOnScale.Contains(other.gameObject))
        {
            Weights block = other.GetComponent<Weights>();
            if (block != null)
            {
                currentWeight -= block.weight;
                blocksOnScale.Remove(other.gameObject);
                UpdateWeightDisplay();

                if (blocksOnScale.Count == 0)
                {
                    weightPanel.SetActive(false); // Hide the panel if there are no blocks on the scale
                }
            }
        }
    }

    private void UpdateWeightDisplay()
    {
        weightDisplay.text = "Weight: " + currentWeight.ToString("F2") + "kg";
    }

    private void CheckWeight()
    {
        if (currentWeight == requiredWeight) 
        {
            OpenDrawer();
        }
    }

    private void OpenDrawer()
    {
        drawer.SetActive(false); // This would open the drawer using an animation or transform jaiden will help
        Debug.Log("Drawer opened!");
    }
}
