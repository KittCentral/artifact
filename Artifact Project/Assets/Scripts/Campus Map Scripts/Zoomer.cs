using UnityEngine;
using System.Collections;

public class Zoomer : MonoBehaviour {

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

	}
	public void zoomIn()
	{
		Camera.main.transform.position = Camera.main.transform.position + new Vector3(Camera.main.transform.position.x,Camera.main.transform.position.y,-1);
	}
	public void zoomOut()
	{
		Camera.main.transform.position = Camera.main.transform.position + new Vector3(-Camera.main.transform.position.x / 2.0f,-Camera.main.transform.position.y / 2f,1);;
	}
}
