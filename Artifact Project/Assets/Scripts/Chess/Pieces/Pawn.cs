using UnityEngine;
using System.Collections;
using Algorithms;

namespace Chess
{
	public class Pawn : Piece 
	{
		public override bool SpecificCheck (Move move)
		{
			int tx = move.to.TileX; int ty = move.to.TileY; int fx = move.from.TileX; int fy = move.from.TileY;
			if (tx == fx && ty == fy + 1 * (whiteTeam?1:-1) && EmptyCheck(move.to, !whiteTeam))
				return true;
			else if(tx == fx && ty == fy + 2 * (whiteTeam?1:-1) && hasMoved == false && PathCheck(fx, fy, whiteTeam) && EmptyCheck(move.to, !whiteTeam))
				return true;
			else
				return false;
		}

		public override bool KillCheck (Move move)
		{
			int tx = move.to.TileX; int ty = move.to.TileY; int fx = move.from.TileX; int fy = move.from.TileY;
			if(!EmptyCheck(move.to, !whiteTeam))
			{
				if (Mathf.Abs (fx-tx) == 1 && ty == fy + 1 * (whiteTeam?1:-1))
					return true;
			}
			return false;
		}

		bool PathCheck(int fx, int fy, bool whiteTeam)
		{
			if(!EmptyCheck(new BoardPosition(fx,fy + (whiteTeam?1:-1)),whiteTeam))
				return false;
			return true;
		}
	}
}