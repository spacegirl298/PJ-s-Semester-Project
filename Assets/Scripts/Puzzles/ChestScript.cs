using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestScript : MonoBehaviour
{
    [SerializeField] private Animator TopChest;
    [SerializeField] private Animator Key;
    
    public bool KeyMove;

    public GameObject Trigger;
    //public GameObject keyTrigger;
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

            StartCoroutine(KeyAnimation());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Destroy(Trigger);
            //keyTrigger.SetActive(true);
        }
    }

    public IEnumerator KeyAnimation()
    {
        yield return new WaitForSeconds(1f);
        Key.SetBool("KeyMove", true);
        //  Debug.Log("Chest Triggered Key Trigger!");
    }
}
