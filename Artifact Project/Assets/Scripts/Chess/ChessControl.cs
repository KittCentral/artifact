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
		string moveName;

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
				Send();

			moveName = inputField.text;
		}

		public void Send()
		{
			if(moveName.Length == 4)
			{
				char[] array = moveName.ToCharArray();
				string first = CoordNameFromMoveName(array[0],array[1]);
				string second = CoordNameFromMoveName(array[2],array[3]);
				BoardPosition from = first.ToBoardPosition();
				BoardPosition to = second.ToBoardPosition();
				int pieceIndex;
				if(WhiteTurn)
				{
					if(!(FindWhitePiece(from) == null))
					{
						pieceIndex = (int)FindWhitePiece(from);
						whitePieces[pieceIndex].MovePiece(to);
					}
					else
					{
						Debug.Log ("Not a valid start");
						return;
					}
				}
				else
				{
					if(!(FindBlackPiece(from) == null))
					{
						pieceIndex = (int)FindBlackPiece(from);
						blackPieces[pieceIndex].MovePiece(to);
					}
					else
					{
						Debug.Log ("Not a valid start");
						return;
					}
				}
				inputField.text = "";
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