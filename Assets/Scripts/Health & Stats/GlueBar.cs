using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GlueBar : MonoBehaviour
{
    public Slider glueSlider;

    public void SetSliderMax(float maxGlueTime)
    {
        glueSlider.maxValue = maxGlueTime;
        glueSlider.value = maxGlueTime;
    }

    public void SetSlider(float glueTime)
    {
        glueSlider.value = glueTime;
    }
}