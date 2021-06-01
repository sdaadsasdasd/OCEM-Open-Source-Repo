using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMovement : MonoBehaviour
{
    float xInput;               // Player a/d movement
    float zInput;               // Player w/s movement
    public float speed;         // Speed multiplier to default values
    public Rigidbody rb;

    void Update()
    {
        /*// Delta time ensures that player doesn't move faster at higher framerates

        // Move the player left-to-right
        xInput = Input.GetAxis("Horizontal");
        transform.Translate(Vector3.right * xInput * Time.deltaTime * speed);

        // Move the player forward or backwards
        zInput = Input.GetAxis("Vertical");
        transform.Translate(Vector3.forward * zInput * Time.deltaTime * speed);*/

        xInput = Input.GetAxis("Horizontal");
        zInput = Input.GetAxis("Vertical");
        rb = GetComponent<Rigidbody>();
        rb.AddForce(xInput * speed, 0, zInput * speed, ForceMode.Impulse);
    }
}
