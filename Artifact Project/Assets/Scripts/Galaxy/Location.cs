using UnityEngine;
using System.Collections;

namespace Galaxy
{
	public class Location : Object
	{
		public Vector3 coord;
		public float distance;

		public Location(float declination, float ascension, float parallax)
		{
			declination *= Mathf.PI/180;
			ascension *= Mathf.PI/2;
			Vector2 xz = new Vector2(Mathf.Cos(ascension),Mathf.Sin(ascension));
			Vector3 unit = new Vector3(xz.x*Mathf.Cos(declination),Mathf.Sin (declination),xz.y*Mathf.Cos(declination));
			if(parallax == 0)
				distance = 0;
			else
				distance = 1000/parallax;
			coord = unit * distance; 
		}

		public Vector3 Coord
		{
			get
			{
				return coord;
			}
		}
	}
}
