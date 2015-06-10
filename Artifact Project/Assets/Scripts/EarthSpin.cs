// This script controls the rotation of the Earth.  The user can rotate and spin it to view is from all sides.

using UnityEngine;
using System.Collections;

public class EarthSpin : MonoBehaviour 
{
	public float flickSpeed;
	public GameObject mainCamera;
	public Vector3 offClickRotate;
	public Quaternion rotation;

	public GameObject Earth;

	void Start()
	{
		offClickRotate = new Vector3 (0, 0, 0);
	}

	void Update () 
	{
		rotation = transform.rotation;
		if(rotation.eulerAngles.x<90)
		{
			Earth.transform.rotation = Quaternion.Euler(new Vector3(rotation.eulerAngles.x,rotation.eulerAngles.y,rotation.eulerAngles.z));
		}
		else
		{
			Earth.transform.rotation = Quaternion.Euler(new Vector3(rotation.eulerAngles.x,rotation.eulerAngles.y,rotation.eulerAngles.z));
		}
		CameraRay cameraRay = mainCamera.GetComponent<CameraRay>();
		Vector3 spin = new Vector3 (0, cameraRay.earthRotation.y, 0);
		Vector3 lift = new Vector3 (cameraRay.earthRotation.x, 0, 0);
		Vector3 zero = new Vector3 (0,0,0);
		offClickRotate = Vector3.Lerp(offClickRotate, zero, .01f);
		Vector3 changeMouse = new Vector3 (cameraRay.changeMouse.y, cameraRay.changeMouse.x * -1, 0);
		if (cameraRay.earthClick == true)
		{
			transform.Rotate (spin * Time.deltaTime, Space.World);
			transform.Rotate (lift * Time.deltaTime, Space.World);
		}
		else
		{
			transform.Rotate (offClickRotate, Space.World);
			transform.Rotate (0,1  * Time.deltaTime,0,Space.Self);
		}
		if (Input.GetKeyUp (KeyCode.Mouse0) == true)
		{
			offClickRotate = changeMouse * Time.deltaTime * flickSpeed * 2;
		}
	}
}