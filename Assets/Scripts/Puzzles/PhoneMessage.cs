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

    [Header("Phone Audio")]
    [Space(5)]
    public AudioSource audioSource; 
    public AudioClip phoneAudio;
    private void Start()
    {
        messagePanel.SetActive(false); 
        
        if (audioSource != null)
        {
            audioSource.clip = phoneAudio; 
            audioSource.loop = true; 
            audioSource.Play(); 
        }
    }

    private void Update()
    {
        float distance = Vector3.Distance(player.position, transform.position);
        if (distance < distanceFromPhone)
        {
            messagePanel.SetActive(true); 
            transform.LookAt(player);
            transform.rotation = Quaternion.Euler(new Vector3(0f, 90f, 0f));
            
            if (audioSource != null && audioSource.isPlaying) 
            {
                audioSource.Stop();
            }
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