using UnityEngine;
using System.Collections;
using Algorithms;

namespace Chess
{
	public class Bishop : Piece 
	{
		public override bool SpecificCheck (Move move)
		{
			int tx = move.to.TileX; int ty = move.to.TileY; int fx = move.from.TileX; int fy = move.from.TileY;
			if ((Mathf.Abs(tx-fx) == Mathf.Abs(ty-fy)))
				return true;
			else
				return false;
		}
	}
}
