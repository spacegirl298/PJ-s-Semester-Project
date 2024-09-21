using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Matching1 : MonoBehaviour
{
    public LayerMask layerMask;
    public float raycastDistance = 1.0f;


    public bool blueDetected;

    public Matching2 matching2;
    public Matching3 matching3;

    public GameObject Drawer;


    Renderer m_Renderer;


    void Start()
    {
        m_Renderer = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        Detect();

        //this detects if all the teddy bears are matched to the right objects
        if (matching2.purpleDetected && blueDetected && matching3.greenDetected)
        {
            Drawer.transform.position = new Vector3(1.730592f, 2.406113f, 0.871f);
        }
    }

    public void Detect()
    {
        {
            Vector3 origin = transform.position;
            Vector3 direction = Vector3.up;

            if (Physics.Raycast(origin, direction, out RaycastHit hit, raycastDistance))
            {
                GameObject objectAbovePink = hit.collider.gameObject;

                if (objectAbovePink.name == "Blue")
                {
                    blueDetected = true;
                    Renderer renderer = GetComponent<Renderer>();
                    renderer.material.color = new Color(0, 0.6799f, 1f); //dark blue
                    Debug.Log(" Blue Player Detected");
                }


            }
            else
            {
                blueDetected = false;
                Renderer renderer = GetComponent<Renderer>();
                renderer.material.color = new Color(0.3649f, 0.6325f, 0.7584f); //light Blue
                Debug.Log("Raycast did not hit anything");
            }

            Debug.DrawRay(origin, direction * raycastDistance, Color.green);
        }
    }


}

