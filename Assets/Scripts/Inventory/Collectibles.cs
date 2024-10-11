using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectibles : MonoBehaviour
{
    // The name of the collectible (e.g., "Needle", "Yarn", etc.)
    public string collectibleName;

    // The sprite associated with the collectible
    public Sprite collectibleSprite;

    private void OnTriggerEnter(Collider other)
    {
        // Check if the player has collided with the collectible
        if (other.CompareTag("Player"))
        {
            // Get a reference to the player's InventoryManager script
            InventoryManager playerInventory = other.GetComponent<InventoryManager>();

            if (playerInventory != null)
            {
                // Add the item to the inventory using its name and sprite
                playerInventory.AddItem(collectibleName, collectibleSprite);

                // Disable the collectible object after it's picked up
                gameObject.SetActive(false);
            }
            else
            {
                
            }
        }
    }
}