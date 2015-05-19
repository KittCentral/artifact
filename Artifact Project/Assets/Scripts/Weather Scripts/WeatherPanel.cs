// This script goverens the animation of the weather icons that are described in WeatherIcons.cs

using UnityEngine;
using System.Collections;

public class WeatherPanel : MonoBehaviour 
{
	public GameObject Camera;
	CameraRay cameraray;
	
	public bool earthZoom;
	public bool hover;
	Animator anim;

	void Start () 
	{
		cameraray = Camera.GetComponent<CameraRay> ();
		anim = GetComponent<Animator> ();
	}

	public void HoverControl(bool onoff)
	{
		hover = onoff;
		anim.SetBool ("Weather Hover", hover);
	}

	void Update () 
	{
		earthZoom = cameraray.earthZoom;
		anim.SetBool ("Earth Zoom", earthZoom);
	}
}