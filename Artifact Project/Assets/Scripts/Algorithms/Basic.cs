using UnityEngine;
using System.Collections;

namespace Algorithms
{
	public static class Basic
	{
		///<summary>
		///Returns whether or not the input is between the two bounds
		///</summary>
		public static bool IsBetween(int input, int lower, int upper, bool inclusive = true)
		{
			if(lower > upper)
			{
				int temp = upper;
				upper = lower;
				lower = temp;
			}
			if(inclusive == true)
			{
				if(input >= lower && input <= upper)
					return true;
				else
					return false;
			}
			else
			{
				if(input > lower && input < upper)
					return true;
				else
					return false;
			}
		}
	}
}