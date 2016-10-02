using UnityEngine;

public class UIControl : MonoBehaviour
{
    public void SceneAdd(int i)
    {
        SceneControl.OpenSceneAdditive(i);
    }

    public void SceneOpen(int i)
    {
        SceneControl.OpenScene(i);
    }
}
