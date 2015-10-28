using UnityEngine;
using System.Collections;

namespace Chess
{
	public class BoardPosition 
	{
		int tileX, tileY;

		public BoardPosition(int x, int y)
		{
			tileX = x;
			tileY = y;
		}

		public int TileX
		{
			get{ return tileX;}

			set{ tileX = value;}
		}

		public int TileY
		{
			get{ return tileY;}

			set{ tileY = value;}
		}
	}
}