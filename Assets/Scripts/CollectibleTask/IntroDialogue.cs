using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem; 

public class IntroDialogue : MonoBehaviour
{
    public GameObject dialoguePanel;
    public TMP_Text dialogueText;
    public string[] dialogue;
    private int index;
    
    public GameObject[] collectibles;
    public AudioSource collectAppearSound; 

    public float wordSpeed;
  //  public GameObject continueButton;
   // public GameObject FindCollectiblesButton;
   [Header("UI PANELS")]
   [Space(5)]
    public GameObject winPanel;  // win panel
    public GameObject pressEnterPanel;  
    public GameObject startCollectingPanel;  
    private bool collectiblesTaskStarted = false;  

    // public Timer timerScript; -REMOVED

    private bool isTyping = false;
    private bool canContinueDialogue = false;  
    private bool canStartCollecting = false; 
    private Controls controls;
    private FirstPersonControls firstPersonControls; 
    private FirstPersonController firstPersonController;
    

    
    private void Awake()
    {
        controls = new Controls();
    }
    
    private void OnEnable()
    {
        controls.Player.ContinueDialogue.performed += OnContinueDialogue;
        controls.Player.StartCollecting.performed += OnStartCollecting;
        controls.Player.Enable(); 
    }

    private void OnDisable()
    {
        controls.Player.ContinueDialogue.performed -= OnContinueDialogue;
        controls.Player.StartCollecting.performed -= OnStartCollecting;
        controls.Player.Disable(); 
    }

    
    void Start()
    {
        dialoguePanel.SetActive(false); // dialogue panel doesnt show @ first
        dialogueText.text = "";
        pressEnterPanel.SetActive(false); 
        startCollectingPanel.SetActive(false); 
        winPanel.SetActive(false);  // JUST ADDED
        
        //continueButton.SetActive(false);
        //FindCollectiblesButton.SetActive(false); // Start with Start button inactive
        
     
        foreach (GameObject collectible in collectibles) //collectibles dont show in the game till player goes to box //JUST ADDED
        {
            collectible.SetActive(false);
        }
      
    }

   /* private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            dialoguePanel.SetActive(true); 
            StartCoroutine(Typing());
        }
    } */
   
   
   
   
   
   private void OnTriggerEnter(Collider other)
   {
       if (other.CompareTag("Player"))
       {
           dialoguePanel.SetActive(true);

            
            if (!collectiblesTaskStarted)
            { 
                if (firstPersonControls != null)
                {
                    firstPersonControls = GetComponent<FirstPersonControls>();
                    firstPersonControls.enabled = false;
                }

                

            
               StartCoroutine(Typing());


            }
           else
           {
               CheckForWinCondition();  
           }


       }
   }
   
   private void OnTriggerExit(Collider other)
   {
       if (other.CompareTag("Player"))
       {
           HideDialoguePanel(); // Hide the dialogue panel when the player exits the trigger
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
            pressEnterPanel.SetActive(true); 
            canContinueDialogue = true; // player must press enter
            
            //continueButton.SetActive(true); // show Continue button if more dialogue remains
        }
        else
        {
            pressEnterPanel.SetActive(false);
            startCollectingPanel.SetActive(true); 
            canStartCollecting = true;

            

            if (firstPersonControls != null)
            {
                firstPersonControls.enabled = true; //enable movement when inventoy is open
            }

            //continueButton.SetActive(false); // remove/hide Continue button after the last line
            //FindCollectiblesButton.SetActive(true); // show Start button
        }


    }

    public void NextLine()
    {
       // continueButton.SetActive(false); // hide Continue button after it's clicked

        if (index < dialogue.Length - 1)
        {
            index++;
            StartCoroutine(Typing());

            
        }
    }
    
    public void OnContinueDialogue(InputAction.CallbackContext context)
    {
        if (context.performed && canContinueDialogue)
        {
            pressEnterPanel.SetActive(false);
            canContinueDialogue = false;
            NextLine();
        }
    }
    
    public void OnStartCollecting(InputAction.CallbackContext context)
    {
        if (context.performed && canStartCollecting)
        {
            startCollectingPanel.SetActive(false);
            dialoguePanel.SetActive(false);
            canStartCollecting = false;
            collectiblesTaskStarted = true; 
            
            
            foreach (GameObject collectible in collectibles) //collects will now show
            {
                collectible.SetActive(true);
                collectAppearSound.Play();
            }
        }
    }
    
    
    
    public void FindObjects()
    {
        dialoguePanel.SetActive(false); 
       // FindCollectiblesButton.SetActive(false);
        collectiblesTaskStarted = true;  
    }

    private void HideDialoguePanel()
    {
        dialoguePanel.SetActive(false);
        pressEnterPanel.SetActive(false);
    }
    
    private void CheckForWinCondition()
    {
        InventoryManager inventoryManager = FindObjectOfType<InventoryManager>();
        if (inventoryManager != null && inventoryManager.AreAllCollectiblesCollected())
        {
            winPanel.SetActive(true);  // you won
            HideDialoguePanel();
            dialoguePanel.SetActive(false);  
            
            if (firstPersonControls == null)
            {
                firstPersonControls = GetComponent<FirstPersonControls>();
            }

            if (firstPersonControls != null)
            {
                firstPersonControls.enabled = false; //disable movement when win panel is on
            }
            
            
        }
        else
        {
           
            dialogueText.text = "youre not done.. collectible still missing budd";
            if (firstPersonControls != null)
            {
                firstPersonControls.enabled = true; 
            }
           
            
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
