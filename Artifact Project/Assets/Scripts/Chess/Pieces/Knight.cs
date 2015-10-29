using UnityEngine;
using System.Collections;
using Algorithms;

namespace Chess
{
	public class Knight : Piece 
	{
		public override bool SpecificCheck (Move move)
		{
			int tx = move.to.TileX; int ty = move.to.TileY; int fx = move.from.TileX; int fy = move.from.TileY;
			if ((Mathf.Abs (tx-fx) == 2 && Mathf.Abs (ty-fy) == 1) || (Mathf.Abs (tx-fx) == 1 && Mathf.Abs (ty-fy) == 2))
				return true;
			else
				return false;
		}
	}
}
