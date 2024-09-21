using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Matching2 : MonoBehaviour
{
    //Raycast
    public LayerMask layerMask;
    public float raycastDistance = 1.0f;

    public bool purpleDetected;

    

    //colour change 
    Renderer m_Renderer;


    void Start()
    {
        m_Renderer = GetComponent<Renderer>(); //for chnaging of colour 
    }

    // Update is called once per frame
    void Update()
    {
        Detect();
    }

    public void Detect()
    {
        // honestly could have put this code in the Update() but oh well 
        {
            Vector3 origin = transform.position;
            Vector3 direction = Vector3.up;

            if (Physics.Raycast(origin, direction, out RaycastHit hit, raycastDistance)) // setting up a raycast that goes above the object 
            {
                GameObject objectAbovePink = hit.collider.gameObject;

                if (objectAbovePink.name == "Purple") //this changes the colour of object if the object above it is named Pink
                {
                    purpleDetected = true;
                    Renderer renderer = GetComponent<Renderer>();
                    renderer.material.color = new Color(1f, 0.4393f, 0f);
                    Debug.Log("Purple player Detected");
                }


            }
            else
            { // this is so it changes to the original colour if the right object is not above it 
                purpleDetected = false;
                Renderer renderer = GetComponent<Renderer>();
                renderer.material.color = new Color(0.8490f, 0.6308f, 0.4597f);
                Debug.Log("Raycast did not hit anything");
            }

            Debug.DrawRay(origin, direction * raycastDistance, Color.green); //makes the array visible 
        }
    }


}
