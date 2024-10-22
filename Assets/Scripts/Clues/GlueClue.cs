using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
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
            
            
        }


        

    }


    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(TextDisappear());
            //glueText.SetActive(false);
           

        }
    }

    public IEnumerator TextDisappear()
    {
        glueText.SetActive(true );

        yield return new WaitForSeconds(1f);
        glueText.SetActive(false);

        yield return new WaitForSeconds(10f);
        Destroy(glueTrigger);
    }
}
