using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorPopUp : MonoBehaviour
{
    public GameObject textPop;
    void Start()
    {
        textPop.SetActive(false);
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            textPop.SetActive(true);
        }
    }
}
