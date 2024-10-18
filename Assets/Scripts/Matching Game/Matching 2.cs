using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Matching2 : MonoBehaviour
{
    public GameObject Purple;
    
    
    public bool Match2;
    
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject == Purple)
        {
            Match2 = true;
            Debug.Log("Blue matched");
        }



    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == Purple)
        {
            Match2 = false;
        }


    }

}