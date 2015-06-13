// This script is applied to the main RSS Scene buttons to and lets the animator know
// when you have zoomed in on the planet

using UnityEngine;
using System.Collections;

public class InfoFromCanvas : MonoBehaviour 
{
	public GameObject Camera;
	CameraRay cameraray;

	public bool earthZoom;
	Animator anim;

	void Start () 
	{
		cameraray = Camera.GetComponent<CameraRay> ();
		anim = GetComponent<Animator> ();
	}

	void Update () 
	{
		//Get a Boolean for whether you have zoomed in or not
		earthZoom = cameraray.earthZoom;
		//Send it to the Animator
		anim.SetBool ("Earth Zoom", earthZoom);
	}
}
