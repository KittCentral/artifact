using UnityEngine;
using System.Collections;
using Algorithms;

namespace Chess
{
	public class Queen : Piece 
	{
		public override bool SpecificCheck (Move move)
		{
			int tx = move.to.TileX; int ty = move.to.TileY; int fx = move.from.TileX; int fy = move.from.TileY;
			if (((tx == fx || ty == fy) || (Mathf.Abs(tx-fx) == Mathf.Abs(ty-fy))) && PathCheck (tx,ty,fx,fy))
				return true;
			else
				return false;
		}

		bool PathCheck(int tx, int ty, int fx, int fy)
		{
			if((Mathf.Abs(tx-fx) == Mathf.Abs(ty-fy)))
			{
				int change = Mathf.Abs(ty-fy);
				int dirX = (tx-fx)/Mathf.Abs (tx-fx);
				int dirY = (ty-fy)/Mathf.Abs (ty-fy);
				for(int i = 1; i < change; i++)
				{
					if(!EmptyCheck(new BoardPosition(fx + i*dirX,fy + i*dirY),whiteTeam))
						return false;
				}
				return true;
			}
			else
			{
				bool changeX = ty-fy==0;
				int change = changeX ? Mathf.Abs(tx-fx) : Mathf.Abs (ty-fy);
				int dir;
				if(changeX)
				{
					dir=Mathf.Abs(tx-fx)/(tx-fx);
					for(int i = 1; i < change; i++)
					{
						if(!EmptyCheck(new BoardPosition(fx + i*dir,ty),whiteTeam))
							return false;
					}
				}
				else
				{
					dir=Mathf.Abs(ty-fy)/(ty-fy);
					for(int i = 1; i < change; i++)
					{
						if(!EmptyCheck(new BoardPosition(tx,fy + i*dir),whiteTeam))
							return false;
					}
				}
				return true;
			}
		}
	}
}
