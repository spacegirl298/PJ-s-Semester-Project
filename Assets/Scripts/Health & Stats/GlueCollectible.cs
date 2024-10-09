using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlueCollectible : MonoBehaviour
{
    private bool isCollected = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isCollected)
        {
            PlayerStats playerStats = other.GetComponent<PlayerStats>();
            if (playerStats != null)
            {
                playerStats.CollectGlue(); 
                isCollected = true;

                //jaiden add particles 
                
                StartCoroutine(DisableCollectible()); 
            }
        }
    }

    
    private IEnumerator DisableCollectible()
    {
        
        yield return new WaitForSeconds(1f); //before setactive false
        gameObject.SetActive(false); 
    }
}