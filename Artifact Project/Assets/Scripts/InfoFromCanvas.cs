// This script is applied to the main RSS Scene, and it initalizes a few game objectts that 
// are used in other scripts.

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
		earthZoom = cameraray.earthZoom;
		anim.SetBool ("Earth Zoom", earthZoom);
	}
}
