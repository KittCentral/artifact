using UnityEngine;
using System.Collections;

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
			LineDraw(pixelUV,lastUV, tex);
		tex.Apply();
		lastUV = pixelUV;
		hold = true;
	}

	void LineDraw(Vector2 p1, Vector2 p2, Texture2D tex)
	{
		float m;
		float dy = (int)p2.y-(int)p1.y;
		float dx = (int)p2.x-(int)p1.x;
		if (dx != 0)
			m = dy/dx;
		else
			m = dy;
		print ("m:" + m + " dy:" + dy + " dx:" + dx);
		if(p1.x<p2.x)
		{
			for(int i = 0; i <= Mathf.Abs((int)p1.x-(int)p2.x); i++)
			{
				float y = m*i + p1.y;
				for(int j = 0; j < Mathf.Abs(m)+1; j++)
				{
					tex.SetPixel(i+(int)p1.x, (int)y + j, color);
				}
				tex.Apply();
			}
		}
		else
		{
			for(int i = Mathf.Abs((int)p1.x-(int)p2.x); i >= 0; i--)
			{
				float y = -m*i + p1.y;
				for(int j = 0; j < Mathf.Abs(m)+1; j++)
				{
					tex.SetPixel((int)p1.x - i, (int)y + j, color);
				}
				tex.Apply();
			}
		}
	}
}
