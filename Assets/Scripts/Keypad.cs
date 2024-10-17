using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Keypad : MonoBehaviour
{
     FirstPersonControls firstPersonControls; //player script
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

        }
        else
        {
            Ans.text = "Invalid";
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
        
        if (other.tag == "Player")
        {
            CodePanel.SetActive(true);
           
            if (firstPersonControls == null)
            {
                firstPersonControls = GetComponent<FirstPersonControls>();
            }

            if (firstPersonControls != null)
            {
                firstPersonControls.enabled = false; //disable movement when inventoy is open
            }

        }

        if (Ans.text == Answer)
        {
            Destroy(CodePanel);
            Destroy(Trigger);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        CodePanel.SetActive(false);
        
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