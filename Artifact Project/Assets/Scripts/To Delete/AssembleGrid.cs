// Is this script used anymore?  It is very similar to Calendar.cs.

using UnityEngine;
using System.Collections;

public class AssembleGrid : MonoBehaviour {
	private GameObject testObject;
	public Transform mapSquarePrefab;
	private GameObject mapSquareClone;

	private Sprite testSprite;
	private Sprite test;
	private string filename;
	private Sprite square;

	public Sprite[,] mapSquares = new Sprite[31,13];
	//private string fileName;
	// Use this for initialization
	void Start () 
	{
		print ("start");

		for(int x = 0; x < 5; x++)
		{
			for(int y = 0; y < 5; y++)
			{
				print ("inFor");
				filename = "x " + x.ToString() + " y " + y.ToString();
				//print (x.ToString() + " " + y.ToString());
				print (filename);
				mapSquares[x,y] = Resources.Load<Sprite>(filename);
				//mapSquareClone = Instantiate(mapSquarePrefab) as GameObject;

				print (mapSquares[x,y].ToString());

			}
		}
		
		
	}

	void Update () {
	
	}
}
