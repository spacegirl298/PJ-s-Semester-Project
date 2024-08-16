using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Title: How to OPEN A DOOR With KEY in UNITY - Easy Tutorial
//Author/Youtube Channel: Kozmobot Games
//Date: 15 August 2024
//Code Version: 2
//Availability: https://youtu.be/RQ61nY2WOtA?feature=shared 
public class KeyScriptApollo : MonoBehaviour
{
    public GameObject doorCollider;
    // Start is called before the first frame update
    void Start()
    {
        doorCollider.SetActive(false);
    }

    // Update is called once per frame
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            doorCollider.SetActive(true);
            Destroy(gameObject);


        }
    }
}