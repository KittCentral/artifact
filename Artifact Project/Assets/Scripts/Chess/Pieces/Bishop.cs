using UnityEngine;
using System.Collections;
using Algorithms;

namespace Chess
{
	public class Bishop : Piece 
	{
		public override bool Check (Move move)
		{
			int tx = move.to.TileX; int ty = move.to.TileY; int fx = move.from.TileX; int fy = move.from.TileY;
			if((Basic.IsBetween(tx,0,7) && Basic.IsBetween(ty,0,7)) && !(tx == fx && ty == fy))
			{
				if ((Mathf.Abs(tx-fx) == Mathf.Abs(ty-fy)))
					return true;
				else
					return false;
			}
			else
				return false;
		}
	}
}
