using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FadeInOut : MonoBehaviour {

	public GameObject newTile;

	private Sprite newSprite;
	private float alphaChange = 0;
	private SpriteRenderer tileSpriteRenderer;
	private bool isLoaded = false;
	private bool error;
	private int xGridLocation;
	private int yGridLocation;
	private string[] numbers;
	private Texture2D imageTex2D;
	private Dictionary <int, Vector2> zoomLevelToXYLocations = new Dictionary<int,Vector2>();

	void Start () 
	{
		for (int i = 0; i < 15; i++)//This is necessary to adjust the placement of tiles, to ensure the camera location stays the same between zoom levels.  
									//Unfortunately the system that sorted the source files has an offset between each section
		{
		
			if (i == 0) {
				zoomLevelToXYLocations.Add (i, new Vector2 (3399 * Mathf.Pow (2,i), 6205 * Mathf.Pow (2,i)));
			}
			if (i == 1) {
				zoomLevelToXYLocations.Add (i, new Vector2 (3399 * Mathf.Pow (2,i), 6205 * Mathf.Pow (2,i)+1));//done
			}
			else if(i == 2)
			{
				zoomLevelToXYLocations.Add (i, new Vector2 (3399 * Mathf.Pow (2,i), 6205 * Mathf.Pow (2,i)+3));//done
			}
			else if(i == 3)
			{
				zoomLevelToXYLocations.Add (i, new Vector2 (3399 * Mathf.Pow (2,i), 6205 * Mathf.Pow (2,i)+7));//done
			}
			else if(i == 4)
			{
				zoomLevelToXYLocations.Add (i, new Vector2 (3399 * Mathf.Pow (2,i), 6205 * Mathf.Pow (2,i)+15));//done
			}
			else if(i == 5)
			{
				zoomLevelToXYLocations.Add (i, new Vector2 (3399 * Mathf.Pow (2,i), 6205 * Mathf.Pow (2,i)+31));
			}
			else if(i == 6)
			{
				zoomLevelToXYLocations.Add (i, new Vector2 (3399 * Mathf.Pow (2,i), 6205 * Mathf.Pow (2,i)+63));
			}
			else if(i == 7)
			{
				zoomLevelToXYLocations.Add (i, new Vector2 (3399 * Mathf.Pow (2,i), 6205 * Mathf.Pow (2,i)+127));
			}
			else if(i == 8)
			{
				zoomLevelToXYLocations.Add (i, new Vector2 (3399 * Mathf.Pow (2,i), 6205 * Mathf.Pow (2,i)+265));
			}
			else if(i == 9)
			{
				zoomLevelToXYLocations.Add (i, new Vector2 (3399 * Mathf.Pow (2,i), 6205 * Mathf.Pow (2,i)));
			}
			else if(i == 10)
			{
				zoomLevelToXYLocations.Add (i, new Vector2 (3399 * Mathf.Pow (2,i), 6205 * Mathf.Pow (2,i)));
			}
			else if(i == 11)
			{
				zoomLevelToXYLocations.Add (i, new Vector2 (3399 * Mathf.Pow (2,i), 6205 * Mathf.Pow (2,i)));
			}
			else if(i == 12)
			{
				zoomLevelToXYLocations.Add (i, new Vector2 (3399 * Mathf.Pow (2,i), 6205 * Mathf.Pow (2,i)));
			}
			else if(i == 13)
			{
				zoomLevelToXYLocations.Add (i, new Vector2 (3399 * Mathf.Pow (2,i), 6205 * Mathf.Pow (2,i)));
			}
			else if(i == 14)
			{
				zoomLevelToXYLocations.Add (i, new Vector2 (3399 * Mathf.Pow (2,i), 6205 * Mathf.Pow (2,i)));
			}


		}

		//testSprite = Sprite.Create (TestPic, new Rect (0, 0, TestPic.width, TestPic.height), new Vector2(0.0f,0.0f));
		tileSpriteRenderer = newTile.GetComponent<SpriteRenderer> ();
		//tileSpriteRenderer.sprite = testSprite;
		numbers = newTile.name.Split(" "[0]);
		xGridLocation = int.Parse (numbers [0]);
		yGridLocation = int.Parse (numbers [1]);
		StartCoroutine (downloadImage (xGridLocation, yGridLocation));
	}

	// Update is called once per frame
	void Update () 
	{
		if (isLoaded == true && error == false) {//if the image downloaded without error
			tileSpriteRenderer.sprite = newSprite;
			alphaChange = Mathf.Lerp (alphaChange, 1, 0.03f);//this is the fading effect
			tileSpriteRenderer.color = new Color (1f, 1f, 1f, alphaChange);//assign the new alpha value
		}  else if (isLoaded == true && error == true) //if there was an error downloading
		{
			tileSpriteRenderer.sprite = newSprite;
			tileSpriteRenderer.color = new Color (1f, 1f, 1f, 0);
		}
	}


	IEnumerator downloadImage(int xLoc, int yLoc)
	{
		int zoomLevel = (int)-Camera.main.transform.position.z-14;//subtracted 14 for the calculations below- it is added again in the url
		print (zoomLevel);
//		float xLocationForURL = 3399 * Mathf.Pow (2,zoomLevel) + xLoc;
//		float yLocationForURL = 6205 * Mathf.Pow (2,zoomLevel) - yLoc;//their coordinates start in the top left, ours start in the bottom left -> invert the y axis

		float xLocationForURL = zoomLevelToXYLocations[zoomLevel].x + xLoc;
		float yLocationForURL = zoomLevelToXYLocations[zoomLevel].y - yLoc;

		string URL = "http://b.tiles.mapbox.com/v3/cuboulder.cu-"+ numbers[2] + "/" + (zoomLevel+14).ToString() + "/" + xLocationForURL.ToString ()
			+ "/" + yLocationForURL.ToString () + ".png";
		WWW image = new WWW (URL);

		yield return image;

		if (!string.IsNullOrEmpty (image.error)) {//if there was an error getting the image
			imageTex2D = new Texture2D (256, 256);
			error = true;
		}
		else
			imageTex2D = image.texture;

		newSprite = Sprite.Create (imageTex2D, new Rect (0, 0, imageTex2D.width, imageTex2D.height), new Vector2(0.0f,0.0f));

		isLoaded = true;

		yield return new WaitForSeconds(0.0f);


	}
}























