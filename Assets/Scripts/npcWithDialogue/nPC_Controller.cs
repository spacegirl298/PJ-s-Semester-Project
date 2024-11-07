using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Npc_Controller : MonoBehaviour
{
    [Header("npc AI Reference")]
    public Npc_AI Npc_AI; 

    [Header("appear Trigger points")]
    public Transform[] appearanceTriggers; 

    private int nextAppearanceIndex = 0; 

    private void Start()
    {
        if (Npc_AI == null)
        {
           
        }
    }

    private void Update()
    {
        
        if (nextAppearanceIndex < appearanceTriggers.Length) // check if there are more locations left for npc to appear at
        {
         
            if (IsPlayerNear(appearanceTriggers[nextAppearanceIndex].position))    // chec if the player is near the next trigger point
            {
                TriggerPatchAppearance();
                nextAppearanceIndex++;
            }
        }
    }

  
    private bool IsPlayerNear(Vector3 targetPosition)   // check  the player is near a given position 
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            float distance = Vector3.Distance(player.transform.position, targetPosition);
            return distance < 2.0f; 
        }
        return false;
    }

    
    private void TriggerPatchAppearance() // calls the AppearAtLocation method on on npcai script
    {
        Npc_AI.AppearAtLocation(nextAppearanceIndex);
        Debug.Log("Patch appeared at location " + nextAppearanceIndex);
    }
}