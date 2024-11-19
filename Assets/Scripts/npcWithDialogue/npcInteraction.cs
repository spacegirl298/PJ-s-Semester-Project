using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCInteraction : MonoBehaviour
{
    public GameObject interactUI; 
    private bool isPlayerInTrigger = false; 
    private Controls controls; 

    void Awake()
    {
       
        controls = new Controls();
        controls.Player.Enable();

        
        controls.Player.Interact.performed += ctx => OnInteract();
    }

    void Start()
    {
        interactUI.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            interactUI.SetActive(true); 
            isPlayerInTrigger = true; 
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            interactUI.SetActive(false); 
            isPlayerInTrigger = false; 
        }
    }

    private void OnInteract()
    {
       
        if (isPlayerInTrigger)  // only hide the UI if the player is inside the trigger zone
        {
            interactUI.SetActive(false);
            Debug.Log("interacted with NPC!");
        }
    }

    void OnDestroy()
    {
      
        controls.Player.Interact.performed -= ctx => OnInteract();
    }
}