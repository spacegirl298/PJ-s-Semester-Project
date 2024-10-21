using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPad : MonoBehaviour
{
    public float boostForce = 5;

    private void OnTriggerEnter(Collider other)
    {
      //  Debug.Log("player jumped");

        if (other.CompareTag("Player"))
        {
           
            FirstPersonControls playerControls = other.GetComponent<FirstPersonControls>();
            if (playerControls != null)
            {
               
                playerControls.JumpPad(boostForce);
            }
        }
    }
}