// This script is used to check where the mouse is hovering and respond accordingly

using UnityEngine;

public class CameraRay : MonoBehaviour 
{
	//Initialization
	float targetWidth = 1;
	Vector3 oldMouse = Vector3.zero;
	public Vector3 changeMouse;
	public float rotationSpeed;
	public Vector3 earthRotation;
	public bool earthClick;
	public bool earthZoom;
	Ray ray;

	void Update () 
	{
		//Creates the ray to check where the mouse hits. If we don't want to check hits it points backwards
		ray = GetComponent<Camera>().ScreenPointToRay (new Vector3 (Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z));
		Debug.DrawRay (ray.origin, ray.direction * 10, Color.yellow);//Makes the ray show up in Scene View

		//Records a change in the mouses position for click and drag
		if (oldMouse != Input.mousePosition && earthClick == true) 
		{
			changeMouse = Input.mousePosition - oldMouse;
			earthRotation = new Vector3 (changeMouse.x * rotationSpeed, changeMouse.y * rotationSpeed * -1, 0);
		}
		else
		{
			earthRotation = Vector3.zero;
			changeMouse = Vector3.zero;
		}

		//Checking if you hit something on click
		if (Physics.Raycast (ray, 2) == true && Input.GetKeyDown (KeyCode.Mouse0) == true) 
			earthClick = true;

		//Checking if you are releasing the click and drag
		if (Input.GetKeyUp (KeyCode.Mouse0) == true)
			earthClick = false;

		oldMouse = Input.mousePosition;//Records the mouse position for the next frame
	}
}