using UnityEngine;

namespace Algorithms
{
    public static class General
    {
        /// <summary>
		/// Swaps two values of the same type
		/// </summary>
		/// <param name="lhs">First value being switched</param>
		/// <param name="rhs">Second value being switched</param>
        public static void Swap<T>(ref T lhs, ref T rhs)
        {
            T temp;
            temp = lhs;
            lhs = rhs;
            rhs = temp;
        }

        public static int IsPositive(int input)
        {
            int output = input >= 0 ? 1 : -1;
            return output;
        }
    }

    public static class Bresenham
	{
		/// <summary>
		/// The plot function delegate
		/// </summary>
		/// <param name="x">The x co-ord being plotted</param>
		/// <param name="y">The y co-ord being plotted</param>
		/// <returns>True to continue, false to stop the algorithm</returns>
		public delegate bool BasicPlotFunction(int x, int y, Texture2D tex);

        /// <summary>
		/// The plot function delegate
		/// </summary>
		/// <param name="x">The x co-ord being plotted</param>
		/// <param name="y">The y co-ord being plotted</param>
		/// <returns>True to continue, false to stop the algorithm</returns>
		public delegate bool GrayScalePlotFunction(int x, int y, Texture2D tex, float alpha);

        /// <summary>
        /// Plot the line from (p1.x, p1.y) to (p2.x, p2.y)
        /// </summary>
        /// <param name="p1">The start point</param>
        /// <param name="p2">The end point</param>
        /// <param name="plot">The plotting function (if this returns false, the algorithm stops early)</param>
        /// <param name="tex">The texture the line is being added to</param>
        public static void Line(Vector2 p1, Vector2 p2, BasicPlotFunction plot, Texture2D tex)
		{
            int x1 = (int)p1.x, x2 = (int)p2.x, y1 = (int)p1.y, y2 = (int)p2.y;

            bool steep = Mathf.Abs(y2 - y1) > Mathf.Abs(x2 - x1);
			if (steep)
            {
				General.Swap(ref x1, ref y1); 
				General.Swap(ref x2, ref y2); 
			}

			if (x1 > x2) 
			{ 
				General.Swap(ref x1, ref x2); 
				General.Swap(ref y1, ref y2); 
			}

			int dX = x2 - x1;
			int dY = Mathf.Abs(y2 - y1);
			int err = (dX / 2);
			int ystep = (y1 < y2 ? 1 : -1);
			int y = y1;

            for (int x = x1; x <= x2; ++x)
            {
                if (!(steep ? plot(y, x, tex) : plot(x, y, tex))) return;
                err -= dY;
                if (err < 0)
                {
                    y += ystep;
                    err += dX;
                }
            }
		}

        public static void Circle(Vector2 center, int r, BasicPlotFunction plot, Texture2D tex)
        {
            int centerx = (int)center.x;
            int centery = (int)center.y;
            int x = -r;
            int y = 0;
            int err = 2 - 2 * r;

            while (x <= 0)
            {
                plot(centerx - x, centery + y, tex);
                plot(centerx - x, centery - y, tex);
                plot(centerx + x, centery - y, tex);
                plot(centerx + x, centery + y, tex);
                r = err;
                if (r <= y)
                    err += ++y * 2 + 1;
                if (r > x || err > y)
                    err += ++x * 2 + 1;
            } 
        }

        public static void Ellipse(Vector2 center, int width, int height, BasicPlotFunction plot, Texture2D tex)
        {
            int x1 = (int)center.x - width, x2 = (int)center.x + width, y1 = (int)center.y, y2 = (int)center.y;
            int a = width, b = height, b1 = b & 1;
            long dx = 8 * (1 - a) * b * b;
            long dy = 4 * (b1 + 1) * a * a;
            long err = dx + dy + b1 * a * a;
            long e2;

            a *= 8 * a;
            b1 = 8 * b * b;

            while(x1 <= x2)
            {
                plot(x2, y1, tex);
                plot(x1, y1, tex);
                plot(x1, y2, tex);
                plot(x2, y2, tex);
                e2 = 2 * err;
                if(e2 <= dy)
                {
                    y1++;
                    y2--;
                    err += dy += a;
                }
                if (e2 >= dx || 2 * err > dy)
                {
                    x1++;
                    x2--;
                    err += dx += b1;
                }
            }
        }

        public static void AALine(Vector2 p1, Vector2 p2, float width, GrayScalePlotFunction plot, Texture2D tex)
        {
            int x1 = (int)p1.x, y1 = (int)p1.y, x2 = (int)p2.x, y2 = (int)p2.y;
            int dx = Mathf.Abs(x2 - x1);
            int dy = Mathf.Abs(y2 - y1);
            int err = dx - dy, e2, x3, y3;
            float ed = dx + dy == 0 ? 1 : Mathf.Sqrt((float)dx * dx + (float)dy * dy);

            for(width = (width+1)/2; ; )
            {
                plot(x1, y1, tex, Mathf.Max(0, 255 * (Mathf.Abs(err - dx + dy) / ed - width + 1)));
                e2 = err;
                x3 = x1;
                if(2*e2 >= -dx)
                {
                    for (e2 += dy, y3 = y1; e2 < ed + width && (y2 != y3 || dx > dy); e2 += dx)
                        plot(x1, y3 += General.IsPositive(y2 - y1), tex, Mathf.Max(0, 255 * (Mathf.Abs(e2) / ed - width + 1)));
                    if (x1 == x2) break;
                    e2 = err;
                    err -= dy;
                    x1 += General.IsPositive(x2 - x1);
                }

                if (2 * e2 >= dy)
                {
                    for (e2 = dx - e2; e2 < ed * width && (x2 != x3 || dx < dy); e2 += dy)
                        plot(x3 += General.IsPositive(x2 - x1), y1, tex, Mathf.Max(0, 255 * (Mathf.Abs(e2) / ed - width + 1)));
                    if (y1 == y2) break;
                    err += dx;
                    y1 += General.IsPositive(y2 - y1);
                }
            }
        }
    }
}