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
    private Controls controls;
    private FirstPersonControls firstPersonControls;

    private bool isFinalAppearance = false;
    private bool completionTriggerActivated = false; // To track if the final collider should be active

    public enum Location { Location1, Location2, Location3 }
    public Location currentLocation;

    private Npc_AI npcAI;
    public Collider finalCheckCollider; // Secondary collider for checking task completion

    private void Awake()
    {
        controls = new Controls();
        npcAI = GetComponent<Npc_AI>();
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
        finalCheckCollider.enabled = false; // Disable secondary collider at the start
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (collectiblesTaskStarted && completionTriggerActivated)
            {
                // Check for collectibles completion if final collider is active
                CheckForWinCondition();
            }
            else if (!collectiblesTaskStarted)
            {
                TriggerDialogue();
            }
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

        foreach (char letter in currentDialogue[index].ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(wordSpeed);
        }

        isTyping = false;

        if (index < currentDialogue.Length - 1)
        {
            pressEnterPanel.SetActive(true);
            canContinueDialogue = true;
        }
        else
        {
            if (isFinalAppearance)
            {
                startCollectingPanel.SetActive(true);
                canStartCollecting = true;
            }
            else
            {
                pressEnterPanel.SetActive(true); //after final line
                canContinueDialogue = true; //when you press enter u can continue
            }
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
            EndDialogue();
            npcAI.OnInteractionComplete();
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
                NextLine();
            }
            else if (canStartCollecting)
            {
                startCollectingPanel.SetActive(false);
                dialoguePanel.SetActive(false);
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
            TogglePlayerControls(false);
        }
    }

    private void HideDialoguePanel()
    {
        dialoguePanel.SetActive(false);
        pressEnterPanel.SetActive(false);
        TogglePlayerControls(true);
    }

    private void StartCollectibles()
    {
        collectiblesTaskStarted = true;
        completionTriggerActivated = true; // switch secomd trigger checking

        foreach (GameObject collectible in collectibles)
        {
            collectible.SetActive(true);
            collectAppearSound.Play();
        }

        finalCheckCollider.enabled = true; //switch on trigger collider
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
        }
        else
        {
            dialoguePanel.SetActive(true);
            dialogueText.text = "u're not done yet... collectibles stil missing.";
        }
    }

    public void SetDialogueForLocation()
    {
        switch (currentLocation)
        {
            case Location.Location1:
                currentDialogue = dialogueLocation1; //make sho dialogue is for first location
                isFinalAppearance = false;
                break;

            case Location.Location2:
                currentDialogue = dialogueLocation2;
                isFinalAppearance = false;
                break;

            case Location.Location3:
                currentDialogue = dialogueLocation3;
                isFinalAppearance = true;
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

    private void EndDialogue()
    {
        dialoguePanel.SetActive(false);
        pressEnterPanel.SetActive(false);
        TogglePlayerControls(true);
    }
}