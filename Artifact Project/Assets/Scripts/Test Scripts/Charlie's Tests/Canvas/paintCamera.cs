using UnityEngine;
using System.Collections;
using Algorithms;

public static class paintCamera
{
	static Vector2 lastUV;
	static Texture2D canvas = new Texture2D(200,200,TextureFormat.RGB24,false);

	public static void DrawOnLine(Vector3 start, Vector3 end)
	{
		int layerMask = 1 << 11;
		RaycastHit hit;
		if(!Physics.Raycast(new Ray(start, end-start), out hit, 20f, layerMask))
			return;

		if(!Draw(hit))
			return;
	}

	public static void DrawAtMouse(Camera cam)
	{
		if (!Input.GetKey(KeyCode.Mouse0))
			return;

		RaycastHit hit;
		if (!Physics.Raycast(cam.ScreenPointToRay(Input.mousePosition), out hit))
			return;

		if (!Draw(hit))
			return; 
	}

	static bool Draw(RaycastHit hit)
	{
		Renderer rend = hit.transform.GetComponent<Renderer>();
		MeshCollider meshCollider = hit.collider as MeshCollider;
		rend.sharedMaterial.mainTexture = canvas;
		if (rend == null || rend.sharedMaterial == null || rend.sharedMaterial.mainTexture == null || meshCollider == null)
			return false;
		
		Texture2D tex = rend.material.mainTexture as Texture2D;
		Vector2 pixelUV = hit.textureCoord;
		pixelUV.x *= tex.width;
		pixelUV.y *= tex.height;
		tex.SetPixel((int)pixelUV.x, (int)pixelUV.y, Color.black);
		Bresenham.Line(pixelUV,lastUV,LinePlot,tex);
		tex.Apply();
		lastUV = pixelUV;
		return true;
	}

	static bool LinePlot(int x, int y)
	{
		return true;
	}
}
