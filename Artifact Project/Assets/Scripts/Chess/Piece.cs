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
		public Vector3 target;
		GameObject explosionPrefab;

		public BoardPosition Pos
		{
			get{ return pos;}
			
			set{ pos = value;}
		}

		void Start()
		{
			explosionPrefab = whiteTeam ? control.darkExplosion : control.lightExplosion;
			Pos = new BoardPosition((int)transform.localPosition.x,(int)transform.localPosition.z);
			target = new Vector3(Pos.TileX,.5f,Pos.TileY);
		}

		void Update()
		{
			transform.localPosition = Vector3.Lerp(transform.localPosition,target,.1f);
		}

		public bool MovePiece (BoardPosition position)
		{
			Move move = new Move(this.pos,position);
			if(GeneralCheck (move))
			{
				hasMoved = true;
				ChessControl.WhiteTurn = !ChessControl.WhiteTurn;
				Debug.Log("Move was made");
				Pos = position;
				target = new Vector3(position.TileX,.5f,position.TileY);
				return true;
			}
			return false;
		}

		bool GeneralCheck(Move move)
		{
			int tx = move.to.TileX; int ty = move.to.TileY; int fx = move.from.TileX; int fy = move.from.TileY;
			if((Basic.IsBetween(tx,0,7) && Basic.IsBetween(ty,0,7)) && !(tx == fx && ty == fy) && EmptyCheck(move.to, whiteTeam))
			{
				if(KillCheck(move))
				{
					if(whiteTeam)
					{
						Piece hitPieceB = control.blackPieces[(int)control.FindBlackPiece(move.to)];
						hitPieceB.Pos.TileX += 10;
						Instantiate(explosionPrefab,hitPieceB.gameObject.transform.position,hitPieceB.gameObject.transform.rotation);
						Destroy(hitPieceB.gameObject);
					}
					else
					{
						Piece hitPieceW = control.whitePieces[(int)control.FindWhitePiece(move.to)];
						hitPieceW.Pos.TileX -= 10;
						Instantiate(explosionPrefab,hitPieceW.gameObject.transform.position,hitPieceW.gameObject.transform.rotation);
						Destroy(hitPieceW.gameObject);
					}
					return true;
				}
				else if(SpecificCheck (move))
					return true;
				else
				{
					Debug.Log ("Move not legal");
					return false;
				}
			}
			else
			{
				if(!EmptyCheck(move.to, whiteTeam))
					Debug.Log ("Space is Occupied");
				Debug.Log ("Not a valid Endpoint");
				return false;
			}
		}

		public virtual bool SpecificCheck(Move move)
		{
			return false;
		}

		public virtual bool KillCheck(Move move)
		{
			if(!EmptyCheck(move.to, !whiteTeam) && SpecificCheck(move))
				return true;
			return false;
		}

		protected bool EmptyCheck(BoardPosition position, bool whiteTeam)
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