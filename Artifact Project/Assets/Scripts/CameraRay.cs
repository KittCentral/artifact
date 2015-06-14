// This script is used to check where the mouse is hovering and respond accordingly

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

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
	public GameObject GUIControl;
	GUI_Control guiThing;
	Ray ray;

	public GameObject[] ButtonObjects = new GameObject[6];
	Button[] buttons = new Button[6];

	void Start()
	{
		guiThing = GUIControl.GetComponent<GUI_Control> ();
		for(int i = 0; i < buttons.Length; i++)
		{
			buttons[i] = ButtonObjects[i].GetComponent<Button>();
		}
	}

	//Controls the zoom in and out functionality
	IEnumerator EarthZoomControl (bool up)
	{
		//Zoom in
		if (up == false)
		{
			earthZoom = true;
			targetWidth = .5f;
			ButtonEnabler(false);
		}
		//Zoom out
		if (up == true)
		{
			yield return new WaitForSeconds(1);
			earthZoom = false;
			changeMouse = Vector3.zero;
			targetWidth = guiThing.planetSize;
			ButtonEnabler(true);
		}
	}

	//Controls whether the buttons are active or not
	void ButtonEnabler(bool onoff)
	{
		for(int i = 0; i < buttons.Length; i++)
		{
			buttons[i].enabled = onoff;
		}
	}

	void Update () 
	{
		//Creates the ray to check where the mouse hits. If we don't want to check hits it points backwards
		if(guiThing.GUIHover == false)
		{
			ray = GetComponent<Camera>().ScreenPointToRay (new Vector3 (Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z));
		}
		else
		{
			ray = new Ray(Vector3.zero,Vector3.back);
		}
		Debug.DrawRay (ray.origin, ray.direction * 10, Color.yellow);//Makes the ray show up in Scene View

		//Records a change in the mouses position for click and drag
		if (oldMouse != Input.mousePosition && earthClick == true) 
		{
			changeMouse = Input.mousePosition - oldMouse;
			earthRotation = new Vector3 (changeMouse.y * rotationSpeed, changeMouse.x * rotationSpeed * -1, 0);
		}
		else
		{
			earthRotation = Vector3.zero;
			changeMouse = Vector3.zero;
		}

		//Checking if you hit something on click
		if (Physics.Raycast (ray, 2) == true && Input.GetKeyDown (KeyCode.Mouse0) == true) 
		{
			earthClick = true;
			StopAllCoroutines();
			StartCoroutine(EarthZoomControl(false));
		}

		//Checking if you are releasing the click and drag
		if (Input.GetKeyUp (KeyCode.Mouse0) == true)
		{
			earthClick = false;
			StartCoroutine(EarthZoomControl(true));
		}

		//Zooms the camera in and out
		if(guiThing.planetButton == false)
		{
			GetComponent<Camera>().orthographicSize = Mathf.Lerp (GetComponent<Camera>().orthographicSize, targetWidth, Time.deltaTime * 3);
		}
		else
		{
			GetComponent<Camera>().orthographicSize = guiThing.planetSize;
		}
		oldMouse = Input.mousePosition;//Records the mouse position for the next frame
	}
}