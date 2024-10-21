using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering; // For Volume (Global Volume)

public class GlobalVolumeBloom : MonoBehaviour
{
    public Volume globalVolume;

    void Awake()
    {
        if (globalVolume != null)
        {
            //  post-processing in Awake to to reset  before the game starts
            StartCoroutine(ResetPostProcessing());
        }
        else
        {
           
        }
    }

    private IEnumerator ResetPostProcessing()
    {
      
        globalVolume.enabled = false;   // disable post-processing

        
        yield return null;
        yield return null;

 
        globalVolume.enabled = true;        // re-enable post-processing

       // Debug.Log(global volume on");
    }
}