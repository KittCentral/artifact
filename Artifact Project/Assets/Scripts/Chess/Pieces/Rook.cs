using UnityEngine;
using System.Collections;
using Algorithms;

namespace Chess
{
	public class Rook : Piece 
	{
		public override bool SpecificCheck (Move move)
		{
			int tx = move.to.TileX; int ty = move.to.TileY; int fx = move.from.TileX; int fy = move.from.TileY;
			if ((tx == fx || ty == fy) && PathCheck(tx,ty,fx,fy))
				return true;
			else
				return false;
		}

		bool PathCheck(int tx, int ty, int fx, int fy)
		{
			bool changeX = ty-fy==0;
			int change = changeX ? Mathf.Abs(tx-fx) : Mathf.Abs (ty-fy);
			int dir;
			if(changeX)
			{
				dir=Mathf.Abs(tx-fx)/(tx-fx)>0?1:-1;
				for(int i = 1; i < change; i++)
				{
					print (EmptyCheck(new BoardPosition(fx + i*dir,ty)));
					if(!EmptyCheck(new BoardPosition(fx + i*dir,ty)))
					   return false;
				}
			}
			else
			{
				dir=Mathf.Abs(ty-fy)/(ty-fy)>0?1:-1;
				for(int i = 1; i < change; i++)
				{
					if(!EmptyCheck(new BoardPosition(tx,fy + i*dir)))
						return false;
				}
			}
			return true;
		}
	}
}
