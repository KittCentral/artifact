using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Zoomer : MonoBehaviour {
	public Slider slider;
	public Camera mainCamera;
	private float oldSliderValue;
	private float newSliderValue;
	// Use this for initialization
	void Start () {
		oldSliderValue = slider.value;

	}

	// Update is called once per frame
	void Update () {
		newSliderValue = slider.value;
		if (oldSliderValue < newSliderValue) {
			zoomIn ();
			print ("in");
			oldSliderValue = newSliderValue;
		} else if (oldSliderValue > newSliderValue) {
			zoomOut ();
			print ("out");
			oldSliderValue = newSliderValue;
		}


	}
	public void zoomIn()
	{
		Vector3 newPos = Camera.main.transform.position + new Vector3(Camera.main.transform.position.x,mainCamera.transform.position.y,-1.0f);
		Camera.main.transform.position = newPos;
		print (newPos);
	}
	public void zoomOut()
	{
		Camera.main.transform.position = Camera.main.transform.position + new Vector3(-Camera.main.transform.position.x / 2.0f,-Camera.main.transform.position.y / 2f,1.0f);
		print ("out2");
	}
}
