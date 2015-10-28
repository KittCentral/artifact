using UnityEngine;
using System.Collections;
using Chess;

namespace Algorithms
{
	namespace ExtensionMethods
	{
		public static class ExtensionMethods
		{
			public static BoardPosition ToBoardPosition(this string str)
			{
				if(str.Length == 2)
				{
					int x = str.ToCharArray()[0].ToBoardCoordinate();
					int y = str.ToCharArray()[1].ToBoardCoordinate();
					return new BoardPosition(x,y);
				}
				else
					return null;
			}

			public static int ToBoardCoordinate(this char c)
			{
				if(c == 'a' || c == 'A' || c == '1')
					return 0;
				else if(c == 'b' || c == 'B' || c == '2')
					return 1;
				else if(c == 'c' || c == 'C' || c == '3')
					return 2;
				else if(c == 'd' || c == 'D' || c == '4')
					return 3;
				else if(c == 'e' || c == 'E' || c == '5')
					return 4;
				else if(c == 'f' || c == 'F' || c == '6')
					return 5;
				else if(c == 'g' || c == 'G' || c == '7')
					return 6;
				else if(c == 'h' || c == 'H' || c == '8')
					return 7;
				else
					return -1;
			}
		}
	}
}