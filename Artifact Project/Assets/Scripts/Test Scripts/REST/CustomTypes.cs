using UnityEngine;
using System.Collections;

namespace REST
{
	public class LongLat
	{
		float longitude;
		float latitude;

		public float Longitude{get{return longitude;} set{longitude = value;}}
		public float Latitude{get{return latitude;} set{latitude = value;}}

		public LongLat(float longitudeVal, float latitudeVal)
		{
			Longitude = longitudeVal;
			Latitude = latitudeVal;
		}
	}
}