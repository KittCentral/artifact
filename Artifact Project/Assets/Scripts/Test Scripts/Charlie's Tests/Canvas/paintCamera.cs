using UnityEngine;
using System.Collections;
using Algorithms;

public class paintCamera : MonoBehaviour 
{
	public Color color;
	public Camera cam;
	Texture2D canvas;
	Vector2 lastUV;
	bool hold;

	void Start() 
	{
		cam = GetComponent<Camera>();
		canvas = new Texture2D(2000,2000, TextureFormat.ARGB32, false);
	}

	void Update() 
	{
        if (!Input.GetKey(KeyCode.Mouse0))
            hold = false;

        RaycastHit hit;
		if (!Physics.Raycast(cam.ScreenPointToRay(Input.mousePosition), out hit))
			return;
		
		Renderer rend = hit.transform.GetComponent<Renderer>();
		MeshCollider meshCollider = hit.collider as MeshCollider;
		rend.sharedMaterial.mainTexture = canvas;
		if (rend == null || rend.sharedMaterial == null || rend.sharedMaterial.mainTexture == null || meshCollider == null)
			return;
		
		Texture2D tex = rend.material.mainTexture as Texture2D;
		Vector2 pixelUV = hit.textureCoord;
        print(pixelUV);
		pixelUV.x *= tex.width;
		pixelUV.y *= tex.height;

		if(hold)
			Bresenham.Line(pixelUV, lastUV, TexturePlot, tex);

        if (Input.GetKeyUp(KeyCode.C))
        {
            Bresenham.Ellipse(pixelUV, 300, 600, TexturePlot, tex);
            Bresenham.Circle(pixelUV, 300, TexturePlot, tex);
        }

        tex.Apply();
		lastUV = pixelUV;
		hold = true;
	}

	bool TexturePlot(int x, int y, Texture2D tex)
	{
        tex.SetPixel(x, y, color);
        return true;
	}

    bool GrayScaleTexturePlot(int x, int y, Texture2D tex, float alpha)
    {
        tex.SetPixel(x, y, new Color(alpha, alpha, alpha));
        return true;
    }
}
