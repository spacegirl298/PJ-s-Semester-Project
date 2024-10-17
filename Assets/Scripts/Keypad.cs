using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;


public class Keypad : MonoBehaviour
{
    private FirstPersonControls firstPersonControls; //player script
    private Controls Controls; //defined controls in acfion 
    public GameObject player;


    [SerializeField] private Text Ans;
    [SerializeField] private Animator Door;

    private string Answer = "0451";

    private string Input;

    public GameObject CodePanel;
    public GameObject Trigger;
    public GameObject Doors;

    //public Vector3 newPosition;
    void awake()
    {
        firstPersonControls = player.GetComponent<FirstPersonControls>();
    }
    private void Start()
    {
        firstPersonControls = player.GetComponent<FirstPersonControls>();
    }

    public void Number(int number)
    {
        Ans.text += number.ToString();

    }

    public void Execute()
    {
        if (Ans.text == Answer)
        {
            //Destroy(CodePanel);
            //Door.transform.position = newPosition;
            Ans.text = "Correct";
            Door.SetBool("Open", true);
            StartCoroutine("StopDoor");
            Destroy(Trigger);
            Destroy(CodePanel);
            //Destroy(Trigger);

            if (firstPersonControls == null)
            {
                firstPersonControls = GetComponent<FirstPersonControls>();
            }

            if (firstPersonControls != null)
            {
                firstPersonControls.enabled = true; //enable movement when inventoy is open
            }


        }
        else
        {
            Ans.text = "Invalid";

            if (firstPersonControls == null)
            {
                firstPersonControls = GetComponent<FirstPersonControls>();
            }

            if (firstPersonControls != null)
            {
                firstPersonControls.enabled = true; //enable movement when inventoy is open
            }
        }
    }

    public void Clear()
    {
        Ans.text = "";
    }

    IEnumerator StopDoor()
    {
        yield return new WaitForSeconds(2.30f);
        Door.SetBool("Open", false);
        Door.enabled = false;
    }


    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Player")) ;
        {
            CodePanel.SetActive(true);

            if (firstPersonControls == null)
            {
                firstPersonControls = GetComponent<FirstPersonControls>();
            }

            if (firstPersonControls != null)
            {
                firstPersonControls.enabled = false;

            }
        }
        
        if (Ans.text == Answer)
        {
                Destroy(CodePanel);
                Destroy(Trigger);

                
                
                if (firstPersonControls == null)
                {
                    firstPersonControls = GetComponent<FirstPersonControls>();
                }

                if (firstPersonControls != null)
                {
                    firstPersonControls.enabled = true; //enable movement when inventoy is open
                } 
                
        }          
        

    }
    
    private void OnTriggerExit(Collider other)
    {
        CodePanel.SetActive(false);




    }
}