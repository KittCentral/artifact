// This script goverens the animation of the weather icons that are described in WeatherIcons.cs

using UnityEngine;
using System.Collections;

public class WeatherPanel : MonoBehaviour 
{
	public GameObject cameraPrime;
	CameraRay cameraray;
	
	public bool earthZoom;
	public bool hover;
	Animator anim;

	void Start () 
	{
        cameraPrime = GameObject.Find("Primary Camera");
        cameraray = cameraPrime.GetComponent<CameraRay> ();
		anim = GetComponent<Animator> ();
	}

	public void HoverControl(bool onoff)
	{
		hover = onoff;
		anim.SetBool ("Weather Hover", hover);
	}
}