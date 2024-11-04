using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestScript : MonoBehaviour
{
    [SerializeField] private Animator TopChest;
    public GameObject Trigger;
    public GameObject keyTrigger;
    public GameObject MainBeam;
    public GameObject LeftBeam;
    public GameObject RightBeam;
    public bool ChestOpen;
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {

            TopChest.SetBool("ChestOpen", true);
            Destroy(MainBeam);
            Destroy(LeftBeam);
            Destroy(RightBeam);
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
