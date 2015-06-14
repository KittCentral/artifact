//This script allows us to switch between scenes with the loading screen as an intermediate point

using UnityEngine;
using System.Collections;

public class SceneControl : MonoBehaviour 
{
	//Initialize Scene Names
	public int targetScene;
	string[] Scenes = {"RSS Scene", "Loading", "Calendar", "Campus Map", "GPS Scene"};
	
	//Checks if you are in the Loading Scene then opens the approriate Scene
	void Update () 
	{
		if(Application.loadedLevelName == "Loading")
		{
			targetScene = PlayerPrefs.GetInt("Target");
			StartCoroutine(Wait(targetScene));
		}
	}

	//Opens Loading and Puts the target in a place which doesn't change when the scene does
	public void OpenScene(int number)
	{
		targetScene = number;
		PlayerPrefs.SetInt("Target",targetScene);
		UnityEngine.Application.LoadLevel("Loading");
	}

	//Lets the Loading run for a while before opening the next Scene
	IEnumerator Wait(int number)
	{
		yield return new WaitForSeconds(3);
		UnityEngine.Application.LoadLevel(Scenes[number]);
	}
}
