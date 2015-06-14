// This script controls the rotation of the Earth.  The user can rotate and spin it to view is from all sides.
// It is applied to the SkyDome

using UnityEngine;
using System.Collections;

public class EarthSpin : MonoBehaviour 
{
	//Initialization
	public float flickSpeed;
	public GameObject mainCamera;
	public Vector3 offClickRotate;
	public Quaternion rotation;

	public GameObject Earth;
	CameraRay cameraRay;

	void Start()
	{
		cameraRay = mainCamera.GetComponent<CameraRay>();
	}

	void Update () 
	{
		//Syncs Earth's rotation with the Sky Dome's
		rotation = transform.rotation;
		Earth.transform.rotation = rotation;

		//Click and Drag Distances for planet rotation
		Vector3 spin = new Vector3 (0, cameraRay.earthRotation.y, 0);
		Vector3 lift = new Vector3 (cameraRay.earthRotation.x, 0, 0);

		//The decreasing velocity after the release of the mouse
		offClickRotate = Vector3.Lerp(offClickRotate, Vector3.zero, .01f);

		//Change in location of the Mouse's onscreen position per frame
		Vector3 changeMouse = new Vector3 (cameraRay.changeMouse.y, cameraRay.changeMouse.x * -1, 0);

		//Click and Drag rotation
		if (cameraRay.earthClick == true)
		{
			transform.Rotate (spin * Time.deltaTime, Space.World);
			transform.Rotate (lift * Time.deltaTime, Space.World);
		}

		//Rotation with no mouse clicks
		else
		{
			transform.Rotate (offClickRotate, Space.World);
			transform.Rotate (0,1 * Time.deltaTime,0,Space.Self);//This is the constant spin of the Earth
		}

		//On release this records the initial rotational force
		if (Input.GetKeyUp (KeyCode.Mouse0) == true)
		{
			offClickRotate = changeMouse * Time.deltaTime * flickSpeed * 2;
		}
	}
}