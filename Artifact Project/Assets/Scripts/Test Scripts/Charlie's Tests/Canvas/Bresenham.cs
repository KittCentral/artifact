using UnityEngine;
using System.Collections;

namespace Algorithms
{
	public static class Bresenham
	{
		private static void Swap<T>(ref T lhs, ref T rhs) { T temp; temp = lhs; lhs = rhs; rhs = temp; }
		
		/// <summary>
		/// The plot function delegate
		/// </summary>
		/// <param name="x">The x co-ord being plotted</param>
		/// <param name="y">The y co-ord being plotted</param>
		/// <returns>True to continue, false to stop the algorithm</returns>
		public delegate bool PlotFunction(int x, int y);
		
		/// <summary>
		/// Plot the line from (p1.x, p1.y) to (p2.x, p2.y)
		/// </summary>
		/// <param name="p1.x">The start x</param>
		/// <param name="p1.y">The start y</param>
		/// <param name="p2.x">The end x</param>
		/// <param name="p2.y">The end y</param>
		/// <param name="plot">The plotting function (if this returns false, the algorithm stops early)</param>
		public static void Line(Vector2 p1, Vector2 p2, PlotFunction plot, Texture2D tex)
		{
			bool steep = Mathf.Abs(p2.y - p1.y) > Mathf.Abs(p2.x - p1.x);
			if (steep) {
				Swap<float>(ref p1.x, ref p1.y); 
				Swap<float>(ref p2.x, ref p2.y); 
			}

			if (p1.x > p2.x) 
			{ 
				Swap<float>(ref p1.x, ref p2.x); 
				Swap<float>(ref p1.y, ref p2.y); 
			}

			int dX = (int)p2.x - (int)p1.x;
			int dY = Mathf.Abs((int)p2.y - (int)p1.y);
			int err = (dX / 2);
			int ystep = ((int)p1.y < (int)p2.y ? 1 : -1);
			int y = (int)p1.y;
			
			for (int x = (int)p1.x; x <= (int)p2.x; ++x)
			{
				if(steep)
					tex.SetPixel(y, x, Color.black);
				else
					tex.SetPixel(x, y, Color.black);
				err = err - dY;
				if (err < 0) { y += ystep;  err += dX; }
			}
			tex.Apply();
		}
	}
}