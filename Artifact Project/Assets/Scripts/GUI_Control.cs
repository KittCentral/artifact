// This is one of the most important scripts for the RSS Scene.
// It controls the actions of each button and menu described in in CameraRay.cs.
// It also loads several dll to allow this scene to launch other programs and apps.

using UnityEngine;
using System.Collections;
using System;
using System.Runtime.InteropServices;

public class GUI_Control : MonoBehaviour 
{
	//Initialize control variables
	public bool planetButton;
	public bool GUIHover;
	bool zoomCheck;
	public bool weatherBackground;
	Vector3 targetBGSize;
	public float planetSize;
	bool GUIState = true;

	int np = 3;
	int nb = 14;

	float targetAlpha;
	float alpha;

	const int windowNormal = 1;
	const int windowMinimized = 2;
	const int windowMaximized = 3;

	//Initialize GameObjects and UI Object
	//public GameObject sun;
	//public GameObject earth;
	//public GameObject background;
	//public GameObject BGSphere;
	//public GameObject mainCamera;
	public GameObject RSS;
	public GameObject News;
	public GameObject[] ObjectArray = new GameObject[5];

	//Initialize Scripts
	OurRSS ourRSS;
	//CameraRay cameraRay;

	//Initialize Materials
	public Material[] earthTextures = new Material[3];
	public Material[] bgTextures = new Material[7];
	
	//Prepare the DLL functions
	[DllImport("user32.dll")]
	private static extern bool ShowWindowAsync(IntPtr hWnd, int nCmdShow);

	[DllImport("user32.dll", SetLastError = true)]
	static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
	
	void Start()
	{
		//Hide pop-up menus at startup
		ObjectArray[4].SetActive(false);
		ObjectArray[5].SetActive(false);

		//Link the Script variables
		//cameraRay = mainCamera.GetComponent<CameraRay>();
		ourRSS = RSS.GetComponent<OurRSS>();

		//Set Textures for the Backgrounds
		//background.GetComponent<Renderer>().material = bgTextures[nb];
		//BGSphere.GetComponent<Renderer>().material = bgTextures[nb];
	}
   
    /*
	//Function that recenters the rotation
	public void RotationReset()
	{
			earth.transform.rotation = new Quaternion (0, 0, 0, 0);
			BGSphere.transform.rotation = new Quaternion (0, 0, 0, 0);
	}

	//These two are called as you Hover over buttons using Event Systems
	public void EnterHover()
	{
		GUIHover = true;
	}

	public void ExitHover()
	{
		GUIHover = false;
	}

	//Used to switch planet textures but not currently used
	public void PlanetSwitch()
	{	
		earth.GetComponent<Renderer>().material = earthTextures[np];
		if (np < earthTextures.Length - 1)
		{
			np = np + 1;
		}
		else 
		{
			np = 0;
		}
		planetButton = true;
		StartCoroutine(Wait(1));
	}
    */

	//Turns on and off parts of the UI
	public void ObjectControl(int number)
	{
		ObjectArray[number].SetActive(!ObjectArray[number].activeSelf);
	}

    public void SceneAdd(int i)
    {
        SceneControl.OpenSceneAdditive(i);
    }

	//These three are tests to control external applications
	public void MinimizeWindow()
	{
		IntPtr hWnd = FindWindow("Notepad", "Untitled - Notepad");
		if (!hWnd.Equals(IntPtr.Zero))
		{
			ShowWindowAsync(hWnd, windowMinimized);
		}
	}

	public void MaximizeWindow()
	{
		IntPtr hWnd = FindWindow("Notepad", "Untitled - Notepad");
		if (!hWnd.Equals(IntPtr.Zero))
		{
			ShowWindowAsync(hWnd, windowMaximized);
		}
	}

	public void EnlargeWindow()
	{
		IntPtr hWnd = FindWindow("Notepad", "Untitled - Notepad");
		if (!hWnd.Equals(IntPtr.Zero))
		{
			ShowWindowAsync(hWnd, windowNormal);
		}
	}

	//Does something after a second
	IEnumerator Wait(int i)
	{
		yield return new WaitForSeconds (1);
		switch (i)
		{
		case 1:
			planetButton = false;
			break;
		case 2:
			GUIHover = false;
			break;
		default:
			print("Bad index for Wait function");
			break;
		}
	}

    /*
	//Toggles whether or not the Background is based on the weather or stars
	public void changeWeatherBackground()
	{
		weatherBackground = !weatherBackground;

		if(weatherBackground == true)
		{
			nb = ourRSS.nb2;
		}
		else
		{
			nb = 14;
		}
		background.GetComponent<Renderer>().material = bgTextures[nb];
		BGSphere.GetComponent<Renderer>().material = bgTextures[nb];
	}
    
	void Update () 
	{
		alpha = targetAlpha;//Mathf.Lerp (alpha, targetAlpha, Time.deltaTime*10);

		//Checks if the state of the zoom has changed and fixes things accordingly
		if (cameraRay.earthZoom != zoomCheck)
		{
			if (cameraRay.earthZoom == true)
			{
				targetAlpha = 0;
				GUIState = false;
				News.SetActive(false);
			}
			else
			{
				targetAlpha = 1;
				GUIState = true;
				News.SetActive(true);
			}
		}

		//Controls whether or not it looks like you are in space
		if(nb != 14)
		{
			background.SetActive (true);
			sun.SetActive (false);
		}
		else
		{
			background.SetActive (false);
			sun.SetActive (true);
		}

		//Makes the background proportional to the camera size
		BGSphere.transform.localScale = new Vector3(mainCamera.GetComponent<Camera>().orthographicSize*3.5f,mainCamera.GetComponent<Camera>().orthographicSize*3.5f,mainCamera.GetComponent<Camera>().orthographicSize*3.5f);
		background.transform.localScale = new Vector3(mainCamera.GetComponent<Camera>().orthographicSize*0.3f,mainCamera.GetComponent<Camera>().orthographicSize*0.25f,mainCamera.GetComponent<Camera>().orthographicSize*0.25f);
		float[] sizes = {.009f,2.61f,1.05f,1.0f,1.88f,.09f,.11f,.25f,.26f,5.35f,3.07f}; //Array of planet sizes

		//switches planet size
		if(np < sizes.Length)
		{
			planetSize = sizes [np];
		}
		else
		{
			planetSize = 1.0f;
		}
		zoomCheck = cameraRay.earthZoom; //Records what state the zoom was for the proceeding frame
	}
    */
}
