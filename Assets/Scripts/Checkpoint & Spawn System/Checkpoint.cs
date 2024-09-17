using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        
        if (other.CompareTag("Player"))
        {
            CheckpointManagers checkpointManager = other.GetComponent<CheckpointManagers>();

            if (checkpointManager != null)
            {
                // Update the checkpoint in the CheckpointManager
                checkpointManager.UpdateCheckpoint(transform.position);
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
