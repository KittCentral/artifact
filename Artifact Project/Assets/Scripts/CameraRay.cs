// This script places all 6 of the primary buttons along the left side of the screen.
// The actions of these buttons are described in GUI_Control.cs.

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CameraRay : MonoBehaviour 
{
	private float targetWidth;
	public Vector3 changeMouse;
	private Vector3 oldMouse;
	public float rotationSpeed;
	public Vector3 earthRotation;
	public bool earthClick;
	public bool earthZoom;
	public GameObject GUIControl;
	GUI_Control guiThing;

	public GameObject ButtonOne;
	public GameObject ButtonTwo;
	public GameObject ButtonThree;
	public GameObject ButtonFour;
	public GameObject ButtonFive;
	public GameObject ButtonSix;

	Button b1;
	Button b2;
	Button b3;
	Button b4;
	Button b5;
	Button b6;

	void Start()
	{
		b1 = ButtonOne.GetComponent<Button>();
		b2 = ButtonTwo.GetComponent<Button>();
		b3 = ButtonThree.GetComponent<Button>();
		b4 = ButtonFour.GetComponent<Button>();
		b5 = ButtonFive.GetComponent<Button>();
		b6 = ButtonSix.GetComponent<Button>();
		oldMouse = new Vector3 (0, 0, 0);
		targetWidth = 1;
		guiThing = GUIControl.GetComponent<GUI_Control> ();
	}

	IEnumerator EarthZoomControl (bool up)
	{
		if (up == false)
		{
			earthZoom = true;
			targetWidth = .5f;
			ButtonEnabler(false);
		}
		if (up == true)
		{
			yield return new WaitForSeconds(1);
			earthZoom = false;
			changeMouse = new Vector3 (0,0,0);
			targetWidth = guiThing.planetSize;
			ButtonEnabler(true);
		}
	}

	void ButtonEnabler(bool onoff)
	{
		b1.enabled = onoff;
		b2.enabled = onoff;
		b3.enabled = onoff;
		b4.enabled = onoff;
		b5.enabled = onoff;
		b6.enabled = onoff;
	}

	void Update () 
	{
		Ray ray;
		if(guiThing.GUIHover == false)
		{
			ray = GetComponent<Camera>().ScreenPointToRay (new Vector3 (Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z));
		}
		else
		{
			ray = new Ray(Vector3.zero,Vector3.back);
		}
		Debug.DrawRay (ray.origin, ray.direction * 10, Color.yellow);
		if (oldMouse != Input.mousePosition && earthClick == true) 
		{
			changeMouse = Input.mousePosition - oldMouse;
			earthRotation = new Vector3 (changeMouse.y * rotationSpeed, changeMouse.x * rotationSpeed * -1, 0);
		}
		else
		{
			earthRotation =  new Vector3(0,0,0);
			changeMouse = new Vector3(0,0,0);
		}
		if (Physics.Raycast (ray, 2) == true && Input.GetKeyDown (KeyCode.Mouse0) == true) 
		{
			earthClick = true;
			StopAllCoroutines();
			StartCoroutine(EarthZoomControl(false));
		}
		if (Input.GetKeyUp (KeyCode.Mouse0) == true)
		{
			earthClick = false;
			StartCoroutine(EarthZoomControl(true));
		}
		if(guiThing.planetButton == false)
		{
			GetComponent<Camera>().orthographicSize = Mathf.Lerp (GetComponent<Camera>().orthographicSize, targetWidth, Time.deltaTime * 3);
		}
		else
		{
			GetComponent<Camera>().orthographicSize = guiThing.planetSize;
		}
		oldMouse = Input.mousePosition;
	}
}
