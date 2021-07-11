using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseHandler
{

    public float Evaluate(Vector3 point, NoiseSetting settings){

        float height = (settings.min + settings.max) / 2f;
        float amplitude = settings.max - height; 
        float noiseValue = (settings.noise.Evaluate(point * settings.scale) * amplitude) + height;
        return noiseValue;
    }
}
