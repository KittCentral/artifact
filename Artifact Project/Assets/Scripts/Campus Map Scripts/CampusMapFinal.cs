// This script goverens the execution of the campus map.  Everything needed to run the campus map is included below in this file.
// This scrip is called when the Campus Map Scene is loaded.  The loading of scenes is controlled by the main scene, the RSS Secne.

using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;


public class CampusMapFinal : MonoBehaviour {

	public Transform panel;
	public GameObject tile;
	public GameObject Layer0;
	public GameObject Layer1;
	public GameObject Layer2;
	public GameObject Layer3;
	public GameObject Layer4;
	public GameObject Layer5;
	public GameObject Layer6;
	public GameObject Layer7;

	private Texture2D TileTexture;
	private GameObject newTile;

	private Vector2 tileLocation;
	private Sprite[][,] spriteGrid = new Sprite[8][,];

	private bool[][,] isCreated = new bool[8][,];
	private bool[][,] isDownloaded = new bool[8][,];
	private Texture[][,] Buildings = new Texture[8][,];
	private bool[][,] isDownloadedBuildings = new bool[8][,];
	private Sprite tileSprite;
	private Vector3 cameraPosition;
	private GameObject[][,] Grid = new GameObject[8][,];
	private bool start = true;

	

	//for camera movement
	private bool clicked;
	private Vector3 oldMouse;
	private Vector3 changeInMouse;
	private bool moved = true;
	public float dragSpeed = 2;
	private Vector3 dragOrigin;
	private Rigidbody cameraRigidBody;
	private int frameCounter = 0;
	private Vector3 dragOrigin2;
	int xPos = 2;
	int yPos = 2;

	//for fading

	private float startTime;
	private float t;
	private float[][,] tGrid = new float[8][,];
	private bool[][,] fadedIn = new bool[8][,];
	private float[][,] startTimeGrid = new float[8][,];

	//for zooming feature
	private float zoomLevel = 0.0f;
	private int zoomLevelInt = 0;
	private Vector2 Min = new Vector2(0,1);
	private Vector2 Max = new Vector2(6,6);

	//for Dynamic tiling
	private Vector3 center;
	private int centerXInt;
	private int centerYInt;
	Dictionary<int,int> xDimensions;
	Dictionary<int,int> yDimensions;
	// Use this for initialization

	void Start () 
	{
		//initialize the 3D arrays

		xDimensions = new Dictionary<int, int> ();

		yDimensions = new Dictionary<int, int> ();

		xDimensions [0] = 3404 - 3398;

		xDimensions [1] = 6809 - 6797;

		xDimensions [2] = 13619 - 13596;

		xDimensions [3] = 27239 - 27193;

		xDimensions [4] = 54479 - 54387;

		xDimensions [5] = 108958 - 108775;

		xDimensions [6] = 217917 - 217551;

		xDimensions [7] = 435834 - 435103;

		yDimensions [0] = 6205 - 6198;

		yDimensions [1] = 12410 - 12397;

		yDimensions [2] = 24820 - 24795;

		yDimensions [3] = 49640 - 49592;

		yDimensions [4] = 99281 - 99186;

		yDimensions [5] = 198563 - 198374;

		yDimensions [6] = 397126 - 396750;

		yDimensions [7] = 794253 - 793501;

		startTime = Time.time;

		for (int i = 0; i < 8; i++) 
		{
			spriteGrid[i] = new Sprite[xDimensions[i],yDimensions[i]];
			Grid[i] = new GameObject[xDimensions[i],yDimensions[i]];
			isDownloaded[i] = new bool[xDimensions[i],yDimensions[i]];
			isCreated[i] = new bool[xDimensions[i],yDimensions[i]];
			fadedIn[i] = new bool[xDimensions[i],yDimensions[i]];
			startTimeGrid[i] = new float[xDimensions[i],yDimensions[i]];
			Buildings[i] = new Texture[xDimensions[i],yDimensions[i]];
			isDownloadedBuildings[i] = new bool[xDimensions[i],yDimensions[i]];
		}

		cameraRigidBody = Camera.main.GetComponent<Rigidbody> ();

		center = new Vector3(2.0f,2.0f,0);
		for( int i = -2; i < 3; i++)
		{
			for(int j = -2; j < 3; j++)
			{
				if(!isCreated[zoomLevelInt][xPos + i,(yPos + j)])
				{
					StartCoroutine(DownloadTileTextures(new Vector3(xPos + i,yPos + j, cameraPosition.z),"basemap",zoomLevel, xPos + i,(yPos + j)));
				}
			}
		}
		start = false;
	}
	
	// Update is called once per frame

	void FixedUpdate()
	{


		//print (xPos.ToString () + " " + yPos.ToString ());
		//int zPos = Mathf.FloorToInt (cameraPosition.z);
		//check if the tiles required tiles are downloaded and if the tiles at that location have been loaded already
		if (Input.GetMouseButtonDown(0))
		{
			dragOrigin = Input.mousePosition;
			moved = false;
		}
		
		if(Input.GetMouseButton(0))
		{
			frameCounter++;
			//print (frameCounter.ToString());
			Vector3 dragAndMove = Camera.main.ScreenToViewportPoint(Input.mousePosition - dragOrigin2);
			dragAndMove.z = 0;
			//print ("dragging" + dragAndMove.ToString());
			Camera.main.transform.Translate(-5*dragAndMove, Space.World);
		}
		dragOrigin2 = Input.mousePosition;
		if (!Input.GetMouseButton(0) && moved == false)
		{
			Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition - dragOrigin);
			//print (pos.ToString ());
			Vector3 move = new Vector3(pos.x * dragSpeed, pos.y * dragSpeed,0); 
			if(frameCounter < 20)
			{
				cameraRigidBody.velocity = -5*move;

				//print ("fling");
			}
			frameCounter = 0;
			//print ("fling over");
			//Camera.main.transform.Translate(-move, Space.World);
			moved = true;
		}


	}


	void Update () {
		//t = (Time.time - startTime) / 2.0f;
		//print (t.ToString ());
		cameraPosition = Camera.main.transform.position;
		print (cameraPosition.y.ToString());
		xPos = Mathf.FloorToInt (cameraPosition.x / 2.56f);
		yPos = Mathf.FloorToInt (-1*cameraPosition.y / 2.56f);
//		for(int i = -2; i < 3; i++)
//		{
//			for(int j = -2; j < 3; j++)
//			{
//				if(xPos + i > -1 && -1 * yPos + j > -1)
//				{
//
//					if(!isCreated[zoomLevelInt][xPos + i,-1 * (yPos + j)])
//					{
//						StartCoroutine(DownloadTileTextures(new Vector3(xPos + i,yPos + j, cameraPosition.z),"basemap",zoomLevel, xPos + i,-1 * (yPos + j)));
//					}

//					if(isDownloaded[zoomLevelInt][xPos + i,-1 * yPos + j] && !isCreated[zoomLevelInt][xPos + i,-1 * yPos + j])
//					{
//						StartCoroutine(makeTile(xPos + i,-1 * yPos + j));
//					}
//					if(!fadedIn[zoomLevelInt][xPos + i,-1 * (yPos + j)])
//					{
//						print ("fading");
//						DarthFader(Grid[zoomLevelInt][xPos + i,-1 * (yPos + j)],xPos + i, -1 * (yPos + j));
//					}
//				}
//			}
//		}
		if(Camera.main.transform.position.x/2.56 >= center.x + 1.0f)
		{
			MoveRight();
		}
		if(Camera.main.transform.position.x/2.56 <= center.x - 1.0f)
		{
			MoveLeft();
		}
		if(-1*Camera.main.transform.position.y/2.56 >= center.y + 1.0)
		{
			print ("Move up");
			MoveUp();
		}
	}

	void moveCamera()
	{
		if (Input.GetMouseButtonDown(0))
		{
			dragOrigin = Input.mousePosition;
			return;
		}
		
		if (!Input.GetMouseButton(0)) return;
		
		Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition - dragOrigin);
		Vector3 move = new Vector3(pos.x * dragSpeed, 0, pos.y * dragSpeed);
		
		transform.Translate(move, Space.World);




	}

	IEnumerator DownloadTileTextures(Vector3 cameraPosition, string tileType, float zoomLevel, int i, int j)
	{
		//isDownloaded [zoomLevelInt] [Mathf.FloorToInt(cameraPosition.x), Mathf.FloorToInt(-1.0f*cameraPosition.y)] = true;
		bool inStart = false;
		if(start)
		{
			inStart = true;
		}
		isCreated [zoomLevelInt] [i, j] = true;
		//Basis of the url: http://a.tiles.mapbox.com/v3/cuboulder.cu- 
		//Options after this point:
		
		//http://a.tiles.mapbox.com/v3/cuboulder.cu-INSERT TILE TYPE HERE/
		//The options are:
		//buildings
		//basemap
		//labels
		//footpaths
		//a few others, check the map website as it has all the layers
		
		//Next http://a.tiles.mapbox.com/v3/cuboulder.cu-labels/INSERT ZOOM LEVEL HERE/
		//Basically it's an int that ranges from 15-21 (check these)
		
		//Here's where it gets crazy:
		//http://a.tiles.mapbox.com/v3/cuboulder.cu-labels/16/INSERT (X,Y) LOCATION HERE
		
		//Depending on the zoom level, the numbers start at different places
		//For Level 0 (Zoomed out max): X/Y start: 3399/6199
		//For Level 14 (Zoomed out max): X/Y Finish: 3404/6205
		
		//For Level 1: X/Y Start: 6798/12398
		//For Level 15: X/Y Finish: 6809/12410
		
		//For Level 2: X/Y Start: 13597/24796
		//For Level 16: X/Y Finish: 13619/24820
		
		//For Level 3: X/Y Start: 27194/49593
		//For Level 17: X/Y Finish: 27239/49640
		
		//For Level 4: X/Y Start: 54388/99187
		//For Level 18: X/Y Finish: 54479/99281
		
		//For Level 5: X/Y Start: 108776/198375
		//For Level 19: X/Y Finish: 108958/198563
		
		//For Level 6: X/Y Start: 217552/396751
		//For Level 20: X/Y Finish: 217917/397126
		
		//For Level 7: X/Y Start: 435104/793502
		//For Level 21: X/Y Finish: 435834/794253
		
		//for any zoom level referenced in the script, I have subtracted fourteen from the above values (level 14 -> level 0)

		tileLocation.x = 3399 * Mathf.Pow (2.0f, zoomLevel) + cameraPosition.x;

		int tileLocationIntx = (int) tileLocation.x;

		tileLocation.y = 6199 * Mathf.Pow (2.0f, zoomLevel) + cameraPosition.y;

		int tileLocationInty = (int) tileLocation.y;

		string url = "http://b.tiles.mapbox.com/v3/cuboulder.cu-" + tileType + "/" + Mathf.FloorToInt(zoomLevel+14.0f).ToString()
						+ "/" + tileLocationIntx.ToString () + "/" + tileLocationInty.ToString () + ".png";
		//print (tileLocation.x.ToString() + " " + tileLocation.y.ToString());
		//Combining layers code starts here:
		string buildingsURL = "http://b.tiles.mapbox.com/v3/cuboulder.cu-buildings" + "/" + Mathf.FloorToInt(zoomLevel+14.0f).ToString()
			+ "/" + tileLocationIntx.ToString () + "/" + tileLocationInty.ToString () + ".png";
		//print (buildingsURL);
		if(!isDownloaded[zoomLevelInt][i,j])
		{
			WWW tileImage = new WWW (url);

			yield return tileImage;

			TileTexture = tileImage.texture;

			tileSprite = Sprite.Create(TileTexture,new Rect(0,0,TileTexture.width,TileTexture.height),new Vector2(0.5f,0.5f));

			spriteGrid[zoomLevelInt][i,j] = tileSprite;
			
			//print ("Downloaded");
		}
//	
//	}
//	IEnumerator makeTile(int i, int j)
//	{
		newTile = (GameObject) Instantiate (tile);
		//Get the Transform Component

		Transform positionSet = newTile.GetComponent<Transform> ();
		
		//Calculate position
		//Depends on the zoom
		//Assuming zoom = 0 for now
		//-j because of the layout of the images

		positionSet.Translate(new Vector3(i*2.56f,-j*2.56f,0));

		//print (i.ToString() + " " + j.ToString());

		//get rawImage component

		SpriteRenderer imagename = newTile.GetComponent<SpriteRenderer> ();

		//set texture
		float r = imagename.color.r;
		float g = imagename.color.g;
		float b = imagename.color.b;

		startTimeGrid [zoomLevelInt] [i, j] = Time.time;
		if(inStart == false)
		{
			print ("wat");
			imagename.color = new Color (r, g, b, 0);
		}
		//print (imagename.color.a.ToString ());
		//print (spriteGrid[zoomLevelInt][i,j].ToString());
		imagename.sprite = spriteGrid[zoomLevelInt][i,j];

		string parentName = "Layer: " + zoomLevelInt.ToString ();
//
		GameObject parent = GameObject.Find (parentName);
//
//		print (parent.ToString());


		//imagename.color.a = Mathf.Lerp (imagename.color.a, 1, Time.deltaTime * 2);

		//For readability, make the tile's name it's coordinate location
		
		newTile.name = "Layer: " + zoomLevelInt.ToString() + ", " + i.ToString () + " " + j.ToString ();

		
		Grid [zoomLevelInt] [i, j] = newTile;

		newTile.transform.SetParent(parent.transform);

		DarthFader(i,j);

		yield return new WaitForSeconds(0.0f);

	}

	Vector3 worldMapToPictureMap(Vector3 cameraPosition)
	{
		cameraPosition.x = cameraPosition.x / 256 * 100;

		cameraPosition.y = cameraPosition.y / 256 * 100;

		return cameraPosition;
	}
	Vector3 floatPosToIntPos(Vector3 floatPos)
	{
		Vector3 intPos;
		intPos.x = Mathf.FloorToInt (floatPos.x);
		intPos.y = Mathf.FloorToInt (floatPos.y);
		intPos.z = Mathf.FloorToInt (floatPos.z);
		return intPos;
	}


	void LoadCenter()
	{
		for( int i = -2; i < 3; i++)
		{
			for(int j = -2; j < 3; j++)
			{
				if(!isCreated[zoomLevelInt][xPos + i,(yPos + j)])
				{
					StartCoroutine(DownloadTileTextures(new Vector3(xPos + i,yPos + j, cameraPosition.z),"basemap",zoomLevel, xPos + i,(yPos + j)));
				}
			}
		}
	}

	void DarthFader(int i, int j)
	{
		SpriteRenderer renderer = Grid[zoomLevelInt][i,j].GetComponent<SpriteRenderer> ();
		//print (fade.ToString());
		float fade = 0.0f;
		for(float t = 0.0f; t < 1000.0f; t = t + 0.1f)
		{
			fade = t/1000.0f;
			renderer.color = new Color (1f,1f,1f, fade);
		}

		if (fade == 1.0f)
		{
			//print (t.ToString());
			print ("done");
			startTime = Time.time;
			fadedIn[zoomLevelInt][i,j] = true;
		}
			
		//print ("fade " + fade.ToString ());

	}

	IEnumerator DestroyChildrenFunction(float oldZoom)
	{
		ReInitializeArrays ();

		yield return new WaitForSeconds (0.5f);

		string tileName = "Layer: " + oldZoom.ToString();
		GameObject DestroyChildren = GameObject.Find(tileName);
		int childCount = DestroyChildren.transform.childCount;

		for(int i = 0; i < childCount; i++)
		{
			GameObject.Destroy(DestroyChildren.transform.GetChild(i).gameObject);
		}



		yield return new WaitForSeconds (0.0f);
	}

	void ReInitializeArrays()
	{	
		startTime = Time.time;		
		for (int i = 0; i < 8; i++) 
		{
			//spriteGrid[i] = new Sprite[xDimensions[i],yDimensions[i]];
			Grid[i] = new GameObject[xDimensions[i],yDimensions[i]];
			//isDownloaded[i] = new bool[xDimensions[i],yDimensions[i]];
			isCreated[i] = new bool[xDimensions[i],yDimensions[i]];
			fadedIn[i] = new bool[xDimensions[i],yDimensions[i]];
			startTimeGrid[i] = new float[xDimensions[i],yDimensions[i]];
		}
	}

	void MoveRight()
	{
		int xNew = Mathf.FloorToInt(center.x) + 3;//x location in gridspace
		int xDestroy = Mathf.FloorToInt(center.x) - 2;
		for(int i = -2; i < 3; i++)
		{
			int yNew = Mathf.FloorToInt(center.y) + i;//y location in gridspace
			if(center.x + 3.0f <= xDimensions[zoomLevelInt] && yNew >= 0 && yNew < yDimensions[zoomLevelInt]/*Just for now */ && center.x + 3.0 >=0)
			{
				if(!isCreated[zoomLevelInt][xNew,yNew])
				{
					print ("Load: " + xNew.ToString() + " " + yNew.ToString());
					StartCoroutine(DownloadTileTextures(new Vector3(center.x+3.0f,center.y + i, cameraPosition.z),"basemap",zoomLevel, xNew,yNew));
				}
			}
			if(xDestroy >= 0 && yNew >= 0 && yNew <= yDimensions[zoomLevelInt] && xDestroy < xDimensions[zoomLevelInt])
			{
				print ("Destroying: " + xDestroy.ToString() + " " + yNew.ToString());
				Destroy(Grid[zoomLevelInt][xDestroy,yNew]);
				isCreated[zoomLevelInt][xDestroy,yNew] = false;
			}
		}
		center = Camera.main.transform.position/2.56f;
		center.y = -1 * center.y;
		print ("New center: " + center.ToString());
		LoadCenter ();
	}
	void MoveLeft()
	{
		int xNew = Mathf.FloorToInt(center.x) - 3;//x location in gridspace
		int xDestroy = Mathf.FloorToInt(center.x) + 2;//x location in gridspace
		for(int i = -2; i < 3; i++)
		{
			int yNew = Mathf.FloorToInt(center.y) + i;//y location in gridspace
			if(xNew >= 0 && yNew >= 0 && yNew <= yDimensions[zoomLevelInt])//Put limits on which ones we load. i.e. Top left corner gives array out of range errors, so don't load those ones
			{
				if(!isCreated[zoomLevelInt][xNew,yNew])
				{
					print ("Load: " + xNew.ToString() + " " + yNew.ToString());
					StartCoroutine(DownloadTileTextures(new Vector3(center.x-3.0f,center.y + i, cameraPosition.z),"basemap",zoomLevel, xNew,yNew));
				}
			}
			if(xDestroy < xDimensions[zoomLevelInt] && yNew >= 0 && yNew <= yDimensions[zoomLevelInt])//if the objects to be destroyed exist
			{
				print ("Left Destroying: " + xDestroy.ToString() + " " + yNew.ToString());
				Destroy(Grid[zoomLevelInt][xDestroy,yNew]);
				isCreated[zoomLevelInt][xDestroy,yNew] = false;
			}
		}
		center = Camera.main.transform.position/2.56f;
		center.y = -1 * center.y;
		LoadCenter();
		print ("New center: " + center.ToString());
	}

	void MoveUp()
	{
		int yNew = Mathf.FloorToInt(center.y) + 3;//x location in gridspace
		int yDestroy = Mathf.FloorToInt(center.y) - 2;//x location in gridspace
		for(int i = -2; i < 3; i++)
		{
			int xNew = Mathf.FloorToInt(center.y) + i;//y location in gridspace
			if(yNew >= 0 && xNew >= 0 && xNew <= xDimensions[zoomLevelInt])//Put limits on which ones we load. i.e. Top left corner gives array out of range errors, so don't load those ones
			{
				if(!isCreated[zoomLevelInt][xNew,yNew])
				{
					print ("Load: " + xNew.ToString() + " " + yNew.ToString());
					StartCoroutine(DownloadTileTextures(new Vector3(center.x + i,center.y + 3.0f, cameraPosition.z),"basemap",zoomLevel, xNew,yNew));
				}
			}
			if(yDestroy < yDimensions[zoomLevelInt] && xNew >= 0 && xNew <= yDimensions[zoomLevelInt])//if the objects to be destroyed exist
			{
				print ("Up Destroying: " + yDestroy.ToString() + " " + yNew.ToString());
				Destroy(Grid[zoomLevelInt][yDestroy,yNew]);
				isCreated[zoomLevelInt][yDestroy,yNew] = false;
			}
		}
		center = Camera.main.transform.position/2.56f;
		center.y = -1 * center.y;
		LoadCenter();
		print ("New center: " + center.ToString());
	}
	void MoveDown()
	{
		for(int i = -2; i < 3; i++)
		{
			StartCoroutine(DownloadTileTextures(new Vector3(center.x+2.0f,center.y + i, cameraPosition.z),"basemap",zoomLevel, Mathf.FloorToInt(center.x+2.0f),-1 * (Mathf.FloorToInt(center.y) + i)));
			Destroy(Grid[zoomLevelInt][Mathf.FloorToInt(center.x-2.0f),Mathf.FloorToInt(center.y)+i]);
		}
	}
	public void ZoomInButton()
	{
		//print ("in");

		float oldZoom = zoomLevel;
		print (oldZoom.ToString ());
		zoomLevel = zoomLevel + 1.0f;
		zoomLevelInt = Mathf.FloorToInt (zoomLevel);
		//Camera.main.transform.Translate (new Vector3 (0f, 0f, -Camera.main.transform.position.z / 10));
		if(oldZoom == 0)
		{
			Max = Max * 2.0f;
			Camera.main.transform.Translate(new Vector3 (Camera.main.transform.position.x,Camera.main.transform.position.y, 0.0f));
			//print ("zoomed in");
		}
		else if(oldZoom == 1)
		{
			Max = Max * 2.0f;
			Camera.main.transform.Translate(new Vector3 (Camera.main.transform.position.x+1.28f,Camera.main.transform.position.y-1.28f, 0.0f));
		}

		cameraPosition = Camera.main.transform.position;
		xPos = Mathf.FloorToInt (cameraPosition.x / 2.56f);
		yPos = Mathf.FloorToInt (-1*cameraPosition.y / 2.56f);
		StartCoroutine (DestroyChildrenFunction (oldZoom));
		LoadCenter();


	}
	public void ZoomOutButton()
	{
		//print ("out");
		float oldZoom = zoomLevel;
		zoomLevel = zoomLevel - 1.0f;
		zoomLevelInt = Mathf.FloorToInt (zoomLevel);
		//Camera.main.transform.Translate (new Vector3 (0f, 0f, Camera.main.transform.position.z));
		if(oldZoom == 1)
		{
			Max = Max / 2.0f;
			Camera.main.transform.Translate(new Vector3 (-Camera.main.transform.position.x/2.0f,-Camera.main.transform.position.y/2.0f, 0.0f));
		}
		else if(oldZoom == 2)
		{
			Max = Max / 2.0f;
			Camera.main.transform.Translate(new Vector3 (-(Camera.main.transform.position.x+1.28f)/2.0f,-(Camera.main.transform.position.y-1.28f)/2.0f, 0.0f));
		}
		cameraPosition = Camera.main.transform.position;
		xPos = Mathf.FloorToInt (cameraPosition.x / 2.56f);
		yPos = Mathf.FloorToInt (-1*cameraPosition.y / 2.56f);
		StartCoroutine (DestroyChildrenFunction (oldZoom));
		LoadCenter();
	}


}
