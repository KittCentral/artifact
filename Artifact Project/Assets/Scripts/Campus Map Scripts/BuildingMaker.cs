using UnityEngine;
using System.Collections;

public class BuildingMaker : MonoBehaviour {
	public GameObject tile;
	public GameObject parent;

	private GameObject newTile;
	private Vector3 cameraGridPos;//the position of the camera in the scale of 2.56:1 world units:image units (100 pixels per 1 world unit)
	private int cameraGridPosIntx;
	private int cameraGridPosInty;
	private float rightLoadLine;
	private float leftLoadLine;
	private float upLoadLine;
	private float downLoadLine;
	private float inLoadLine;
	private float outLoadLine;
	private GameObject[][] squaresInView = new GameObject[7][];
	void Start () 
	{
		cameraGridPos = Camera.main.transform.position / 2.56f;
		cameraGridPosIntx = Mathf.FloorToInt (cameraGridPos.x);
		cameraGridPosInty = Mathf.FloorToInt (cameraGridPos.y);
		for (int i = 0; i < 7; i++) //initial
		{
			squaresInView [i] = new GameObject[7];
		}

		for (int i = -3; i <= 3; i++) {
			for (int k = -3; k <= 3; k++) {
				squaresInView[i+3][k+3] = (GameObject)Instantiate (tile);
				squaresInView[i+3][k+3].transform.position = new Vector3 ((cameraGridPosIntx + i)*2.56f, (cameraGridPosInty+k)*2.56f, Camera.main.transform.position.z + 4.99f );
				squaresInView[i+3][k+3].name = (cameraGridPosIntx+i).ToString() + " " + (cameraGridPosInty+k).ToString() + " buildings";
				squaresInView[i+3][k+3].transform.SetParent(parent.transform);
			}
		}
		rightLoadLine = cameraGridPosIntx + 0.75f;
		leftLoadLine = cameraGridPosIntx - 0.5f;
		upLoadLine = cameraGridPosInty + 0.75f;
		downLoadLine = cameraGridPosInty - 0.5f;
		inLoadLine = -13.5f;
		outLoadLine = -14.5f;
	}
	// Update is called once per frame
	void Update () 
	{
		cameraGridPos = Camera.main.transform.position / 2.56f;
		cameraGridPosIntx = Mathf.FloorToInt (cameraGridPos.x);
		cameraGridPosInty = Mathf.FloorToInt (cameraGridPos.y);
		int cameraGridPosIntz = Mathf.FloorToInt (cameraGridPos.z);
		if (cameraGridPos.x > rightLoadLine) 
		{
			rightLoadLine = rightLoadLine + 1.0f;
			leftLoadLine = leftLoadLine + 1.0f;
			loadRight (cameraGridPosIntx, cameraGridPosInty);
		}
		if (cameraGridPos.x < leftLoadLine) 
		{
			rightLoadLine = rightLoadLine - 1.0f;
			leftLoadLine = leftLoadLine - 1.0f;
			loadLeft (cameraGridPosIntx, cameraGridPosInty);

		}
		if (cameraGridPos.y > upLoadLine) 
		{
			upLoadLine++;
			downLoadLine++;
			loadUp(cameraGridPosIntx, cameraGridPosInty);
		}
		if (cameraGridPos.y < downLoadLine) 
		{
			upLoadLine--;
			downLoadLine--;
			loadDown (cameraGridPosIntx, cameraGridPosInty);
		}
	}

	private void loadRight(int posXInt, int posYInt)
	{
		for (int i = 0; i < 7; i++)//clear far left column of squares
		{
			GameObject.Destroy(squaresInView[0][i]);
		}
		for(int i = 0; i < 6; i++)//shift all the other squares left one column
		{
			for(int j = 0; j < 7; j++)
				squaresInView[i][j] = squaresInView[i+1][j];
		}
		for (int i = -3; i <= 3; i++)//write over the far right column with the new squares
		{
			squaresInView[6][i+3] = (GameObject)Instantiate (tile);
			squaresInView[6][i+3].transform.position = new Vector3 ((posXInt + 4)*2.56f, (posYInt+i)*2.56f, Camera.main.transform.position.z + 4.99f);
			squaresInView[6][i+3].name = (posXInt+4).ToString() + " " + (posYInt+i).ToString() + " buildings";
			squaresInView[6][i+3].transform.SetParent(parent.transform);
		}

	}
	private void loadLeft(int posXInt, int posYInt)
	{
		for (int i = 0; i < 7; i++)//clear far Right column of squares
		{
			GameObject.Destroy(squaresInView[6][i]);
		}
		for(int i = 6; i > 0; i--)//shift all the other squares right one column
		{
			for(int j = 0; j < 7; j++)
				squaresInView[i][j] = squaresInView[i-1][j];
		}
		for (int i = -3; i <= 3; i++)//write over the far left column with the new squares
		{
			squaresInView[0][i+3] = (GameObject)Instantiate (tile);
			squaresInView[0][i+3].name = (posXInt-3).ToString() + " " + (posYInt+i).ToString() + " buildings";
			squaresInView[0][i+3].transform.position = new Vector3 ((posXInt - 3)*2.56f, (posYInt+i)*2.56f, Camera.main.transform.position.z + 4.99f);
			squaresInView[0][i+3].transform.SetParent(parent.transform);
		}

	}
	private void loadUp(int posXInt, int posYInt)
	{
		for (int i = 0; i < 7; i++) //clear bottom row of squares
			GameObject.Destroy(squaresInView[i][0]);
		for (int i = 0; i < 7; i++) //shift the rows down one
		{
			for (int j = 0; j < 6; j++) 
				squaresInView [i] [j] = squaresInView [i] [j+1];
		}
		for (int i = -3; i <= 3; i++)//fill the top row with the new squares
		{
			squaresInView[i+3][6] = (GameObject)Instantiate(tile);
			squaresInView[i+3][6].name = (posXInt + i).ToString() + " " + (posYInt+3).ToString() + " buildings";
			squaresInView[i+3][6].transform.position = new Vector3((posXInt+i)*2.56f,(posYInt+3)*2.56f,Camera.main.transform.position.z + 4.99f);
			squaresInView[i+3][6].transform.SetParent(parent.transform);
		}

	}
	private void loadDown(int posXInt, int posYInt)
	{
		for(int i = 0; i < 7; i++)//clear top row of squares
			GameObject.Destroy(squaresInView[i][6]);
		for (int i = 0; i < 7; i++) //shift squares up one row
		{
			for (int j = 6; j > 0; j--)
				squaresInView [i] [j] = squaresInView [i] [j - 1];
		}
		for (int i = -3; i <= 3; i++) 
		{
			squaresInView[i+3][0] = (GameObject)Instantiate(tile);
			squaresInView[i+3][0].name = (posXInt + i).ToString() + " " + (posYInt-3).ToString() + " buildings";
			squaresInView[i+3][0].transform.position = new Vector3((posXInt+i)*2.56f,(posYInt-3)*2.56f,Camera.main.transform.position.z + 4.99f);
			squaresInView[i+3][0].transform.SetParent(parent.transform);
		}
	}
	public void loadinOut()
	{
		for (int i = 0; i < 7; i++) {
			for (int j= 0; j < 7; j++) {
				GameObject.Destroy (squaresInView [i] [j]);
			}
		}
		Start ();
	}
}




