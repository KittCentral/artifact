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
			if (tx == fx && ty == fy + 1 * (whiteTeam?1:-1))
				return true;
			else if(tx == fx && ty == fy + 2 * (whiteTeam?1:-1) && hasMoved == false)
				return true;
			else
				return false;
		}
	}
}