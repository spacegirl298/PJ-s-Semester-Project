using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class EyeLidsScript : MonoBehaviour
{
    public Animator EyeLids;
    public GameObject Eyes;
    public bool Blinking;

    public GameObject dialoguePanel;
    public TMP_Text dialogueText;

    // Start is called before the first frame update
    void Start()
    {
        EyeLids.SetBool("Blinking", true);

        dialoguePanel.SetActive(false);
        StartCoroutine(StoppedBlinking());
    }

    public void StopBlink()
    {
        
        EyeLids.SetBool("Blinking", false);
        Eyes.SetActive(false);

    }

    public IEnumerator StoppedBlinking()
    {
        yield return new WaitForSeconds(5.5f);
        dialoguePanel.SetActive(true);
        dialogueText.text = "Where...Where am I?";


        yield return new WaitForSeconds(4f);
        dialoguePanel.SetActive(false); //delete panel after 10 secs
    }

    

}