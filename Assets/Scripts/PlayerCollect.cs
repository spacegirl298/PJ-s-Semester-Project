using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollect : MonoBehaviour
{
    public AudioSource collectSound;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Collectible") || (other.CompareTag("Powerup")))
        {
            if (collectSound != null)
            {
                collectSound.Play();
            }
             
            other.gameObject.SetActive(false); 
            
            //particles below
        }
    }
}