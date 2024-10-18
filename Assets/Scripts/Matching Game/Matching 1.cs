using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Matching1 : MonoBehaviour
{
    public GameObject Blue;
    public Animator matchingDrawer;
    public bool Match1;
   

    public Matching2 Matching2;
    public Matching3 Matching3;
    public void Update()
    {
       
        
        if (Match1 == true)
        {
            if (Matching2.Match2 == true)
            {

                if (Matching3.Match3 == true)
                {
                    Debug.Log("OPEN SESAME");
                    matchingDrawer.SetBool("MatchCorrect", true);
                    
                }
            }
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject == Blue)
        {
            Match1 = true;
            Debug.Log("Blue matched");
        }

       

    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == Blue)
        {
            Match1 = false;
        }

        
    }


}
