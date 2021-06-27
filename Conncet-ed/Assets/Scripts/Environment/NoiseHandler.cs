using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseHandler
{
    Noise noise = new Noise();

    public float Evaluate(Vector3 point, float scale, float amplitude, float height){

        float noiseValue = (noise.Evaluate(point * scale) * amplitude) + height;
        return noiseValue;
    }
}
