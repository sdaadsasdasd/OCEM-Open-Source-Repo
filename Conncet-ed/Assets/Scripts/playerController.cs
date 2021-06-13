using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour
{
    public float speed = 6.0F;
    public float jumpSpeed = 8.0F;
    public float gravity = 20.0F;

    CollisionFlags x;

    private Vector3 moveDirection;
    void Update()
    {
        CharacterController controller = GetComponent<CharacterController>();                           // We use this to control our player
        if (controller.isGrounded)                                                                      // Don't wanna move around/jump again in air
        {
            moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            moveDirection = transform.TransformDirection(moveDirection);
            moveDirection *= speed;
            if (Input.GetButton("Jump"))
            {
                moveDirection.y = jumpSpeed;
            }
        }
        moveDirection.y -= gravity * Time.deltaTime;                                                    // Gravity always applies
        x = controller.Move(moveDirection * Time.deltaTime);
        Debug.Log(x);
    }
}
