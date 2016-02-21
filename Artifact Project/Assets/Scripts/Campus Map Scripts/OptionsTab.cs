using UnityEngine;
using System.Collections;

public class OptionsTab : MonoBehaviour {

	public GameObject panel;
	public Camera mainCamera;

	private RectTransform panelTransform;
	private Transform cameraTransform;
	private bool following = false;
	private Vector3 OriginalCameraPosition;
	// Use this for initialization
	void Start () {

		panelTransform = panel.GetComponent<RectTransform> ();
		cameraTransform = mainCamera.GetComponent<Transform> ();
		
	}

	public void ClickOn()
	{
		Vector3 cameraPosition = cameraTransform.position;
		Vector3 mouse = Input.mousePosition;
		if (mouse.x <= Screen.width*.3f && mouse.x >= 0.0f) 
		{
			panelTransform.offsetMax = new Vector2 (mouse.x, 0.0f);
			panelTransform.offsetMin = new Vector2 (mouse.x, 0.0f);
		}
		mainCamera.transform.position = cameraPosition;
	}
		
}
