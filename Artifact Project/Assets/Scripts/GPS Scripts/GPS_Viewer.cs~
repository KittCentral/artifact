// This script calculates the postion of the GPS satellites in the game's world coordinate system and updates their postion.
// The following websites are useful for debugging:
// 		http://en.wikipedia.org/wiki/List_of_GPS_satellites 	(the satillite ID's from the GPS receiver are the PRN numbers)
//		http://www.nstb.tc.faa.gov/Full_WaasSatelliteStatus.htm	(shows the GPS satellites overhead in real time by PRN number)
//		https://www.wolframalpha.com/input/?i=Navstar+57		(wolframalpha can give you the latitude and longitude of a satellite
//															//   	however, it needs the space vehical name (SVN) not the PRN number
//															//		The wikipedia page above shows the name of each spacecraft and PRN number) 
//
// This script expects:
//		gsv_info and rmc_info variables from GPS_Control.cs, which contain data from the GPS receiver
//		a prefab of a satellite model to be attached to this script in the Unity Inspector

// This script assumes:
//		that the artifact project is operating in Boulder, Colorado, in the Mountain Time Zone
//		that a functioning GPS receiver is connected to a USB port on the artifact computer, and the necessary drivers are installed
// 		north latitude (above Equator) = positive, east longitude (east of Prime Meridian) = positive
//
// This script produces:
//		GPS satellite models on the screen whose position cooresponts of the actual GPS satellites overhead
//		the GPS satellite models update in real time

// directives
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
namespace AssemblyCSharp
{
	public class GPS_Viewer : MonoBehaviour
	{
		// declare variables
		public GameObject GPS_Control;			// reference to the GPS_Control game object for importing variables from GPS_Control.cs
		GPS_Control gps_control;				// script variable repesenting GPS_Control.cs
		public GameObject gps_satellite_model;	// game object for the 3D model of the GPS satellite, model made by Tommy Mueller 
		//										//   	and retreived from https://grabcad.com/library/navstar-gps-space-satellite-life-size
		public double elevation;				// elevation of the satellite above the horizon as seen by the GPS receiver in degrees
		public double azimuth;					// azimuth of the satellite clockwise from north as seen by the GPS receiver in degrees
		public string satellites_tracked;		// number of satellites seen by the GPS receiver
		public string error_tester;
		public bool error_thrown;
		
		// delcare a data strucrture (a sort of dynamic list) to hold the currently tracked satellites
		public Dictionary<int, satellite> satellite_list = new Dictionary<int, satellite>();	// dynamic list of currently tracked satellites
		public int key;			// a unique ID for each satellite, which is the PRN number
		public satellite sat;	// a variable of the satellite class defined below
		
		
		void Start()
		{
			// import the GPS data from GPS_Control.cs
			gps_control = GPS_Control.GetComponent<GPS_Control>();
			// DEBUG
			//print(gps_control.gsv_info[0]);
			//print(gps_control.rmc_info[0]);
			error_thrown = false;
		}
		
		
		void FixedUpdate()	// runs every 0.02 seconds
		{
			// check for import errors
			try
			{
				// DEBUG
				//error_tester = gps_control.gsv_info[16];
				//error_tester = gps_control.gsv_info[12];
				//error_tester = gps_control.gsv_info[8];
				//error_tester = gps_control.gsv_info[4];
				//error_tester = gps_control.gsv_info[3];
			}
			catch (System.IndexOutOfRangeException)
			{
				error_thrown = true;
			}
			// DEBUG
			//print ("Passed GPS_Viewer");
			//print (error_thrown);
			
			if(error_thrown == false)
			{
				// set each satellite's position_update member to false
				foreach(KeyValuePair <int, satellite> entry in satellite_list)
				{
					sat.position_updated = false;
				}
				
				// update satellite list (add new entries and recalculate every satellite's position)
				for(int i = 0; i < 4; ++i)	// there are no more than 4 satellites per line of imported text
				{
					// update variables
					key = Convert.ToInt32( gps_control.gsv_info[(4 + i*4)] );
					//print ("top " + key.ToString());
					elevation = Convert.ToDouble( gps_control.gsv_info[(5 + i*4)] );  // FIX conversion throws FormatException error
					azimuth = Convert.ToDouble( gps_control.gsv_info[(6 + i*4)] );
					
					// update the satellite dictionary (place models, calculate position, and set members)
					if(satellite_list.ContainsKey(key) == true)
					{
						// model is already instantiated...so update position and set members
						satellite_list.TryGetValue(key, out sat);
						sat.instantiated = true;
						sat.satellite_position.position = calculate_position(elevation, azimuth);
						sat.position_updated = true;
					}
					else  // satellite_list.ContainsKey(key) == false
					{
						// model is not instantiated...so add to dictionary, instantiate, then update position and set members
						satellite_list.Add(key, new satellite(gps_control, key, gps_satellite_model));
						satellite_list.TryGetValue(key, out sat);
						sat.instantiated = true;
						sat.satellite_position.position = calculate_position(elevation, azimuth);
						sat.position_updated = true;
					}
					//print ("end");
				}
				
				// FIX it does not appear to delete the old models...
				// remove old satellite entries from dictionary
				foreach(KeyValuePair <int, satellite> entry in satellite_list)
				{
					// iterate through entire dictionary, if updated = false, then destroy satellite model and delete entry
					if(sat.position_updated == false)
					{
						Destroy(sat.satellite_instance);
						satellite_list.Remove(entry.Key);
					}
				}
				// DEBUG
				//print(satellite_list.Count);

				// get number of tracked satellites
				satellites_tracked =  gps_control.gsv_info[3];
			}
			
			// reset error tracker
			error_thrown = false;
		}
//---------------------------------------------------------------------------------------------------------------------------------------		
//---------------------------------------------------------------------------------------------------------------------------------------
// Subfunctions

		private double degree_to_radian(double angle)
		{
			// this function converts a given angle from degrees to radians
			return ((Math.PI) * (angle / 180.0));
		}

		private double radian_to_degree(double angle)
		{
			// this function converts a given angle from radians to degrees
			return ((angle) * (180.0 / Math.PI));
		}
		
		private double magnitude(double first, double second, double third)
		{
			// this function returns the magniture of a vector based on the pythagorean theorem in cartesian coordinates
			return ( Math.Sqrt( (first * first) + (second * second) + (third * third) ) );
		}

		private double[] matrix_multiply(double[][] rotation_matrix, double[] vector)
		{
			// this function multiplies a 3 by 3 matrix with a 3 by 1 vector
			// the equation is: [rotation_matrix]*[vector] = [result]

			// declare variables
			double[] result = new double[3];	// a 3 by 1 vector which is the answer that is returned by this function
			double sum;							// a temporary variable which is used in the dot product multiplication of the elements


			// loop through the rows of rotation_matrix ( i = row, j = column)
			for(int i = 0; i <= 2; i++)
			{
				// loop through the rows of vector
				sum = 0;
				for(int j = 0; j <= 2; j++)
				{
					// each element of the result is the dot product of a row of rotation_matrix and the all of vector
					sum += rotation_matrix[i][j] * vector[j];
				}
				result[i] = sum;
			}
			return result;
		}

		public Vector3 calculate_position(double elevation, double azimuth)
		{
			// declare constants (and other "variables" that will always be constant)
			const double EARTH_RADIUS = 6371;					// km from center of Earth, from wikipedia: http://en.wikipedia.org/wiki/Earth
			const double SATELLITE_RADIUS = 20200;				// km above Earth's surface, from gps.gov: http://www.gps.gov/systems/gps/space/
			const double SCALE_FACTOR = 1/(2*EARTH_RADIUS);		// scale factor from universal coordinates to game coordinates = 0.00007848061529 
			const double MU = 1.306242809;						// longitude between Boulder and 180 W (= 74.8422 degrees)  
			double BOULDER_LAT = degree_to_radian(40.0015);		// latitude of Boulder, Colorado in radians
			double BOULDER_LONG = degree_to_radian(15.1578);	// longitude of Boudler, Colorado in radians
			//													//		-105.1578 W or 254.822 E, which is 15 deg when the angle is reduced to 
			//													//		the nearest axis (x-axis)
			double PI_TWO = Math.PI / 2;						// shorter way to write PI/2, it helps make the formulas easier to read


			// declare variables
			double boulder_x, boulder_y, boulder_z;				// position of Boulder in wold coordinates
			double range;										// distance from the GPS receiver to the satellite in km
			double[] position_prime = new double[3];			// GPS receiver's coordinates converted to a local cartesian system 
			//													//		(which does not align with the world's cartesian coordinate system, yet)
			double[] temporary_matrix = new double[3];			// temporary matrix used to store the result of a matrix transformation
			double angle_z = Math.PI/2 - BOULDER_LAT;			// how much the coordinates of the satellite need to be rotated about the z axis in radians
			double[][] rotation_matrix_z = new double[][]		// matrix for rotation about the z axis
			{
				new double[] {Math.Cos(angle_z), -1 * Math.Sin(angle_z), 0}, 
				new double[] {Math.Sin(angle_z), Math.Cos(angle_z),		 0}, 
				new double[] {0, 				 0, 					 1}
			};
			double angle_y = Math.PI/2 - MU;					// how much the coordinates of the satellite need to be rotated about the y axis in radians
			double[][] rotation_matrix_y = new double[][]		// matrix for rotation about the y axis
			{
				new double[] {Math.Cos(angle_y), 	  0, Math.Sin(angle_y)},
				new double[] {0,				 	  1, 0},
				new double[] {-1 * Math.Sin(angle_y), 0, Math.Cos(angle_y)}
			};
			double[] satellite_coordinates = new double[3];		// the coordinates of the satellite in world coordinates, this is the result of the matrix operations
			double x, y, z;										// final postion of the GPS satellite in world coordiantes
			float x_float, y_float, z_float;					// final postion of the GPS satellite in world coordiantes converted to a float


			// convert the elevation and azimuth angles from degrees to radians
			elevation = degree_to_radian(elevation);
			azimuth = degree_to_radian(azimuth);

			// convert spherical coordinates of Boulder (from GPS coordinates) to cartesian coordinates (left handed system)
			boulder_x = -1 * EARTH_RADIUS * Math.Cos(BOULDER_LAT) * Math.Sin(MU);
			boulder_y = EARTH_RADIUS * Math.Sin(BOULDER_LAT);
			boulder_z = EARTH_RADIUS * Math.Cos(BOULDER_LAT) * Math.Cos(MU);

			// calculate the range from GPS receiver to satellite
			range = (EARTH_RADIUS * Math.Cos(PI_TWO + elevation)) + Math.Sqrt( (EARTH_RADIUS * EARTH_RADIUS * Math.Cos(PI_TWO + elevation) * Math.Cos(PI_TWO + elevation)) + 
			                                                              ((2 * EARTH_RADIUS * SATELLITE_RADIUS) + (SATELLITE_RADIUS * SATELLITE_RADIUS)) );

			// convert spherical coordinates of the GPS receiver (horizon coorinates with elevation and azimuth) to cartesian coordinates (left handed system)
			position_prime[0]  = range * Math.Cos(elevation) * Math.Cos(azimuth);
			position_prime[1]  = range * Math.Sin(elevation);
			position_prime[2]  = -1 * range * Math.Cos(elevation) * Math.Sin(azimuth);

			// align the GPS receiver's cartesian coordinate system with the Earth's cartesian coordinate system by applying transformation matricies
			temporary_matrix = matrix_multiply(rotation_matrix_z, position_prime);
			satellite_coordinates =  matrix_multiply(rotation_matrix_y, temporary_matrix);

			// add the satellite vector's components to the components of the vector for Boulder's location
			x = boulder_x + satellite_coordinates[0];
			y = boulder_x + satellite_coordinates[1];
			z = boulder_x + satellite_coordinates[2];

			// scale values to game coordinates and convert to floats
			x_float = Convert.ToSingle(x * SCALE_FACTOR);
			y_float = Convert.ToSingle(y * SCALE_FACTOR);
			z_float = Convert.ToSingle(z * SCALE_FACTOR);

			// DEBUG
			//print(x + " " + y + " " + z);
			//print(x_float + " " + y_float + " " + z_float);


			// DEBUG
			// convert back to spherical coordiantes to check for accuracy against http://www.nstb.tc.faa.gov/Full_WaasSatelliteStatus.htm
			// this spherical coordinate system used the rho, theta, phi convention from mathematics, not the ISO one used in physics
				// declare variables
			/*
			 	double rho, theta, phi;
				double lat;// lon;
				
				// calculate variables
				rho = Math.Sqrt(x * x + y * y + z * z);
				theta = Math.Atan(z / x);
				
				// calculate latitude
				lat = -9999;
				if (y >= 0)
				{
					phi = Math.Acos(y / rho);
					lat = radian_to_degree( (Math.PI/2) - phi );
				}
				if (y < 0)
				{
					phi = Math.Acos(Math.Abs(y) / rho);
					lat = radian_to_degree( (Math.PI/2) - phi );
					lat = -1 * lat;
				}

			/*
				// calculate longitude
				lon = -9999;
				if (theta > 0)
				{
					lon = radian_to_degree( (Math.PI/2 - theta) );
					lon = -1 * lon;
				}
				if (theta <= 0)
				{
					lon = radian_to_degree( (theta + Math.PI/2) );
					lon = -1 * lon;
				}
				print (lat.ToString() + " " + lon.ToString());
			*/
			// end debug

			// return the satellite's coordinates in the game's left handed coordinate system
			return (new Vector3(x_float, y_float, z_float));
		}

//---------------------------------------------------------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------------------------------------------------------
// Sub-class

		public class satellite
		{
			// This class represents the satellite on the screen.  It tracks each satellite's unique ID, position, and the 3D model
			// of the satellite that is displayed on the screen.

			// declare members
			public int id;							// the satellite's unique ID, also known as it PRN number
			public bool instantiated;				// true if the satellite model has been drawn on the screen, otherwise false
			public bool position_updated;			// true if the satellite's position has been updated 
			public GameObject satellite_instance;	// game opjece to hold the model of the satellite
			public Transform satellite_position;	// hold the coordinates of the satellite's position for each satellite_instance
			
			
			// constructor function
			public satellite(GPS_Control gps_control, int key, GameObject gps_satellite_model)
			{
				// set all membeer variables to their default values
				id = key;
				satellite_instance = (GameObject) Instantiate(gps_satellite_model);
				satellite_position = satellite_instance.GetComponent<Transform> ();
				satellite_position.position = new Vector3(0, 0, -1);
				instantiated = false;
				position_updated = false;
			}
		}
	}
}