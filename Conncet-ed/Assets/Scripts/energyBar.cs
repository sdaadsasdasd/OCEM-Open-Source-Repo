using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class energyBar : MonoBehaviour
{
    public Slider slider;
    public bool noEnergy = false;

    public void setMaxEnergy(float energy)
    {
        slider.maxValue = energy;
        slider.value = energy;
    }

    public void setEnergy(float energy)
    {
        slider.value = energy;
        if (energy <= 0)
            noEnergy = true;
    }
}

