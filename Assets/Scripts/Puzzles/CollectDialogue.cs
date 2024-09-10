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
    // Start is called before the first frame update
    void Start()
    {
        dialoguePanel.SetActive(false); // the dialogue  the panel starts inactive
        dialogueText.text = "";
        
        //BUTTONS are inititally inactive
        continueButton.SetActive(false);
        FindCollectiblesButton.SetActive(false); 
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            dialoguePanel.SetActive(true); // activate the dialogue panel
            StartCoroutine(Typing());
        }
    }

    // Update is called once per frame
    void Update()
    {
        
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
            continueButton.SetActive(true); // show Continue button if there is more dialogue 
        }
        else
        {
            continueButton.SetActive(false); // remove/hide Continue button after the last line
            FindCollectiblesButton.SetActive(true); // show 'Find' button
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
