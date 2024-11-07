using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Npc_AI : MonoBehaviour
{
    [Header("Settings")]
    public float NpcSpeed = 2f; 
    public float stopDistance = 2f; 
    public float followRadius = 5f;
    public Transform[] keyLocations; 
  

    private NavMeshAgent agent;
    private GameObject player;
    private bool hasAppeared = false;
    private int currentLocationIndex = 0;
    private NpcDialogue npcDialogue; 

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = NpcSpeed;
        player = GameObject.FindGameObjectWithTag("Player");
        npcDialogue = GetComponent<NpcDialogue>(); 

        if (player == null)
        {
            
        }
    }

    private void Update()
    {
        
        if (!hasAppeared && currentLocationIndex < keyLocations.Length)
        {
            AppearAtLocation(currentLocationIndex);
        }
        else if (hasAppeared)
        {
            ApproachPlayer(); 
        }
    }

    
    public void AppearAtLocation(int locationIndex)
    {
        if (locationIndex >= 0 && locationIndex < keyLocations.Length)
        {
            
            transform.position = keyLocations[locationIndex].position; //move npc to location gaeobject 
            hasAppeared = true;

           
            UpdateDialogueForLocation(locationIndex);
        }
        else
        {
           
        }
    }

    
    private void ApproachPlayer()
    {
        if (player != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

            
            if (distanceToPlayer <= followRadius) // chec k if the player is within   radius
            {
               
                if (distanceToPlayer > stopDistance)  // if the npc is still farr than the stop distance move it mkre to plahyer
                {
                    agent.SetDestination(player.transform.position);
                    agent.isStopped = false; 
                }
                else
                {
                    agent.isStopped = true;

                    
                }
            }
            else
            {
                agent.isStopped = true; 
            }
        }
    }

    
    public void OnInteractionComplete()
    {
        hasAppeared = false; 
        currentLocationIndex++; 

        
        if (currentLocationIndex < keyLocations.Length)
        {
            AppearAtLocation(currentLocationIndex); 
        }
        else
        {
          
        }
    }

    private void UpdateDialogueForLocation(int locationIndex)
    {
        switch (locationIndex)
        {
            case 0:
                npcDialogue.UpdateLocation(NpcDialogue.Location.Location1); 
                break;
            case 1:
                npcDialogue.UpdateLocation(NpcDialogue.Location.Location2); 
                break;
            case 2:
                npcDialogue.UpdateLocation(NpcDialogue.Location.Location3); 
                break;
            default:
                break;
        }
    }
}
