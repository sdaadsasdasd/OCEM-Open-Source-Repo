using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOutOfBounds : MonoBehaviour
{
    public float topBound = 350.0f;     // Upper limit (after which object is out of screen and needs to be deleted)

    void Update()
    {
        checkOutOfBounds();
    }

    // Detect if an object has gone out of bounds to automagically delete it
    void checkOutOfBounds()
    {
        if (transform.position.z > topBound)
        {
            Destroy(gameObject);
        }
    }
}