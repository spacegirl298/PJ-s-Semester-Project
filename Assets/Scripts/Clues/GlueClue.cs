using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlueClue : MonoBehaviour
{
    
    public GameObject glueText;
    public GameObject glueTrigger;


    // Start is called before the first frame update
    void Start()
    {
        glueText.SetActive(false);

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            glueText.SetActive(true);
            
        }


        

    }


    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            glueText.SetActive(false);
            Destroy(glueTrigger);

        }
    }
}
