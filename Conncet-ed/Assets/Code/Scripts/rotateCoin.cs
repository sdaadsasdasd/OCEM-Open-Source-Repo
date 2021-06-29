using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotateCoin : MonoBehaviour
{
    public float rotationSpeed;
    void Start()
    {
    }
    void Update()
    {
        // Delta time ensures that coin doesn't spin faster at higher framerates
        this.transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);    
                                                                        
    }
}
