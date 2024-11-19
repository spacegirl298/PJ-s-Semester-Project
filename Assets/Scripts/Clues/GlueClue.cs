using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class GlueClue : MonoBehaviour
{
    
    public GameObject interactUI;
    public GameObject glueTrigger;


    // Start is called before the first frame update
    void Start()
    {
        interactUI.SetActive(false);

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(DestroyText());

        }


        

    }


    /*private void OnTriggerExit(Collider other)
    {
        Destroy(glueText);
    }
*/
    public IEnumerator DestroyText()
    {
        interactUI.SetActive(true);

        yield return new WaitForSeconds(7f);
        Destroy(interactUI);
    }
}
