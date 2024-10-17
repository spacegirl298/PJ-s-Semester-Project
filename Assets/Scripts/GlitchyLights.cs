using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GlitchyLights : MonoBehaviour
{
    public float glitchSpeed = 0.1f; // Speed of the glitch effect
    public Light lightBulb;
    
    public AudioSource glitchSound;

    // Start is called before the first frame update
    void Start()
    {
        
        lightBulb = GetComponent<Light>();
        
        // Start the glitch effect coroutine
        StartCoroutine(GlitchEffect());
        
        glitchSound.loop = true;     
        glitchSound.Play();  
    }

    IEnumerator GlitchEffect()
    {
        while (true) //loops goes on and on ad on as long as the bulb is there
        {
            // switches light on % off randomylyt
            if (Random.Range(0, 2) == 0)
            {
                lightBulb.enabled = true;
            }
            else
            {
                lightBulb.enabled = false;
            }

            // Wait for a random duration before glitching again
            yield return new WaitForSeconds(glitchSpeed);
        }
    } 



    /*  THIS SCRIPT IS FOR MULTIPLE BULBS
     -------------------------------------------
     public float glitchSpeed = 0.1f; 
    public Light[] lights; 

    // Start is called before the first frame update
    void Start()
    {
        // start the glitch effect coroutine for each light bulb
        foreach (Light lightBulb in lights)
        {
            StartCoroutine(GlitchEffect(lightBulb));
        }
    }

    IEnumerator GlitchEffect(Light lightBulb)
    {
        while (true)
        {
            // Randomly enable or disable the light
            lightBulb.enabled = Random.Range(0, 2) == 0;

            // Wait for the specified glitch speed before changing state again
            yield return new WaitForSeconds(glitchSpeed);
        }
    }
    */
}

