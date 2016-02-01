using UnityEngine;

public class ScreenControl : MonoBehaviour
{
    public RenderTexture tarTex;
    public Material screenMat;
    
    public void DisplayScene()
    {
        GameObject newCamera = GameObject.Find("Main Camera");
        AudioListener listener = newCamera.GetComponent<AudioListener>();
        listener.enabled = false;
        Camera cameraNew = newCamera.GetComponent<Camera>();
        cameraNew.targetTexture = tarTex;
        MeshRenderer rend = gameObject.GetComponent<MeshRenderer>();
        rend.material = screenMat;
    }
}
