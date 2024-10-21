using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingKey : MonoBehaviour
{
    [SerializeField] private Animator Key;
    public GameObject KeyTrigger;
    public bool KeyMove;
    
    
       
        // Start is called before the first frame update
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {

                Key.SetBool("KeyMove", true);
              //  Debug.Log("Chest Triggered Key Trigger!");

            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.tag == "Chest")
            {
                Destroy(KeyTrigger);
            }
        }
    }

