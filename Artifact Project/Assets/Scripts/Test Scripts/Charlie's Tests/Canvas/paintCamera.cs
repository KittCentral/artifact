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
		canvas = new Texture2D(200,200, TextureFormat.ARGB32, false);
	}

	void Update() 
	{
		if (!Input.GetKey(KeyCode.Mouse0))
		{
			hold = false;
			return;
		}
		
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
		pixelUV.x *= tex.width;
		pixelUV.y *= tex.height;
		tex.SetPixel((int)pixelUV.x, (int)pixelUV.y, Color.black);
		if(hold)
			Bresenham.Line(pixelUV,lastUV,LinePlot,tex);
		tex.Apply();
		lastUV = pixelUV;
		hold = true;
	}

	bool LinePlot(int x, int y)
	{
		return true;
	}
}
