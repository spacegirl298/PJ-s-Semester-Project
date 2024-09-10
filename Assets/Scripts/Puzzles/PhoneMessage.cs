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
    
    // Start is called before the first frame update
    void Start()
    {
        messagePanel.SetActive(false); 
    }

    // Update is called once per frame
    void Update()
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
