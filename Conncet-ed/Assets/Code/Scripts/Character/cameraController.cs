using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraController : MonoBehaviour
{
	// Defining upper and lower limits for camera rotation in X axis
	public float minX = -60f;
	public float maxX = 60f;

	public float sensitivity;
	public Camera cam;				// Camera to move it up and down
	public GameObject playerBody;	// playerBody to turn the player along with mouse instead of just camera

	private float rotY = 0f;
	private float rotX = 0f;

	// Variables to store the rotations of player and camera at the point when we freeze the location and rotation (LocRot)
	// We use these variables to keep the camera looking right where it was looking before the LocRot is frozen
	private float prevRotX;
	private float prevRotY;

	void Start()
	{
		// Lock cursor to center of  view and also turns it's visibility off so we don't see it when we are translating/rotating our player/cam
		Cursor.lockState = CursorLockMode.Locked;	
		Cursor.visible = false;
	}

	void Update()
	{
		rotY += Input.GetAxis("Mouse X") * sensitivity;
		rotX += Input.GetAxis("Mouse Y") * sensitivity;

		rotX = Mathf.Clamp(rotX, minX, maxX);				// We want to 'clamp' rotX between 60 and -60 degrees of rotation
		
		if (Input.GetKeyDown(KeyCode.Escape))				// Press Esc to freeze LocRot and save rotation angles of player/cam
		{
			Cursor.lockState = CursorLockMode.None;			// Cursor is freed and can move around
			Cursor.visible = true;							// Cursor is seen
			prevRotX = rotX;
			prevRotY = rotY;
		}

		if (Cursor.visible)									// We want rotX, rotY to stay in the stored positions
        {
			rotX = prevRotX;
			rotY = prevRotY;
		}

		if (Cursor.visible && Input.GetMouseButtonDown(1))	// Un-freeze the game
		{
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
		}


		cam.transform.localEulerAngles = new Vector3(-rotX, 0, 0);          // Turns cam up and down (x-axis rotation)
		playerBody.transform.localEulerAngles = new Vector3(0, rotY, 0);	// Turns player right and left (y-axis rotation)
	}
}