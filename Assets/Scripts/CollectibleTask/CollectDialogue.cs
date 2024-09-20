using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CollectDialogue : MonoBehaviour
{
    public GameObject dialoguePanel;
    public TMP_Text dialogueText;
    public string[] dialogue;
    private int index;

    public float wordSpeed;
    public GameObject continueButton;
    public GameObject FindCollectiblesButton; 
    public Timer timerScript; 

    private bool isTyping = false;

    void Start()
    {
        dialoguePanel.SetActive(false); // Start with the panel inactive
        dialogueText.text = "";
        continueButton.SetActive(false);
        FindCollectiblesButton.SetActive(false); // Start with Start button inactive
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            dialoguePanel.SetActive(true); // Activate the dialogue panel
            StartCoroutine(Typing());
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
        if (timerScript != null)
        {
            timerScript.StartTimer(); 
        }
    }
}
