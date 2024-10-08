using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Title: How to OPEN A DOOR With KEY in UNITY - Easy Tutorial
//Author/Youtube Channel: Kozmobot Games
//Date: 15 August 2024
//Code Version: 2
//Availability: https://youtu.be/RQ61nY2WOtA?feature=shared 
public class KeyScript : MonoBehaviour
{
    public GameObject doorCollider;

    private ParticleSystem collectionSystem;

    public GameObject collectionEffect;

    // Start is called before the first frame update
    void Start()
    {
        doorCollider.SetActive(false);
        collectionSystem = collectionEffect.GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            doorCollider.SetActive(true);
            collectionSystem.Play();
            Destroy(gameObject);


        }
    }
}