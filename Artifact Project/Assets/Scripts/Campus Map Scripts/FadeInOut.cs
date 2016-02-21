using UnityEngine;
using System.Collections;

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

	void Start () 
	{
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
		float xLocationForURL = 3399 * Mathf.Pow (2,zoomLevel) + xLoc;
		float yLocationForURL = 6205 * Mathf.Pow (2,zoomLevel) - yLoc;//their coordinates start in the top left, ours start in the bottom left -> invert the y axis

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























