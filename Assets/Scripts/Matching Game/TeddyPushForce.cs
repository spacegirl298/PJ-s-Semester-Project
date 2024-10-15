using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TeddyPushForce : MonoBehaviour
{
    public float pushForce = 0.7f;
    public float maxVelocity = 1f; 

   
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
        if (rb != null && hit.collider.CompareTag("TeddyPush")) //check if it has rigidbody
        {
            //calculate force direction 
            Vector3 forceDirection = hit.gameObject.transform.position - transform.position;   //suntract player position from position of 
            forceDirection.y = 0; //so it moves on x only
            forceDirection.Normalize(); //so it had magnitude of 1
            
            //method appplies forcd at specific posit
            rb.AddForceAtPosition(forceDirection * pushForce, transform.position, ForceMode.Impulse);//force is applied immediately(impulse) yo the rigidbody//FORCE applied from where character is standing
            
            rb.AddForce(forceDirection * pushForce, ForceMode.Force); 
            
            
            if (rb.velocity.magnitude > maxVelocity) // limit the velocity to prevent the object from moving too fast
            {
                rb.velocity = rb.velocity.normalized * maxVelocity;
            }
        }
    }
}