﻿using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour {


	private Vector3 oldCameraPosition;
	private Vector3 dragOrigin;
	private int frameCounter = 0;
	private Rigidbody cameraBody;
	private Vector3 move = new Vector3(0,0,0);
	private bool clicked;
	// Use this for initialization
	void Start () {
		cameraBody = Camera.main.GetComponent<Rigidbody> ();
	}

	// Update is called once per frame
	void Update () 
	{
		if (Input.GetMouseButtonDown (0)) {//On the frame the mouse is clicked
			dragOrigin = Input.mousePosition;//mark mouse's position
			oldCameraPosition = Camera.main.transform.position;//mark the camera's position
			cameraBody.velocity = new Vector3(0,0,0);
			clicked = true;
		}
		if (Input.GetMouseButton (0)) { //while the mouse is being held down
			frameCounter++;
			Vector3 pos = Camera.main.ScreenToViewportPoint (Input.mousePosition - dragOrigin);
			move = new Vector3 (-pos.x*3f, -pos.y*3f, 0);
			Camera.main.transform.position = oldCameraPosition + move;
		}
		if (Input.GetMouseButtonUp (0) && clicked == true) //mouse was unclicked and had been clicked before
		{
			if (frameCounter < 20) {
				Vector3 force = move*70f;
				cameraBody.AddForce (force);
			}
			clicked = false;
			frameCounter = 0;
		}

	}
}

