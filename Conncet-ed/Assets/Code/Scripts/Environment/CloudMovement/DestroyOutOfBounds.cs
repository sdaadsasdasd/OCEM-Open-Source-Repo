using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOutOfBounds : MonoBehaviour
{
    public float topBound = 35.0f;     // Upper limit (after which object is out of screen and needs to be deleted)
    public float lowerBound = -15.0f;  // Lower limit (after which object is out of screen and needs to be deleted)

    void Update()
    {
        // Detect if an object has gone out of bounds to automagically delete it
        if (transform.position.z > topBound)
        {
            Destroy(gameObject);
        }
        else if (transform.position.z < lowerBound)
        {
            Destroy(gameObject);    // If cloud crosses this point, it's destroyed
        }
    }
}