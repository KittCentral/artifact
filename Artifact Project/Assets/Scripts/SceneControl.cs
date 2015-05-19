using UnityEngine;
using System.Collections;

public class SceneControl : MonoBehaviour 
{
	public string[] Scenes =  new string[5];
	public int targetScene;

	void Start () 
	{
		Scenes[0] = "RSS Scene";
		Scenes[1] = "Rolling";
		Scenes[2] = "Calendar";
		Scenes[3] = "GPS Scene";
		Scenes[4] = "Campus Map";
	}

	public void OpenScene(int number)
	{
		targetScene = number;
		PlayerPrefs.SetInt("Target",targetScene);
		UnityEngine.Application.LoadLevel("Loading");
	}

	public void Test()
	{
		print ("BBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBB");
	}

	IEnumerator Wait(int number)
	{
		yield return new WaitForSeconds(3);
		UnityEngine.Application.LoadLevel(Scenes[number]);
	}

	void Update () 
	{
		if(Application.loadedLevelName == "Loading")
		{
			targetScene = PlayerPrefs.GetInt("Target");
			StartCoroutine(Wait(targetScene));
		}
	}
}
