// This is one of the most important scripts for the RSS Scene.
// It controls the actions of each button and menue described in in CameraRay.cs.
// It also loads several dll to alow this scene to launch other programs and apps.

using UnityEngine;
using System.Collections;
using System.Diagnostics;
using System;
using System.Runtime.InteropServices;

public class GUI_Control : MonoBehaviour 
{
	private int np;
	private int nb;
	private float targetAlpha;
	private float alpha;
	private bool GUIState;
	private Vector3 targetBGSize;

	public float planetSize;
	public bool planetButton;
	public bool GUIHover;
	public bool weatherBackground;
	private bool zoomCheck;

	public GameObject earth;
	public GameObject background;
	public GameObject BGSphere;
	public GameObject mainCamera;
	public GameObject RSS;
	public GameObject News;
	public GameObject[] ObjectArray = new GameObject[5];

	OurRSS ourRSS;
	CameraRay cameraRay;
	
	public Material[] earthTextures = new Material[3];
	public Material[] bgTextures = new Material[7];

	private const int windowNormal = 1;
	private const int windowMinimized = 2;
	private const int windowMaximized = 3;

	[DllImport("user32.dll")]
	private static extern bool ShowWindowAsync(IntPtr hWnd, int nCmdShow);

	[DllImport("user32.dll", SetLastError = true)]
	static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
	
	void Start()
	{
		ObjectArray[4].SetActive(false);
		ObjectArray[5].SetActive(false);
		cameraRay = mainCamera.GetComponent<CameraRay>();
		ourRSS = RSS.GetComponent<OurRSS>();
		nb = 14;
		np = 0;
		GUIState = true;
		GUIHover = false;
		earthCrust = earth.GetComponent<Renderer> ();
		planetSize = 1;
		background.GetComponent<Renderer>().material = bgTextures[nb];
		BGSphere.GetComponent<Renderer>().material = bgTextures[nb];
	}

	public void RotationReset()
	{
			earth.transform.rotation = new Quaternion (0, 0, 0, 0);
			BGSphere.transform.rotation = new Quaternion (0, 0, 0, 0);
	}

	public void EnterHover()
	{
		GUIHover = true;
	}

	public void ExitHover()
	{
		GUIHover = false;
	}

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
		StartCoroutine(Wait());
	}

	public void ObjectControl(int number)
	{
		ObjectArray[number].SetActive(!ObjectArray[number].activeSelf);
	}

	public void MinimizeWindow()
	{
		IntPtr hWnd = FindWindow("Notepad", "Untitled - Notepad");
		if (!hWnd.Equals(IntPtr.Zero))
		{
			ShowWindowAsync(hWnd, windowMinimized);
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
	
	IEnumerator Wait()
	{
		yield return new WaitForSeconds (1);
		planetButton = false;
	}

	IEnumerator WaitforHover()
	{
		yield return new WaitForSeconds (1);
		GUIHover = false;
	}

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
		alpha = Mathf.Lerp (alpha, targetAlpha, Time.deltaTime*10);
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
		if(nb != 14)
		{
			background.SetActive (true);
		}
		else
		{
			background.SetActive (false);
		}
		BGSphere.transform.localScale =new Vector3(mainCamera.GetComponent<Camera>().orthographicSize*3.5f,mainCamera.GetComponent<Camera>().orthographicSize*3.5f,mainCamera.GetComponent<Camera>().orthographicSize*3.5f);
		background.transform.localScale =new Vector3(mainCamera.GetComponent<Camera>().orthographicSize*0.3f,mainCamera.GetComponent<Camera>().orthographicSize*0.25f,mainCamera.GetComponent<Camera>().orthographicSize*0.25f);
		if(np==1){planetSize = .009f;}
		else if(np==2){planetSize = 2.61f;}
		else if(np==3){planetSize = 1.05f;}
		else if(np==5){planetSize = 1.88f;}
		else if(np==6){planetSize = 0.09f;}
		else if(np==7){planetSize = 0.11f;}
		else if(np==8){planetSize = 0.25f;}
		else if(np==9){planetSize = 0.26f;}
		else if(np==10){planetSize = 5.35f;}
		else if(np==11){planetSize = 3.07f;}
		else {planetSize = 1;}
		zoomCheck = cameraRay.earthZoom;
	}
}
