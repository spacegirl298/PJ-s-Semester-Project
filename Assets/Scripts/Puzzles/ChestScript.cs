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

    private ParticleSystem ExplosionLeft;
    private ParticleSystem ExplosionMiddle;
    private ParticleSystem ExplosionRight;
    private ParticleSystem ExplosionRightCorner;

    public GameObject explosionLeft;
    public GameObject explosionMiddle;
    public GameObject explosionRight;
    public GameObject explosionRightCorner;
    
    
   


    public AudioClip detonatorClip;
    public AudioSource detonatorSound;
    // Start is called before the first frame update

    private void Start()
    {
        ExplosionLeft = explosionLeft.GetComponent<ParticleSystem>();
        ExplosionMiddle = explosionMiddle.GetComponent<ParticleSystem>();
        ExplosionRight = explosionRight.GetComponent<ParticleSystem>();
        ExplosionRightCorner = explosionRightCorner.GetComponent<ParticleSystem>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {

            TopChest.SetBool("ChestOpen", true);

            Destroy(MainBeam);
            Destroy(LeftBeam);
            Destroy(RightBeam);

            ExplosionLeft.Play();
            ExplosionMiddle.Play();
            ExplosionRight.Play();
            ExplosionRightCorner.Play();
            
            detonatorSound.Play();

            StartCoroutine(ExplosionsDestroyed());

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

    public IEnumerator ExplosionsDestroyed()
    {
        yield return new WaitForSeconds(1.9f);
        Destroy(ExplosionLeft);
        Destroy(ExplosionMiddle);
        Destroy(ExplosionRight);
        Destroy(ExplosionRightCorner);
    }
}
