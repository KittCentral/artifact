using UnityEngine;
using System.Collections;

namespace Chess
{
	public class Piece : MonoBehaviour 
	{
		BoardPosition pos;
		protected bool hasMoved;

		public BoardPosition Pos
		{
			get{ return pos;}
			
			set
			{ 
				Vector3 newPos = new Vector3(value.TileX,1,value.TileY);
				transform.localPosition = newPos;
				pos = value;
			}
		}

		void Start()
		{
			Pos = new BoardPosition((int)transform.localPosition.x,(int)transform.localPosition.z);
		}

		public void MovePiece (BoardPosition position)
		{
			Move move = new Move(this.pos,position);
			if(Check (move))
			{
				hasMoved = true;
				ChessControl.WhiteTurn = !ChessControl.WhiteTurn;
				Debug.Log("Move was made");
				Pos = position;
			}
			else
				Debug.Log ("Move not legal");
		}

		public virtual bool Check(Move move)
		{
			return false;
		}
	}
}