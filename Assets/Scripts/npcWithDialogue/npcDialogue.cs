using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class NpcDialogue : MonoBehaviour
{
    [Header("Dialogue Settings")]
    public GameObject dialoguePanel;
    public TMP_Text dialogueText;
    public string[] dialogueLocation1;
    public string[] dialogueLocation2;
    public string[] dialogueLocation3;
    public string[] dialogueLocation4;
    private string[] currentDialogue;

    [Header("UI Panels")]
    public GameObject winPanel;
    public GameObject pressEnterPanel;
    public GameObject startCollectingPanel;

    [Header("Collectibles")]
    public GameObject[] collectibles;
    public AudioSource collectAppearSound;

    public float wordSpeed = 0.05f;

    public Collider finalCheckCollider;

    private int dialogueIndex;
    private bool isTyping;
    private bool canContinueDialogue;
    private bool canStartCollecting;
    private bool isLastLine;
    private bool collectiblesTaskStarted;

    private Controls controls;
    private FirstPersonControls firstPersonControls;

    public enum Location { Location1, Location2, Location3, Location4 }
    public Location currentLocation;

    private Npc_AI npcAI;

    private void Awake()
    {
        controls = new Controls();
        InitializeFirstPersonControls();
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
        ResetUI();
        DeactivateCollectibles();
        SetDialogueForLocation();
        if (finalCheckCollider != null)
        {
            finalCheckCollider.enabled = false;
        }
    }

    private void InitializeFirstPersonControls()
    {
        if (firstPersonControls == null)
        {
            firstPersonControls = FindObjectOfType<FirstPersonControls>();
        }
    }

    private void ResetUI()
    {
        SetPanelState(dialoguePanel, false);
        SetPanelState(pressEnterPanel, false);
        SetPanelState(startCollectingPanel, false);
        SetPanelState(winPanel, false);
    }

    private void DeactivateCollectibles()
    {
        foreach (GameObject collectible in collectibles)
        {
            collectible.SetActive(false);
        }
    }

    private void SetPanelState(GameObject panel, bool isActive)
    {
        if (panel != null)
        {
            panel.SetActive(isActive);
        }
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

            DisablePlayerMovement();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            HideDialoguePanel();
            EnablePlayerMovement();
        }
    }

    private IEnumerator Typing()
    {
        isTyping = true;
        isLastLine = (dialogueIndex == currentDialogue.Length - 1);

        dialogueText.text = "";
        foreach (char letter in currentDialogue[dialogueIndex].ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(wordSpeed);
        }

        isTyping = false;
        SetPressEnterPanelState(true);
    }

    private void NextLine()
    {
        if (dialogueIndex < currentDialogue.Length - 1)
        {
            dialogueIndex++;
            StartCoroutine(Typing());
        }
        else
        {
            isLastLine = true;
            SetPressEnterPanelState(true);
        }
    }

    private void SetPressEnterPanelState(bool isActive)
    {
        SetPanelState(pressEnterPanel, isActive);
        canContinueDialogue = isActive;
    }

    public void OnContinueDialogue(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        
        if (canContinueDialogue)
        {
            SetPressEnterPanelState(false);

            if (!isLastLine) 
            {
                NextLine();
            }
            else 
            {
                EndDialogueActions();
            }
        }
        else if (canStartCollecting) 
        {
            SetPanelState(startCollectingPanel, false);
            canStartCollecting = false;
            StartCollectibles();
        }
    }

    public void TriggerDialogue()
    {
        if (!dialoguePanel.activeInHierarchy)
        {
            SetPanelState(dialoguePanel, true);
            dialogueIndex = 0;
            StartCoroutine(Typing());
            DisablePlayerMovement();
        }
    }

    private void HideDialoguePanel()
    {
        SetPanelState(dialoguePanel, false);
        SetPressEnterPanelState(false);
    }

    private void StartCollectibles()
    {
        collectiblesTaskStarted = true;
        foreach (GameObject collectible in collectibles)
        {
            collectible.SetActive(true);
        }

        if (collectAppearSound != null)
        {
            collectAppearSound.Play();
        }

        if (finalCheckCollider != null)
        {
            finalCheckCollider.enabled = true;
        }

        EnablePlayerMovement();
    }

    private void CheckForWinCondition()
    {
        InventoryManager inventoryManager = FindObjectOfType<InventoryManager>();
        if (inventoryManager != null && inventoryManager.AreAllCollectiblesCollected())
        {
            SetPanelState(winPanel, true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            HideDialoguePanel();
            DisablePlayerMovement();
        }
        else
        {
            TriggerDialogueWithText("You're not done yet... some collectibles are still missing. \nCheck your Inventory to see which ones.");
            EnablePlayerMovement();
        }
    }

    private void TriggerDialogueWithText(string text)
    {
        SetPanelState(dialoguePanel, true);
        dialogueText.text = text;
        EnablePlayerMovement();
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
        dialogueIndex = 0;
    }

    public void UpdateLocation(Location newLocation)
    {
        if (currentLocation != newLocation)
        {
            currentLocation = newLocation;
            SetDialogueForLocation();
        }
    }

    private void EndDialogueActions()
    {
        if (currentLocation == Location.Location1)
        {
            // Prepare the collectible-start panel
            SetPanelState(startCollectingPanel, true);
            canStartCollecting = true; 
        }

        if (currentLocation == Location.Location3 && finalCheckCollider != null)
        {
            finalCheckCollider.enabled = true;
        }

       
        EndDialogue();

        
        if (npcAI != null && currentLocation != Location.Location4)
        {
            npcAI.OnInteractionComplete();
        }
    }

    private void EndDialogue()
    {
        HideDialoguePanel();
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
    {
        if (firstPersonControls != null)
        {
            firstPersonControls.enabled = true;
            Debug.Log("Player movement enabled.");
        }
    }
}
