using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillBox : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        
        if (other.CompareTag("Player"))
        {
            CheckpointManagers checkpointManagers = other.GetComponent<CheckpointManagers>();

            if (checkpointManagers != null)
            {
                // Call the StartRespawn method from checkpoint manager script to send the player back to the last checkpoint after some secondsas
                checkpointManagers.StartRespawn();
            }
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}