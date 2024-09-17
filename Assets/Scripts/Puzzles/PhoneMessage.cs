using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PhoneMessage : MonoBehaviour
{
    public Transform player; 
    public float distanceFromPhone = 3f; //radius
    public GameObject messagePanel; 

    private void Start()
    {
        messagePanel.SetActive(false); 
    }

    private void Update()
    {
        float distance = Vector3.Distance(player.position, transform.position);
        if (distance < distanceFromPhone)
        {
            messagePanel.SetActive(true); 
            transform.LookAt(player); 
        }
        else
        {
            messagePanel.SetActive(false); 
        }
    }

    
    private void OnDrawGizmos()
    {
        if (player != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, distanceFromPhone);
        }
    }
}