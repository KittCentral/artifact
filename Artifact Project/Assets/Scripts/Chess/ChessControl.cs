using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Algorithms.ExtensionMethods;

namespace Chess
{
	public class ChessControl : MonoBehaviour 
	{
		public CameraController cam;
		public InputField inputField;
		public Piece[] whitePieces = new Piece[16];
		public Piece[] blackPieces = new Piece[16];
		public GameObject lightExplosion;
		public GameObject darkExplosion;
		string moveName, firstClick, secondClick;
		bool down;
		RaycastHit hit;
		Ray ray;
		Piece subject;
		Vector3 start;

		static bool whiteTurn;
		public static bool WhiteTurn { get{return whiteTurn;}  set{whiteTurn = value;} }

		void Start()
		{
			whiteTurn = true;
		}

		void Update () 
		{
			if(whiteTurn)
				cam.SetIndex = 0;
			else
				cam.SetIndex = 1;

			if(Input.GetKeyUp(KeyCode.KeypadEnter) || Input.GetKeyUp(KeyCode.Return))
				SendFromInput();

			moveName = inputField.text;


			if(Input.GetMouseButtonDown(0))
			{
				ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				
				if (Physics.Raycast(ray, out hit)) 
				{
					Transform objectHit = hit.transform;
					start = new Vector3(objectHit.position.x,.5f,objectHit.position.z);
					firstClick = (objectHit.localPosition.x + 1) + "" + (objectHit.parent.transform.localPosition.z + 1);
					if(!(PieceAtPoint(firstClick) == null))
					{
						down = true;
						subject = PieceAtPoint(firstClick);
					}
				}
			}
			if(Input.GetMouseButton(0) && down)
			{
				ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				
				if (Physics.Raycast(ray, out hit)) 
				{
					subject.target = new Vector3(hit.point.x,2,hit.point.z);
				}
			}
			if(Input.GetMouseButtonUp(0))
			{
				ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				print (down);
				if(down)
				{
					if (Physics.Raycast(ray, out hit)) 
					{
						Transform objectHit = hit.transform;
						secondClick = (objectHit.localPosition.x + 1) + "" + (objectHit.parent.transform.localPosition.z + 1);
						if(!subject.MovePiece(secondClick.ToBoardPosition()))
						{
							subject.target = start;
						}
					}
					else
					{
						subject.target = start;
					}
				}
				down = false;
			}
		}

		public void SendFromInput()
		{
			if(moveName.Length == 4)
			{
				char[] array = moveName.ToCharArray();
				string first = CoordNameFromMoveName(array[0],array[1]);
				string second = CoordNameFromMoveName(array[2],array[3]);
				if(PieceAtPoint(first) == null)
					return;
				subject = PieceAtPoint(first);
				subject.MovePiece(second.ToBoardPosition());
				inputField.text = "";
			}
		}

		public Piece PieceAtPoint(string first)
		{
			BoardPosition from = first.ToBoardPosition();
			int pieceIndex;
			if(WhiteTurn)
			{
				if(!(FindWhitePiece(from) == null))
					return whitePieces[(int)FindWhitePiece(from)];
				else
				{
					Debug.Log ("Not a valid start");
					return null;
				}
			}
			else
			{
				if(!(FindBlackPiece(from) == null))
					return blackPieces[(int)FindBlackPiece(from)];
				else
				{
					Debug.Log ("Not a valid start");
					return null;
				}
			}
		}

		public int? FindWhitePiece(BoardPosition position)
		{
			for(int i=0; i<16; i++)
			{
				if(whitePieces[i].Pos.TileX == position.TileX && whitePieces[i].Pos.TileY == position.TileY)
					return i;
			}
			return null;
		}

		public int? FindBlackPiece(BoardPosition position)
		{
			for(int i=0; i<16; i++)
			{
				if(blackPieces[i].Pos.TileX == position.TileX && blackPieces[i].Pos.TileY == position.TileY)
					return i;
			}
			return null;
		}

		string CoordNameFromMoveName(char x, char y)
		{
			char[] array = new char[2];
			array[0] = x;
			array[1] = y;
			return new string(array);
		}
	}
}