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
        // Disable post-processing
        globalVolume.enabled = false;

        
        yield return null;
        yield return null;

        // Re-enable post-processing
        globalVolume.enabled = true;

        Debug.Log("Global volume on");
    }
}