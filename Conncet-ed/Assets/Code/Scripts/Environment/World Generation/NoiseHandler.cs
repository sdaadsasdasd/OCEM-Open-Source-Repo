using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseHandler
{

    public float Evaluate(Vector3 point, float scale, float min, float max, Noise noise){

        float height = (min + max) / 2f;
        float amplitude = max - height; 
        float noiseValue = (noise.Evaluate(point * scale) * amplitude) + height;
        return noiseValue;
    }
}
