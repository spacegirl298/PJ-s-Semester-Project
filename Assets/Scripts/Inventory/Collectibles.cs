using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectibles : MonoBehaviour
{
   
    public string collectibleName; 

    
    public Sprite collectibleSprite;

    private ParticleSystem collectionSystem;

    public GameObject collectionEffect;

    private void Start()
    {
        collectionSystem = collectionEffect.GetComponent<ParticleSystem>();
    }
    private void OnTriggerEnter(Collider other)
    {
        
        if (other.CompareTag("Player"))
        {
            
            InventoryManager playerInventory = other.GetComponent<InventoryManager>();

            if (playerInventory != null)
            {
                
                playerInventory.AddItem(collectibleName, collectibleSprite);

                collectionSystem.Play();
                gameObject.SetActive(false);
            }
            else
            {
                
            }
        }
    }
}