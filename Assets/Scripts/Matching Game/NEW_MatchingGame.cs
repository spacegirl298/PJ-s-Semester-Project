using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class NEW_MatchingGame : MonoBehaviour
{
    public Transform playerCamera;
    public float pickUpRange = 5f;
    public GameObject heldObject;
    public Transform holdPosition;

    public Transform blueSnap;
    public Transform greenSnap;
    public Transform purpleSnap;

    private Controls controls;

    private bool isBlueBearMatched = false;
    private bool isGreenBearMatched = false;
    private bool isPurpleBearMatched = false;

    public Animator matchingDrawer;
    public AudioSource matchingDrawerSound;


    public AudioClip matchedSound;
    private AudioSource matchedAudioSource;

    private void Awake()
    {
        controls = new Controls();
        matchedAudioSource = gameObject.AddComponent<AudioSource>();
        matchedAudioSource.clip = matchedSound;
    }

    private void Update()
    {

        if (isBlueBearMatched && isGreenBearMatched && isPurpleBearMatched)
        {
            matchingDrawer.SetBool("NewMatching", true);
            matchingDrawerSound.Play();
        }
    }

    private void OnEnable()
    {
        controls.Player.PickUp.performed += ctx => PickUpObject();
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Player.PickUp.performed -= ctx => PickUpObject();
        controls.Disable();
    }

    public void PickUpObject()
    {
        if (heldObject != null)
        {
            DropObject(); // drop if already holding bear
            return;
        }

        Ray ray = new Ray(playerCamera.position, playerCamera.forward);
        RaycastHit hit;

        Debug.DrawRay(playerCamera.position, playerCamera.forward * pickUpRange, Color.red, 2f);

        if (Physics.Raycast(ray, out hit, pickUpRange))
        {
            if (hit.collider.CompareTag("blueBear") || hit.collider.CompareTag("greenBear") ||
                hit.collider.CompareTag("purpleBear"))
            {
                heldObject = hit.collider.gameObject;
                heldObject.GetComponent<Rigidbody>().isKinematic = true;

                heldObject.transform.position = holdPosition.position;
                heldObject.transform.rotation = holdPosition.rotation;
                heldObject.transform.parent = holdPosition;
            }
        }
    }

    public void DropObject()
    {
        if (heldObject != null)
        {
            string bearTag = heldObject.tag;

            Ray ray = new Ray(playerCamera.position, playerCamera.forward);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, pickUpRange))
            {
                // see if the object is mat and matches the bear color
                if ((bearTag == "blueBear" && hit.collider.CompareTag("blueMat")) ||
                    (bearTag == "greenBear" && hit.collider.CompareTag("greenMat")) ||
                    (bearTag == "purpleBear" && hit.collider.CompareTag("purpleMat")))
                {
                    SnapBearToMat(bearTag); // send the bear tag for state update
                }
                else
                {
                    ReleaseBear();
                }
            }
        }
    }

    private void ReleaseBear()
    {
        if (heldObject != null)
        {
            Rigidbody heldObjectRb = heldObject.GetComponent<Rigidbody>();
            heldObjectRb.isKinematic = false;
            heldObjectRb.useGravity = true;

            heldObject.transform.parent = null;
            heldObjectRb.AddForce(Vector3.down * 2f, ForceMode.Impulse);
            heldObject = null;
        }
    }

    private void SnapBearToMat(string bearTag)
    {
        Transform targetPosition = null;

        switch (bearTag)
        {
            case "blueBear":
                targetPosition = blueSnap;
                break;
            case "greenBear":
                targetPosition = greenSnap;
                break;
            case "purpleBear":
                targetPosition = purpleSnap;
                break;
        }

        if (targetPosition != null)
        {
            heldObject.transform.position = targetPosition.position;
            heldObject.transform.rotation = targetPosition.rotation;
            heldObject.transform.SetParent(null);
            heldObject.GetComponent<Collider>().enabled = false;

            UpdateMatchState(bearTag); // update the match state
            LightUpMat(bearTag); // light/change mat color


            matchedAudioSource.Play(); //play everytime matched

            heldObject = null;
        }
    }

    private void LightUpMat(string bearTag)
    {
        switch (bearTag)
        {
            case "blueBear":
                ChangeMatColor("blueMat", Color.blue);
                break;
            case "greenBear":
                ChangeMatColor("greenMat", Color.green);
                break;
            case "purpleBear":
                ChangeMatColor("purpleMat", Color.magenta);
                break;
            default:
                break;
        }
    }

    private void ChangeMatColor(string matTag, Color color)
    {
        GameObject mat = GameObject.FindGameObjectWithTag(matTag);
        if (mat != null)
        {
            Renderer matRenderer = mat.GetComponent<Renderer>();
            if (matRenderer != null)
            {
                matRenderer.material.color = color;
            }
        }
    }

    private void UpdateMatchState(string bearTag)
    {
        switch (bearTag)
        {
            case "blueBear":
                isBlueBearMatched = true;
                break;
            case "greenBear":
                isGreenBearMatched = true;
                break;
            case "purpleBear":
                isPurpleBearMatched = true;
                break;
        }
    }

}
