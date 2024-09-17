using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointManagers : MonoBehaviour
{
    private Vector3 lastCheckpointPosition; // stores the position of the last checkpoint
    private CharacterController characterController;
    
    // Start is called before the first frame update
    void Start()
    {
        lastCheckpointPosition = transform.position;
        characterController = GetComponent<CharacterController>(); //to initialise starting position on teddy bear player
    }
    
//  method called when the player gets to a checkpoint
    public void UpdateCheckpoint(Vector3 newCheckpointPosition)
    {
        lastCheckpointPosition = newCheckpointPosition;
        Debug.Log("Checkpoint updated: " + lastCheckpointPosition);
    }

    /* //  method that handles the respawn of the player
     public void Respawn()
     {
         // i Disabled character controller before setting position to avoid movement issues
         characterController.enabled = false;
         transform.position = lastCheckpointPosition;
         characterController.enabled = true;

         Debug.Log("Player respawned at: " + lastCheckpointPosition);
     }
     */
   
    //  This method starts the respawn process with a delay
    public void StartRespawn()
    {
        StartCoroutine(RespawnAfterDelay(0.5f)); 
    }

    // Coroutine for respawn
    private IEnumerator RespawnAfterDelay(float delay)
    {

        //  disable player controls  to prevent movement during waiting time 
        characterController.enabled = false;

        // wait few seconds THEN respond
        yield return new WaitForSeconds(delay);

        // respawn the player at the last checkpoint
        transform.position = lastCheckpointPosition;

       
        characterController.enabled = true;  //  character controller starts again  after respawn

        Debug.Log("Player respawned at: " + lastCheckpointPosition);
    }
}