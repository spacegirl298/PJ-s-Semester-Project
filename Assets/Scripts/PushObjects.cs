using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Ttitle: Push Obstacles Using a Character Controller (Unity Tutorial)
//Author/Youtube Channel: Ketra Games
//Date: 14 August 2024
//Code Version: 2
//Availability: https://youtu.be/3BOn2gs7z04?feature=shared
public class PushObjects : MonoBehaviour
{
    public float pushForce = 1;
   
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnControllerColliderHit(ControllerColliderHit hit) //called when character collides with anything
    {
        Rigidbody rb = hit.collider.attachedRigidbody; //gets rb of books
        if (rb != null && hit.collider.CompareTag("Moveable")) //check if it has rigidbody
        {
            //calculate force direction 
            Vector3 forceDirection = hit.gameObject.transform.position - transform.position;   //suntract player position from position of book
            forceDirection.y = 0; //so it moves on x only
            forceDirection.Normalize(); //so it had magnitude of 1
            
            //method appplies forcd at specific posit
            rb.AddForceAtPosition(forceDirection * pushForce, transform.position, ForceMode.Impulse);//force is applied immediately(impulse) yo the rigidbody//FORCE applied from where character is standing
        }
    }
}