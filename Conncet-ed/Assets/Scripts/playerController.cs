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

    Events events;
    Vector2Int currentChunk;

    [SerializeField] private CharacterController controller;
    private Vector3 velocity;

    void Start()
    {
        events = FindObjectOfType<Events>();

        currentChunk = CurrentChunk();
        events.MovedChunk(CurrentChunk());
    }

    private Vector2Int CurrentChunk(){

        Vector3 playerPos = this.transform.position;
        Vector2Int playerChunk = Vector2Int.RoundToInt(new Vector2(playerPos.x, playerPos.z)) / 10;

        return playerChunk;
    }

    private void ChunkCheck(){

        Vector2Int playerChunk = CurrentChunk();
        if(playerChunk.x != currentChunk.x || playerChunk.y != currentChunk.y){

            currentChunk = playerChunk;
            events.MovedChunk(playerChunk);
        }
    }

    void Update()
    {
        Vector3 desired = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        desired = transform.TransformDirection(desired).normalized;
        desired *= speed;

        if (controller.isGrounded)
        {
            velocity.y = 0;
            if (Input.GetButton("Jump"))
            {
                velocity.y = jumpSpeed;
            }
            velocity.x = desired.x;
            velocity.z = desired.z;
        }
        else
        {
            velocity.x += desired.x * 0.01f;
            velocity.z += desired.z * 0.01f;
        }
        if(transform.position.y < -50)
        {
            velocity.y = 50;
        }
        // Gravity always applies
        velocity.y -= gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
        ChunkCheck();
    }
}
