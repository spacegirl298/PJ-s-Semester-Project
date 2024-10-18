using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookShelf : MonoBehaviour
{
    public GameObject bookPushText;
    public GameObject bookTrigger;


    // Start is called before the first frame update
    void Start()
    {
        bookPushText.SetActive(false);

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            bookPushText.SetActive(true);
            
        }


        

    }


    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            bookPushText.SetActive(false);
            Destroy(bookTrigger);

        }
    }
}
