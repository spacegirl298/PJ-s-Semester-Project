using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class MatchingGAme : MonoBehaviour
{
    public GameObject[] gameObjects;
    public bool Match1;
    public bool Match2;
    public bool Match3;
    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject == gameObjects[0])
        {
            Match1 = true;
            Debug.Log("Blue matched");
        }

        if(other.gameObject == gameObjects[1])
        {
            Match2 = true;
            Debug.Log("Purple matched");
        }

        if (other.gameObject == gameObjects[2])
        {
            Match3 = true;
            Debug.Log("Green matched");
        }

    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == gameObjects[0])
        {
            Match1 = false; 
        }

        if(other.gameObject == gameObjects[1])
        {
            Match2 = false;
        }

        if (other.gameObject == gameObjects[2])
        {
            Match3 = false;
        }
    }

    private void Update()
    {
        if(Match1 == true)
        {
            if (Match2 == true)
            {

                if (Match3 == true)
                {
                    Debug.Log("Open");
                }
            }
        }
    }
}
