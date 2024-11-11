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

    [Header("Animations")]
    public bool isWaving = false;
    public Animator NPCcontroller;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = NpcSpeed;
        player = GameObject.FindGameObjectWithTag("Player");
        npcDialogue = GetComponent<NpcDialogue>(); 

        if (player == null)
        {
           
        }

        NPCcontroller.SetBool("isWavingtrue", true);
        AppearAtLocation(currentLocationIndex); // show at the starting location
    }

    private void Update()
    {
        if (hasAppeared)
        {
            ApproachPlayer(); 
        }
    }

    public void AppearAtLocation(int locationIndex)
    {
        if (locationIndex >= 0 && locationIndex < keyLocations.Length)
        {
            
            transform.position = keyLocations[locationIndex].position; 
            hasAppeared = true;

            
            UpdateDialogueForLocation(locationIndex);
        }
        else
        {
            Debug.LogWarning("Invalid location index for NPC.");
        }
    }

    private void ApproachPlayer()
    {
        if (player != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

            if (distanceToPlayer <= followRadius) // chek if player is within radius
            {
                if (distanceToPlayer > stopDistance)  // move NPC closer to player if they're far
                {
                    agent.SetDestination(player.transform.position);
                    agent.isStopped = false;
                    NPCcontroller.SetBool("ifWalkingisTrue", true);
                }
                else
                {
                    agent.isStopped = true;
                    NPCcontroller.SetBool("ifWalkingisTrue", false);

                    if (!isWaving)
                    {
                        NPCcontroller.SetBool("isWavingtrue", true);
                        isWaving = true;
                        StartCoroutine(waveWait());
                    }
                }
            }
            else
            {
                agent.isStopped = true;
                NPCcontroller.SetBool("ifWalkingisTrue", false);
                NPCcontroller.SetBool("isWavingtrue", false);
            }
        }
    }

    public void OnInteractionComplete()
    {
        hasAppeared = false; 
        currentLocationIndex++; 

        if (currentLocationIndex < keyLocations.Length)
        {
            AppearAtLocation(currentLocationIndex); // move NPC to the next location
        }
        else
        {
            Debug.Log("NPC @  the final location.");
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
            case 3:
                npcDialogue.UpdateLocation(NpcDialogue.Location.Location4); 
                break;
            default:
                break;
        }
    }

    public IEnumerator waveWait()
    {
        yield return new WaitForSeconds(0.5f);
        NPCcontroller.SetBool("isWavingtrue", false);
    }
}
