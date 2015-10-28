using UnityEngine;
using System.Collections;
using Algorithms;

namespace Chess
{
	public class King : Piece 
	{
		public override bool Check (Move move)
		{
			int tx = move.to.TileX; int ty = move.to.TileY; int fx = move.from.TileX; int fy = move.from.TileY;
			if((Basic.IsBetween(tx,0,7) && Basic.IsBetween(ty,0,7)) && !(tx == fx && ty == fy))
			{
				if (Basic.IsBetween(Mathf.Abs(tx-fx),-1,1) && Basic.IsBetween(Mathf.Abs(ty-fy),-1,1))
					return true;
				else
					return false;
			}
			else
				return false;
		}
	}
}
