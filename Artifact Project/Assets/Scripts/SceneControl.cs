//This script allows us to switch between scenes with the loading screen as an intermediate point

using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SceneControl : MonoBehaviour
{
	//Initialize Scene Names
	public int targetScene;
	string[] Scenes = {"RSS Scene", "Loading", "Calendar", "Campus Map", "GPS Scene"};
    AsyncOperation async;
    bool loading = false;
    public GameObject screen;
	
    void Start ()
    {
        OpenSceneAdditive(2);
    }

	//Checks if you are in the Loading Scene then opens the approriate Scene
	void Update () 
	{
		if(SceneManager.GetActiveScene().name == "Loading")
		{
			targetScene = PlayerPrefs.GetInt("Target");
			StartCoroutine(Wait(targetScene));
		}
        if (async != null)
        {
            if (async.isDone && loading == true)
            {
                print(loading);
                loading = false;
                ScreenControl screenScript = screen.GetComponent<ScreenControl>();
                screenScript.DisplayScene();
            }
        }
    }

	//Opens Loading and Puts the target in a place which doesn't change when the scene does
	public void OpenScene(int number)
	{
		targetScene = number;
		PlayerPrefs.SetInt("Target",targetScene);
		SceneManager.LoadScene("Loading");
	}

	//Lets the Loading run for a while before opening the next Scene
	IEnumerator Wait(int number)
	{
		yield return new WaitForSeconds(3);
        SceneManager.LoadScene(Scenes[number]);
	}

    public void OpenSceneAdditive(int number)
    {
        print("Yeah");
        async = SceneManager.LoadSceneAsync(number, LoadSceneMode.Additive);
        loading = true;
    }
}
