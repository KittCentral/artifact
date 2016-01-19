/// <summary>
/// CodeArtist.mx 2015
/// This is the main class of the project, its in charge of raycasting to a model and place brush prefabs infront of the canvas camera.
/// If you are interested in saving the painted texture you can use the method at the end and should save it to a file.
/// </summary>


using UnityEngine;

public enum Painter_BrushMode { PAINT, DECAL };
public static class TexturePainter
{

    #region "Other Instantiations"
    //Painter_BrushMode mode; //Our painter mode (Paint brushes or decals)
    const float brushSize = 1.0f; //The size of our brush
    static Color brushColor = new Color(1f, .5f, .5f); //The selected color
    public static int brushCounter = 0;
    public static bool saving = false; //Flag to check if we are saving the texture
    #endregion

    #region "Methods"
    

    /// <summary>
    /// The main action, instantiates a brush or decal entity at the clicked position on the UV map
    /// </summary>
    public static void AddPaint(GameObject brush, GameObject container, GameObject camera)
    {
        if (saving)
            return;
        Vector3 worldPos = Vector3.zero;
        if (HitTestCursor(ref worldPos, camera.GetComponent<Camera>()))
        {
            GameObject brushObj;
            brushObj = (GameObject)UnityEngine.Object.Instantiate(brush); //Paint a brush
            brushObj.GetComponent<SpriteRenderer>().color = brushColor; //Set the brush color
            brushColor.a = brushSize * 2.0f; // Brushes have alpha to have a merging effect when painted over.
            brushObj.transform.parent = container.transform; //Add the brush to our container to be wiped later
            brushObj.transform.localPosition = worldPos; //The position of the brush (in the UVMap)
            brushObj.transform.localScale = Vector3.one * brushSize;//The size of the brush
            brushObj.transform.eulerAngles = new Vector3(90, 0, 0);
        }
    }

    /// <summary>
    /// The main action, instantiates a brush or decal entity at the clicked position on the UV map
    /// </summary>
    public static void AddPaintPoint(Vector3 endPos, GameObject brush, GameObject container, GameObject camera, Color color, float size)
    {
        if (saving)
            return;
        Vector3 worldPos = Vector3.zero;
        if (HitTestPoint(ref worldPos, endPos, camera))
        {
            GameObject brushObj;
            brushObj = Object.Instantiate(brush); //Paint a brush
            brushObj.GetComponent<SpriteRenderer>().color = color; //Set the brush color
            brushColor.a = brushSize * 2.0f; // Brushes have alpha to have a merging effect when painted over.
            brushObj.transform.parent = container.transform; //Add the brush to our container to be wiped later
            brushObj.transform.localPosition = worldPos; //The position of the brush (in the UVMap)
            brushObj.transform.localScale = Vector3.zero;
            brushObj.GetComponent<Splatter>().maxSize = size;
            brushObj.transform.eulerAngles = new Vector3(90, 0, Random.Range(0,359));
        }
    }

    /// <summary>
    /// To update at realtime the painting cursor on the mesh
    /// </summary>
    public static void UpdateBrushCursor(Camera camera, GameObject cursor, GameObject container)
    {
        Vector3 worldPos = Vector3.zero;
        if (HitTestCursor(ref worldPos, camera) && !saving)
        {
            cursor.SetActive(true);
            cursor.transform.position = worldPos + container.transform.position;
        }
        else
            cursor.SetActive(false);
    }

    /// <summary>
    /// Returns the position on the texturemap according to a hit in the mesh collider
    /// </summary>
    /// <param name="worldPos">Position of the Hit</param>
    /// <returns>Boolean for whether the ray hit</returns>
    public static bool HitTestCursor(ref Vector3 worldPos, Camera camera)
    {
        RaycastHit hit;
        Vector3 cursorPos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 1.0f);
        Ray cursorRay = camera.ScreenPointToRay(cursorPos);
        if (Physics.Raycast(cursorRay, out hit, 200))
        {
            MeshCollider meshCollider = hit.collider as MeshCollider;
            if (meshCollider == null || meshCollider.sharedMesh == null)
                return false;
            worldPos.x = hit.point.x;
            worldPos.y = hit.point.y;
            worldPos.z = hit.point.z;
            return true;
        }
        else
            return false;
    }

    /// <summary>
    /// Returns the position on the texturemap according to a hit in the mesh collider
    /// </summary>
    /// <param name="worldPos">Position of the Hit</param>
    /// <returns>Boolean for whether the ray hit</returns>
    public static bool HitTestPoint(ref Vector3 worldPos, Vector3 endPos, GameObject camera)
    {
        RaycastHit hit;
        int layerMask = 1 << 1;
        layerMask = ~layerMask;
        if (Physics.Raycast(camera.transform.position,endPos-camera.transform.position, out hit, 20, layerMask))
        {
            MeshCollider meshCollider = hit.collider as MeshCollider;
            if (meshCollider == null || meshCollider.sharedMesh == null)
                return false;
            worldPos.x = hit.point.x;
            worldPos.y = hit.point.y;
            worldPos.z = hit.point.z;
            return true;
        }
        else
            return false;
    }

    /// <summary>
    /// Sets the base material with a our canvas texture, then removes all our brushes
    /// </summary>
    public static void SaveTexture(RenderTexture canvasTexture, Material baseMaterial, GameObject container)
    {
        brushCounter = 0;
        RenderTexture.active = canvasTexture;
        Texture2D tex = new Texture2D(canvasTexture.width, canvasTexture.height, TextureFormat.RGB24, false);
        tex.ReadPixels(new Rect(0, 0, canvasTexture.width, canvasTexture.height), 0, 0);
        tex.Apply();
        RenderTexture.active = null;
        baseMaterial.mainTexture = tex;
        foreach (Transform child in container.transform)
            UnityEngine.Object.Destroy(child.gameObject);
        //StartCoroutine ("SaveTextureToFile"); //Do you want to save the texture? This is your method!
    }

    /// <summary>
    /// Show again the user cursor (To avoid saving it to the texture)
    /// </summary>
    public static void ShowCursor()
    {
        saving = false;
    }
    #endregion
    
    #region "Optional Methods"
    /*
#if !UNITY_WEBPLAYER
    IEnumerator SaveTextureToFile(Texture2D savedTexture)
    {
        brushCounter = 0;
        string fullPath = System.IO.Directory.GetCurrentDirectory() + "\\UserCanvas\\";
        string fileName = "CanvasTexture.png";
        if (!System.IO.Directory.Exists(fullPath))
            System.IO.Directory.CreateDirectory(fullPath);
        var bytes = savedTexture.EncodeToPNG();
        System.IO.File.WriteAllBytes(fullPath + fileName, bytes);
        Debug.Log("<color=orange>Saved Successfully!</color>" + fullPath + fileName);
        yield return null;
    }
#endif*/
    #endregion
}