using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class NpcDialogue : MonoBehaviour
{
    public GameObject dialoguePanel;
    public TMP_Text dialogueText;

    [Header("Location-Specific Dialogue")]
    public string[] dialogueLocation1;
    public string[] dialogueLocation2;
    public string[] dialogueLocation3;
    public string[] dialogueLocation4;

    private string[] currentDialogue;
    private int index;

    public GameObject[] collectibles;
    public AudioSource collectAppearSound;

    public float wordSpeed;

    [Header("UI PANELS")]
    public GameObject winPanel;
    public GameObject pressEnterPanel;
    public GameObject startCollectingPanel;

    private bool collectiblesTaskStarted = false;
    private bool isTyping = false;
    private bool canContinueDialogue = false;
    private bool canStartCollecting = false;
    private bool isLastLine = false;

    private Controls controls;
    private FirstPersonControls firstPersonControls;

    public enum Location { Location1, Location2, Location3, Location4 }
    public Location currentLocation;

    private Npc_AI npcAI;
    public Collider finalCheckCollider;

    private void Awake()
    {
        controls = new Controls();
        npcAI = GetComponent<Npc_AI>();
        
        if (firstPersonControls == null)
        {
            firstPersonControls = FindObjectOfType<FirstPersonControls>(); // Look for FirstPersonControls on any object in the scene
        }
    }

    private void OnEnable()
    {
        controls.Player.ContinueDialogue.performed += OnContinueDialogue;
        controls.Player.Enable();
    }

    private void OnDisable()
    {
        controls.Player.ContinueDialogue.performed -= OnContinueDialogue;
        controls.Player.Disable();
    }

    void Start()
    {
        dialoguePanel.SetActive(false);
        dialogueText.text = "";
        pressEnterPanel.SetActive(false);
        startCollectingPanel.SetActive(false);
        winPanel.SetActive(false);

        foreach (GameObject collectible in collectibles)
        {
            collectible.SetActive(false);
        }

        SetDialogueForLocation();
        finalCheckCollider.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (collectiblesTaskStarted && currentLocation == Location.Location4)
            {
                CheckForWinCondition();
            }
            else
            {
                TriggerDialogue();
            }
        }

        if (firstPersonControls == null)
        {
            firstPersonControls = GetComponent<FirstPersonControls>();
        }

        if (firstPersonControls != null)
        {
            firstPersonControls.enabled = false; //disable movement when inventoy is open
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            HideDialoguePanel();
        }
    }

    private IEnumerator Typing()
    {
        dialogueText.text = "";
        isTyping = true;
        isLastLine = (index == currentDialogue.Length - 1);

        foreach (char letter in currentDialogue[index].ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(wordSpeed);
        }

        isTyping = false;

        if (isLastLine)
        {
            pressEnterPanel.SetActive(true);
            canContinueDialogue = true;
        }
        else
        {
            pressEnterPanel.SetActive(true);
            canContinueDialogue = true;
        }
    }

    private void NextLine()
    {
        if (index < currentDialogue.Length - 1)
        {
            index++;
            StartCoroutine(Typing());
        }
        else
        {
            isLastLine = true;
            EndDialogueActions();
        }
    }

    public void OnContinueDialogue(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (canContinueDialogue)
            {
                pressEnterPanel.SetActive(false);
                canContinueDialogue = false;

                if (isLastLine)
                {
                    EndDialogueActions();
                }
                else
                {
                    NextLine();
                }
            }
            else if (canStartCollecting)
            {
                startCollectingPanel.SetActive(false);
                canStartCollecting = false;
                StartCollectibles();
            }
        }
    }

    public void TriggerDialogue()
    {
        if (!dialoguePanel.activeInHierarchy)
        {
            dialoguePanel.SetActive(true);
            index = 0;
            StartCoroutine(Typing());
            //TogglePlayerControls(false);
            DisablePlayerMovement();
        }
    }

    private void HideDialoguePanel()
    {
        dialoguePanel.SetActive(false);
        pressEnterPanel.SetActive(false);
       // TogglePlayerControls(true);
       DisablePlayerMovement();
    }

    private void StartCollectibles()
    {
        collectiblesTaskStarted = true;

        foreach (GameObject collectible in collectibles)
        {
            collectible.SetActive(true);
        }

        collectAppearSound.Play();
        finalCheckCollider.enabled = true;
        TogglePlayerControls(true);
    }

    private void CheckForWinCondition()
    {
        InventoryManager inventoryManager = FindObjectOfType<InventoryManager>();
        if (inventoryManager != null && inventoryManager.AreAllCollectiblesCollected())
        {
            winPanel.SetActive(true);
            HideDialoguePanel();
            TogglePlayerControls(false);
            DisablePlayerMovement();
        }
        else
        {
            dialoguePanel.SetActive(true);
            dialogueText.text = "You're not done yet... some collectibles are still missing. \n Check your Inventory to see which ones.";
        }
    }

    public void SetDialogueForLocation()
    {
        switch (currentLocation)
        {
            case Location.Location1:
                currentDialogue = dialogueLocation1;
                break;
            case Location.Location2:
                currentDialogue = dialogueLocation2;
                break;
            case Location.Location3:
                currentDialogue = dialogueLocation3;
                break;
            case Location.Location4:
                currentDialogue = dialogueLocation4;
                break;
        }
        index = 0;
    }

    public void UpdateLocation(Location newLocation)
    {
        if (currentLocation != newLocation)
        {
            currentLocation = newLocation;
            SetDialogueForLocation();
        }
    }

    private void TogglePlayerControls(bool isEnabled)
    {
        if (firstPersonControls == null)
        {
            firstPersonControls = GetComponent<FirstPersonControls>();
        }
        if (firstPersonControls != null)
        {
            firstPersonControls.enabled = isEnabled;
        }
    }

    private void EndDialogueActions()
    {
        if (currentLocation == Location.Location1)
        {
            startCollectingPanel.SetActive(true);
            canStartCollecting = true;
        }
        else if (currentLocation == Location.Location3)
        {
            finalCheckCollider.enabled = true;
        }

        EndDialogue();

        if (currentLocation != Location.Location4)
        {
            npcAI.OnInteractionComplete(); //  NPC to the next location only after player presses enter
        }
    }

    private void EndDialogue()
    {
        dialoguePanel.SetActive(false);
        pressEnterPanel.SetActive(false);
       // TogglePlayerControls(true);
        //EnablePlayerMovement();
        if (!winPanel.activeInHierarchy)
        {
            EnablePlayerMovement();
        }
    }
    
    private void DisablePlayerMovement()
    {
        if (firstPersonControls != null)
        {
            firstPersonControls.enabled = false;
            Debug.Log("Player movement disabled.");
        }
    }

    private void EnablePlayerMovement()
    { if (firstPersonControls != null)
        {
            firstPersonControls.enabled = true;
            Debug.Log("Player movement enabled.");
        }
    }
}
