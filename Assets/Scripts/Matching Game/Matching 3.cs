using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Matching3 : MonoBehaviour
{
    public LayerMask layerMask;
    public float raycastDistance = 1.0f;

    public bool greenDetected;



    Renderer m_Renderer;


    void Start()
    {
        m_Renderer = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        Detect();
    }

    public void Detect()
    {
        {
            Vector3 origin = transform.position;
            Vector3 direction = Vector3.up;

            if (Physics.Raycast(origin, direction, out RaycastHit hit, raycastDistance))
            {
                GameObject objectAbovePink = hit.collider.gameObject;

                if (objectAbovePink.name == "Green")
                {
                    greenDetected = true;
                    Renderer renderer = GetComponent<Renderer>();
                    renderer.material.color = new Color(0f, 1f, 0.0104f);
                    Debug.Log("Green player Detected");
                }

            }
            else
            {
                greenDetected = false;
                Renderer renderer = GetComponent<Renderer>();
                renderer.material.color = new Color(0.4899f, 0.7660f, 0.4928f);
                Debug.Log("Raycast did not hit anything");
            }

            Debug.DrawRay(origin, direction * raycastDistance, Color.green);
        }
    }

}

