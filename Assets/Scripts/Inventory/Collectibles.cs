using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectibles : MonoBehaviour
{
   
    public string collectibleName; //yrn;scissors

    
    public Sprite collectibleSprite;

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.CompareTag("Player"))
        {
            
            InventoryManager playerInventory = other.GetComponent<InventoryManager>();

            if (playerInventory != null)
            {
                
                playerInventory.AddItem(collectibleName, collectibleSprite);

                
                gameObject.SetActive(false);
            }
            else
            {
                
            }
        }
    }
}