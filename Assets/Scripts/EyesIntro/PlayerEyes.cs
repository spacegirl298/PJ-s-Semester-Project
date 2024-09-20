using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TeddyBearIntro : MonoBehaviour
{
    public GameObject topEye; 
    public GameObject bottomEye; 
    public GameObject dialoguePanel; 
    public TMP_Text dialogueText; 

    public float eyeBlinkSpeed = 0.5f; // Speed of the eye movement
    public float blinkDelay = 0.3f; // time  between blinks
    public float blinkWaitTime = 1f; //  time waited before dialogue appears

    private RectTransform topEyelidRect;
    private RectTransform bottomEyelidRect;

    private void Start()
    {
        topEyelidRect = topEye.GetComponent<RectTransform>();
        bottomEyelidRect = bottomEye.GetComponent<RectTransform>();

        dialoguePanel.SetActive(false);
        StartCoroutine(OpenEyesAndBlink());
    }

    private IEnumerator OpenEyesAndBlink()
    {
        CloseEyelids();
        yield return OpenEyelids();

        // Blink eyes 3 times
        for (int i = 0; i < 3; i++)
        {
            yield return new WaitForSeconds(blinkDelay);
            CloseEyelids();   // Close eyes
            yield return new WaitForSeconds(eyeBlinkSpeed);
            yield return OpenEyelids();  // Open eyes
        }

        // Wait before showing dialogue panel
        yield return new WaitForSeconds(blinkWaitTime);

        
        dialoguePanel.SetActive(true);
        dialogueText.text = "Where...Where am I?";

       
        yield return new WaitForSeconds(7f);
        dialoguePanel.SetActive(false); //delete panel after 10 secs
    }

    private void CloseEyelids()
    {
        topEyelidRect.anchoredPosition = new Vector2(0, 0); 
        bottomEyelidRect.anchoredPosition = new Vector2(0, 0); 
    }

    private IEnumerator OpenEyelids()
    {
        float elapsedTime = 0f;

        // eyes move to their original positions (offscreen)
        Vector2 topStartPos = topEyelidRect.anchoredPosition;
        Vector2 topEndPos = new Vector2(0, Screen.height / 2);

        Vector2 bottomStartPos = bottomEyelidRect.anchoredPosition;
        Vector2 bottomEndPos = new Vector2(0, -Screen.height / 2);

        while (elapsedTime < eyeBlinkSpeed)
        {
            elapsedTime += Time.deltaTime;

            // Move eyelids
            topEyelidRect.anchoredPosition = Vector2.Lerp(topStartPos, topEndPos, elapsedTime / eyeBlinkSpeed);
            bottomEyelidRect.anchoredPosition = Vector2.Lerp(bottomStartPos, bottomEndPos, elapsedTime / eyeBlinkSpeed);

            yield return null;
        }

        // Ensure eyelids are fully open
        topEyelidRect.anchoredPosition = topEndPos;
        bottomEyelidRect.anchoredPosition = bottomEndPos;
    }
}
