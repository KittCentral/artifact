using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Zoomer : MonoBehaviour {
	public Camera mainCamera;
	private float oldSliderValue;
	private float newSliderValue;
	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

	}
	public void zoomIn()
	{
		Vector3 newPos = Camera.main.transform.position + new Vector3(Camera.main.transform.position.x,mainCamera.transform.position.y,-1.0f);
		Camera.main.transform.position = newPos;
	}
	public void zoomOut()
	{
		Vector3 newPos = Camera.main.transform.position + new Vector3(-Camera.main.transform.position.x / 2.0f,-Camera.main.transform.position.y / 2f,1.0f);
		Camera.main.transform.position = newPos;
		print ("zoom out");
	}
}
