using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class IntroDialogue : MonoBehaviour
{
    public GameObject dialoguePanel;
    public TMP_Text dialogueText;
    public string[] dialogue;
    private int index;

    public float wordSpeed;
    public GameObject continueButton;
    public GameObject FindCollectiblesButton;
    
    public GameObject winPanel;  // win panel
    private bool collectiblesTaskStarted = false;  // to track if the task to find collectibles has started

    // public Timer timerScript; -REMOVED

    private bool isTyping = false;

    void Start()
    {
        dialoguePanel.SetActive(false); // star with the panel inactive
        dialogueText.text = "";
        continueButton.SetActive(false);
        FindCollectiblesButton.SetActive(false); // Start with Start button inactive
        winPanel.SetActive(false);  // JUST ADDED
    }

   /* private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            dialoguePanel.SetActive(true); // Activate the dialogue panel
            StartCoroutine(Typing());
        }
    } */
   
   private void OnTriggerEnter(Collider other)
   {
       if (other.CompareTag("Player"))
       {
           dialoguePanel.SetActive(true); // Activate the dialogue panel
           if (!collectiblesTaskStarted)  // Only play the dialogue the first time
           {
               StartCoroutine(Typing());
           }
           else
           {
               CheckForWinCondition();  // If collectibles task has started, check if all items are collected
           }
       }
   }


    IEnumerator Typing()
    {
        dialogueText.text = "";
        isTyping = true;
        foreach (char letter in dialogue[index].ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(wordSpeed);
        }
        isTyping = false;

        if (index < dialogue.Length - 1)
        {
            continueButton.SetActive(true); // show Continue button if more dialogue remains
        }
        else
        {
            continueButton.SetActive(false); // remove/hide Continue button after the last line
            FindCollectiblesButton.SetActive(true); // show Start button
        }
    }

    public void NextLine()
    {
        continueButton.SetActive(false); // hide Continue button after it's clicked

        if (index < dialogue.Length - 1)
        {
            index++;
            StartCoroutine(Typing());
        }
    }
    
    public void FindObjects()
    {
        dialoguePanel.SetActive(false); 
        FindCollectiblesButton.SetActive(false);
        collectiblesTaskStarted = true;  // Mark that the collectibles task has started
    }

    private void CheckForWinCondition()
    {
        InventoryManager inventoryManager = FindObjectOfType<InventoryManager>();
        if (inventoryManager != null && inventoryManager.AreAllCollectiblesCollected())
        {
            winPanel.SetActive(true);  // Show the win panel if all collectibles are collected
            dialoguePanel.SetActive(false);  // Optionally hide the dialogue panel
        }
        else
        {
            // Optional: Show a message if not all collectibles are collected
            dialogueText.text = "Keep looking for collectibles!";
        }
    }
}



   /* public void FindObjects()
    {
        dialoguePanel.SetActive(false); 
        FindCollectiblesButton.SetActive(false); 
        if (timerScript != null)
        {
            timerScript.StartTimer(); 
        }
    }
} */
