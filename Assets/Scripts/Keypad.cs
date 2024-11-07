using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class Keypad : MonoBehaviour
{
    private FirstPersonControls firstPersonControls; 
    public GameObject player;
    private Controls Controls; 

    [SerializeField] private Text Ans;
    [SerializeField] private Animator Door;

    private string Answer = "0451";
    private string Input;

    public GameObject CodePanel;
    public GameObject Trigger;
    public GameObject Doors;

    public Button[] keypadButtons; 
    private int selectedButtonIndex = 0; 
    private EventSystem eventSystem; 

    public AudioSource apolloDoorSound;
    public AudioClip apolloDoorClip;
    
    

    private void Awake()
    {
        firstPersonControls = player.GetComponent<FirstPersonControls>();
        Controls = new Controls();
        Controls.Player.Enable();
        eventSystem = EventSystem.current; 
        
        
        Controls.Player.Interact.performed += ctx => Interact(); //interact method
    }

    private void Start()
    {
        if (keypadButtons.Length > 0)
        {
            eventSystem.SetSelectedGameObject(keypadButtons[selectedButtonIndex].gameObject);
        }
    }

    private void Interact()
    {
       
        if (IsLookingAtKeypad())
        {
            ShowKeypad();
        }
    }

    private bool IsLookingAtKeypad()
    {
        Ray ray = new Ray(player.transform.position, player.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 15f)) 
        {
            Debug.Log("raycast on: " + hit.collider.name);
            return hit.collider.CompareTag("Keypad");
        }
        return false;
    }

    private void ShowKeypad()
    {
        
        CodePanel.SetActive(true);
        DisablePlayerMovement();
        eventSystem.SetSelectedGameObject(keypadButtons[selectedButtonIndex].gameObject); 
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
            //apolloDoorSound.Play();

            if (apolloDoorSound != null)
            {
                apolloDoorSound.Play();
            }
            
           /* if (unlockDoorSound != null)
            {
                unlockDoorSound.Play();
            } */
            
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

    public void OnNavigate(InputAction.CallbackContext context)
    {
        Vector2 navigationInput = context.ReadValue<Vector2>();
        
        if (navigationInput.y > 0)
        {
            NavigateToButton(selectedButtonIndex - 1);
        }
        else if (navigationInput.y < 0)
        {
            NavigateToButton(selectedButtonIndex + 1);
        }
    }

    public void OnSubmit(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            ExecuteEvents.Execute(keypadButtons[selectedButtonIndex].gameObject, new BaseEventData(eventSystem), ExecuteEvents.submitHandler);
        }
    }

    private void NavigateToButton(int newIndex)
    {
        if (newIndex >= 0 && newIndex < keypadButtons.Length)
        {
            selectedButtonIndex = newIndex;
            eventSystem.SetSelectedGameObject(keypadButtons[selectedButtonIndex].gameObject);
        }
    }

    private void DisablePlayerMovement()
    {
        if (firstPersonControls != null)
        {
            firstPersonControls.enabled = false;
        }
    }

    private void EnablePlayerMovement()
    {
        if (firstPersonControls != null)
        {
            firstPersonControls.enabled = true;
        }
    }

    private void OnDestroy()
    {
        
        Controls.Player.Interact.performed -= ctx => Interact();
    }
}
