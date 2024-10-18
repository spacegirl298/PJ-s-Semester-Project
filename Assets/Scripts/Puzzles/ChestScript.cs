using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestScript : MonoBehaviour
{
    [SerializeField] private Animator TopChest;
    public GameObject Trigger;
    public GameObject keyTrigger;
    public bool ChestOpen;
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {

            TopChest.SetBool("ChestOpen", true);

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Destroy(Trigger);
            keyTrigger.SetActive(true);
        }
    }
}
