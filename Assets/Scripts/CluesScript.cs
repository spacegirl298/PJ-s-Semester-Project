using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CluesScript : MonoBehaviour
{
    public GameObject detonatorText;
    public GameObject detonatorTrigger;


    // Start is called before the first frame update
    void Start()
    {
        detonatorText.SetActive(false);

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            detonatorText.SetActive(true);
            
        }


        

    }


    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            
            StartCoroutine(JumpText());

        }
    }

    public IEnumerator JumpText()
    {
        yield return new WaitForSeconds(1f);
        detonatorText.SetActive(false);

        yield return new WaitForSeconds(5f);
        Destroy(detonatorTrigger);
    }
}
