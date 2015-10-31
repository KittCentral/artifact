using UnityEngine;
using System.Collections;

namespace Algorithms
{
	namespace CustomTypes
	{
		[System.Serializable]
		public struct IntVector2
		{
			public int x, y;

			public IntVector2 (int x, int y)
			{
				this.x = x;
				this.y = y;
			}

			public static IntVector2 operator + (IntVector2 a, IntVector2 b)
			{
				a.x += b.x;
				a.y += b.y;
				return a;
			}

			public static IntVector2 operator - (IntVector2 a, IntVector2 b)
			{
				a.x -= b.x;
				a.y -= b.y;
				return a;
			}
		}

		[System.Serializable]
		public struct IntVector3
		{
			public int x, y, z;
			
			public IntVector3 (int x, int y, int z)
			{
				this.x = x;
				this.y = y;
				this.z = z;
			}
			
			public static IntVector3 operator + (IntVector3 a, IntVector3 b)
			{
				a.x += b.x;
				a.y += b.y;
				a.z += b.z;
				return a;
			}
			
			public static IntVector3 operator - (IntVector3 a, IntVector3 b)
			{
				a.x -= b.x;
				a.y -= b.y;
				a.z -= b.z;
				return a;
			}
		}

		public enum Direction {North, East, South, West}
	}
}
