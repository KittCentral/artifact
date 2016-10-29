//This script allows us to switch between scenes with the loading screen as an intermediate point

using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEditor;
using System.IO;

public class SceneControl : MonoBehaviour
{
	//Initialize Scene Names
	public static int targetScene;
    static AsyncOperation async;
    static bool loading = false;
    public GameObject screen;

    void Start ()
    {
        if(SceneManager.GetActiveScene().name == "RSS Scene" && SceneManager.sceneCount == 1)
            OpenSceneAdditive(10);
    }

	//Checks if you are in the Loading Scene then opens the approriate Scene
	void Update () 
	{
		if(SceneManager.GetActiveScene().name == "Loading")
		{
			targetScene = PlayerPrefs.GetInt("Target");
			StartCoroutine(Wait(targetScene));
		}
        if (async != null && screen != null)
        {
            if (async.isDone && loading == true)
            {
                loading = false;
                ScreenControl screenScript = screen.GetComponent<ScreenControl>();
                screenScript.DisplayScene();
            }
        }
    }

	//Opens Loading and Puts the target in a place which doesn't change when the scene does
	public static void OpenScene(int i)
	{
		targetScene = i;
		PlayerPrefs.SetInt("Target", targetScene);
        for (int sceneIndex = 2; sceneIndex <= SceneManager.sceneCount; sceneIndex++)
            SceneManager.UnloadScene(10);
		SceneManager.LoadScene("Loading");
	}

	//Lets the Loading run for a while before opening the next Scene
	IEnumerator Wait(int number)
	{
		yield return new WaitForSeconds(3);
        SceneManager.LoadScene(Path.GetFileNameWithoutExtension(EditorBuildSettings.scenes[number].path));
	}

    public static void OpenSceneAdditive(int i)
    {
        string tempName = "";
        bool addScene = true;
        try { tempName = SceneManager.GetSceneAt(1).name; }
        catch { addScene = false; }
        if(addScene)
            SceneManager.UnloadScene(tempName);
        async = SceneManager.LoadSceneAsync(i, LoadSceneMode.Additive);
        loading = true;
    }
}
