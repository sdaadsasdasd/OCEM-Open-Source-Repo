using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class followPlayer : MonoBehaviour
{
    public GameObject player;       // Variable to store our player
    private Vector3 cameraOffset;   // Position difference between player and camera
    void Start()
    {
        cameraOffset = player.transform.position - this.transform.position; // Offset defined once whenever we run the game
    }
    void Update()
    {
        this.transform.position = player.transform.position - cameraOffset; // Previously defined offset used to constantly update
    }
}
