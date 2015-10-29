using UnityEngine;
using System.Collections;
using Algorithms;

namespace Chess
{
	public class Piece : MonoBehaviour 
	{
		public ChessControl control;
		BoardPosition pos;
		protected bool hasMoved;
		public bool whiteTeam;

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
			if(GeneralCheck (move))
			{
				hasMoved = true;
				ChessControl.WhiteTurn = !ChessControl.WhiteTurn;
				Debug.Log("Move was made");
				Pos = position;
			}
			else
				Debug.Log ("Move not legal");
		}

		bool GeneralCheck(Move move)
		{
			int tx = move.to.TileX; int ty = move.to.TileY; int fx = move.from.TileX; int fy = move.from.TileY;
			if((Basic.IsBetween(tx,0,7) && Basic.IsBetween(ty,0,7)) && !(tx == fx && ty == fy) && SpecificCheck (move) && EmptyCheck(move.to))
				return true;
			else
				return false;
		}

		public virtual bool SpecificCheck(Move move)
		{
			return false;
		}

		protected bool EmptyCheck(BoardPosition position)
		{
			int? index;
			if(whiteTeam)
				index = control.FindWhitePiece(position);
			else
				index = control.FindBlackPiece(position);
			if( index == null)
				return true;
			else
				return false;
		}
	}
}