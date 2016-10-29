using UnityEngine;
using UnityStandardAssets.ImageEffects;

public class UIControl : MonoBehaviour
{
    GameObject primaryCamera;

    void Start()
    {
        primaryCamera = GameObject.Find("Primary Camera");
    }

    public void UIAdd(int i)
    {
        primaryCamera.GetComponent<BlurOptimized>().enabled = true;
        SceneControl.OpenSceneAdditive(i);
    }

    public void SceneAdd(int i)
    {
        primaryCamera.GetComponent<BlurOptimized>().enabled = false;
        SceneControl.OpenSceneAdditive(i);
    }

    public void SceneOpen(int i)
    {
        SceneControl.OpenScene(i);
    }
}
