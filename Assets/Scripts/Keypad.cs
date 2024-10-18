using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems; // Important for button navigation

public class Keypad : MonoBehaviour
{
    private FirstPersonControls firstPersonControls; //player script
    public GameObject player;

    [SerializeField] private Text Ans;
    [SerializeField] private Animator Door;

    private string Answer = "0451";
    private string Input;

    public GameObject CodePanel;
    public GameObject Trigger;
    public GameObject Doors;

    public Button[] keypadButtons; // Assign your keypad buttons here
    private int selectedButtonIndex = 0; // Keeps track of the currently selected button
    private EventSystem eventSystem; // For managing UI focus
    
    public AudioSource unlockDoorSound;
    public AudioClip unlockDoorSound_;

    void Awake()
    {
        firstPersonControls = player.GetComponent<FirstPersonControls>();
        eventSystem = EventSystem.current; // Get the event system instance
    }

    private void Start()
    {
        // Automatically select the first button when the panel is activated
        if (keypadButtons.Length > 0)
        {
            eventSystem.SetSelectedGameObject(keypadButtons[selectedButtonIndex].gameObject);
        }
    }

    public void Number(int number)
    {
        Ans.text += number.ToString();
    }

    public void Execute()
    {
        if (Ans.text == Answer)
        {
            Ans.text = "Correct";
            Door.SetBool("Open", true);
            
            if (unlockDoorSound != null)
            {
                unlockDoorSound.Play();
            }
            
            StartCoroutine(StopDoor());
            Destroy(Trigger);
            Destroy(CodePanel);

            EnablePlayerMovement();
        }
        else
        {
            Ans.text = "Invalid";
            EnablePlayerMovement();
        }
    }

    public void Clear()
    {
        Ans.text = "";
    }

    IEnumerator StopDoor()
    {
        yield return new WaitForSeconds(2.3f);
        Door.SetBool("Open", false);
        Door.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CodePanel.SetActive(true);
            DisablePlayerMovement();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        CodePanel.SetActive(false);
        EnablePlayerMovement();
    }

    // Navigation Logic with Input System
    public void OnNavigate(InputAction.CallbackContext context)
    {
        Vector2 navigationInput = context.ReadValue<Vector2>();
        
        if (navigationInput.y > 0)
        {
            // Navigate up
            NavigateToButton(selectedButtonIndex - 1);
        }
        else if (navigationInput.y < 0)
        {
            // Navigate down
            NavigateToButton(selectedButtonIndex + 1);
        }
    }

    public void OnSubmit(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            // Simulate button press
            ExecuteEvents.Execute(keypadButtons[selectedButtonIndex].gameObject, new BaseEventData(eventSystem), ExecuteEvents.submitHandler);
        }
    }

    private void NavigateToButton(int newIndex)
    {
        // Ensure the new index is within bounds
        if (newIndex >= 0 && newIndex < keypadButtons.Length)
        {
            selectedButtonIndex = newIndex;
            eventSystem.SetSelectedGameObject(keypadButtons[selectedButtonIndex].gameObject);
        }
    }

    private void DisablePlayerMovement()
    {
        if (firstPersonControls == null)
        {
            firstPersonControls = GetComponent<FirstPersonControls>();
        }
        if (firstPersonControls != null)
        {
            firstPersonControls.enabled = false;
        }
    }

    private void EnablePlayerMovement()
    {
        if (firstPersonControls == null)
        {
            firstPersonControls = GetComponent<FirstPersonControls>();
        }
        if (firstPersonControls != null)
        {
            firstPersonControls.enabled = true;
        }
    }
}
