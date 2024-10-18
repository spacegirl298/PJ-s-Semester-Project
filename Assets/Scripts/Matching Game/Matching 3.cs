using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Matching3 : MonoBehaviour
{
    public GameObject Green;
   
    
    public bool Match3;
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject == Green)
        {
            Match3 = true;
            Debug.Log("Blue matched");
        }



    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == Green)
        {
            Match3 = false;
        }


    }

}

