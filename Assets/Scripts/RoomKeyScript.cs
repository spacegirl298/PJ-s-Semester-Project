using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyScript : MonoBehaviour
{
    public GameObject doorCollider;
    // Start is called before the first frame update
    void Start()
    {
        doorCollider.SetActive(false);
    }

    // Update is called once per frame
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            doorCollider.SetActive(true);
            Destroy(gameObject);


        }
    }
}