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
		//										   and retreived from https://grabcad.com/library/navstar-gps-space-satellite-life-size
		public double elevation;
		public double azimuth;
		public string satellites_tracked;
		public string error_tester;
		public bool error_thrown;
		
		// delcare a data strucrture (some sort of dynamic list) to hold the currently tracked satellites
		public Dictionary<int, satellite> satellite_list = new Dictionary<int, satellite>();
		public int key;
		public satellite sat;
		
		
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
				for(int i = 0; i < 4; ++i)	// there are no more than 4 satellites per line of imported test
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
						//print (key);
						sat.satellite_position.position = calculate_position(elevation, azimuth);
						sat.position_updated = true;
					}
					else  // satellite_list.ContainsKey(key) == false
					{
						// model is not instantiated...so add to dictionary then update position and members
						satellite_list.Add(key, new satellite(gps_control, key, gps_satellite_model));
						satellite_list.TryGetValue(key, out sat);
						sat.instantiated = true;
						//print (key);
						sat.satellite_position.position = calculate_position(elevation, azimuth);
						sat.position_updated = true;
					}
					//print ("end");
					
				}
				
				// FIX it does not appear to delete the old models...
				// remove old satellite entries from dictionary
				foreach(KeyValuePair <int, satellite> entry in satellite_list)
				{
					// iterate through entire dictionary, if updated = false, then kill sat model and delete entry
					if(sat.position_updated == false)
					{
						Destroy(sat.satellite_instance);
						satellite_list.Remove(entry.Key);
					}
				}
				
				// get number of tracked satellites
				satellites_tracked =  gps_control.gsv_info[3];
				//print("Satellites Tracked = " + satellites_tracked);
				//print(satellite_list.Count);
			}
			
			// reset error tracker
			error_thrown = false;
			
		}
		
		
		//------------------------------------------------------------------------------------------------------------------------------------
		private double degree_to_radian(double angle)
		{
			return ((Math.PI) * (angle / 180.0));
		}

		private double radian_to_degree(double angle)
		{
			return ((angle) * (180.0 / Math.PI));
		}
		
		private double magnitude(double first, double second, double third)
		{
			return ( Math.Sqrt( (first * first) + (second * second) + (third * third) ) );
		}

//		private double[] rotate_y(double[] vector, double angle)
//		{
//			double[][] rotation_matrix_y = new double[][]
//			{
//				new double[] {Math.Cos(angle)}, 	 new double[] {0}, new double[] {Math.Sin(angle)},
//				new double[] {0},					 new double[] {1}, new double[] {0},
//				new double[] {-1 * Math.Sin(angle)}, new double[] {0}, new double[] {Math.Cos(angle)}
//			};
//			print ("hit y");
//			return matrix_multiply(rotation_matrix_y, vector);
//		}
//
//		private double[] rotate_z(double[] vector, double angle)
//		{
//			double[][] rotation_matrix_z = new double[][]
//			{
//				new double[] {Math.Cos(angle)}, new double[] {-1 * Math.Sin(angle)}, new double[] {0}, 
//				new double[] {Math.Sin(angle)}, new double[] {Math.Cos(angle)}, 	 new double[] {0}, 
//				new double[] {0}, 				new double[] {0}, 					 new double[] {1}
//			};
//			print ("hit z");
//			return matrix_multiply(rotation_matrix_z, vector);
//		}

		private double[] matrix_multiply(double[][] rotation_matrix, double[] vector)
		{
			double[] result = new double[3];
			double sum;
			for(int i = 0; i <= 2; i++)
			{
				sum = 0;
				for(int j = 0; j <= 2; j++)
				{
					sum += rotation_matrix[i][j] * vector[j];
				}
				result[i] = sum;
			}
			return result;
		}

		public Vector3 calculate_position(double elevation, double azimuth)
		{
			// declare variables
			//double alpha, beta, delta, epsilon, eta, kappa, lambda, tau, psi, omega;
			//double q, dist1, double d;
			double boulder_x, boulder_y, boulder_z;
			//double north_pole_x, north_pole_y,north_pole_z;
			//double pacific_x, pacific_y, pacific_z;
			double x, y, z;
			//double x_prime, y_prime, z_prime;
			float x_float, y_float, z_float;
			
			// declare constants (and other things that will always be constant) and convert to radians
			const double EARTH_RADIUS = 6371;					// km from center of Earth, from wikipedia: http://en.wikipedia.org/wiki/Earth
			const double SATELLITE_RADIUS = 20200;				// km above Earth's surface, from gps.gov: http://www.gps.gov/systems/gps/space/
			const double SCALE_FACTOR = 1/(2*EARTH_RADIUS);		// scale factor from universal coordinates to game coordinates = 0.00007848061529 
			double BOULDER_LAT = degree_to_radian(40.0015);
			double BOULDER_LONG = degree_to_radian(15.1578);	// or -105.1578 W or 254.822 E, which is 15 deg when the angle is reduced to the nearest axis (x-axis))
			//const double NORTH_POLE_LAT = Math.PI/2;			// North  = positive
			//const double NORTH_POLE_LONG = 0;					// East = positive
			//double PACIFIC_LAT = degree_to_radian(40);
			//const double PACIFIC_LONG = Math.PI;
			double PI_TWO = Math.PI / 2;

			const double MU = 1.306242809;	// or 74.8422 degrees longitude between Boulder and 180 W 
			double range;
			double[] position_prime = new double[3];



			// convert the elevation and azimuth angles from degrees to radians
			//print("elevation " + elevation.ToString());
			//print("azimuth " + azimuth.ToString());
			elevation = degree_to_radian(elevation);
			azimuth = degree_to_radian(azimuth);

			// convert spherical coordinates of Boulder (from GPS coordinates) to cartesian coordinates (left handed system)
			boulder_x = -1 * EARTH_RADIUS * Math.Cos(BOULDER_LAT) * Math.Sin(MU);
			boulder_y = EARTH_RADIUS * Math.Sin(BOULDER_LAT);
			boulder_z = EARTH_RADIUS * Math.Cos(BOULDER_LAT) * Math.Cos(MU);
			//print( (boulder_x*SCALE_FACTOR).ToString() + " " + (boulder_y*SCALE_FACTOR).ToString() + " " + (boulder_z*SCALE_FACTOR).ToString());

			// calculate the range from GPS receiver to satellite
			range = (EARTH_RADIUS * Math.Cos(PI_TWO + elevation)) + Math.Sqrt( (EARTH_RADIUS * EARTH_RADIUS * Math.Cos(PI_TWO + elevation) * Math.Cos(PI_TWO + elevation)) + 
			                                                              ((2 * EARTH_RADIUS * SATELLITE_RADIUS) + (SATELLITE_RADIUS * SATELLITE_RADIUS)) );

			// convert spherical coordinates of the GPS receiver (horizon coorinate with elevation and azimuth) to cartesian coordinates (left handed system)
			position_prime[0]  = range * Math.Cos(elevation) * Math.Cos(azimuth);
			position_prime[1]  = range * Math.Sin(elevation);
			position_prime[2]  = -1 * range * Math.Cos(elevation) * Math.Sin(azimuth);
			//print( (x_prime*SCALE_FACTOR).ToString() + " " + (y_prime*SCALE_FACTOR).ToString() + " " + (z_prime*SCALE_FACTOR).ToString() );

			// align the GPS receiver's coordinate system with the Earth's coordinate system by applying transformation matricies


			double[] temporaty_matrix = new double[3];
			double angle_z = Math.PI/2 - BOULDER_LAT;
			double[][] rotation_matrix_z = new double[][]
			{
				new double[] {Math.Cos(angle_z), -1 * Math.Sin(angle_z), 0}, 
				new double[] {Math.Sin(angle_z), Math.Cos(angle_z),		 0}, 
				new double[] {0, 				 0, 					 1}
			};
			//print ("hit z");
			temporaty_matrix = matrix_multiply(rotation_matrix_z, position_prime);

			double angle_y = Math.PI/2 - MU;
			double[][] rotation_matrix_y = new double[][]
			{
				new double[] {Math.Cos(angle_y), 	  0, Math.Sin(angle_y)},
				new double[] {0,				 	  1, 0},
				new double[] {-1 * Math.Sin(angle_y), 0, Math.Cos(angle_y)}
			};
			//print ("hit y");
			double[] satellite_coordinates = new double[3];
			satellite_coordinates =  matrix_multiply(rotation_matrix_y, temporaty_matrix);


//			double[] temporaty_matrix = new double[3];
//			double[] satelite_coordinates = new double[3];
//			temporaty_matrix = rotate_z(position_prime, (Math.PI/2 - BOULDER_LAT));
//			satelite_coordinates = rotate_y(temporaty_matrix, (Math.PI/2 - MU));

			// add the satellite vector's components to the components of the vector for Boulder's location
			x = boulder_x + satellite_coordinates[0];
			y = boulder_x + satellite_coordinates[1];
			z = boulder_x + satellite_coordinates[2];

			// scale values to game coordinates and convert to floats
			float[] final_coordinates_float = new float[3];
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
		
		
		public class satellite
		{
			// declare variables
			public int id;
			public bool instantiated;
			public bool position_updated;
			public GameObject satellite_instance;
			public Transform satellite_position;
			
			
			// constructor
			public satellite(GPS_Control gps_control, int key, GameObject gps_satellite_model)
			{
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

//------------------------------------------------------------------------------------------------------------------------------------

//// test coordinates
// 07,73,007
// 21,19,4
//float te = .55F;
//satellite = (GameObject) Instantiate(gps_satellite_model, satellite_position, Quaternion.identity);  
//satellite_position = new Vector3(0, te, 1);

//
//boulder_x = EARTH_RADIUS * Math.Cos(BOULDER_LAT) * Math.Cos(BOULDER_LONG);
//boulder_y = EARTH_RADIUS * Math.Cos(BOULDER_LAT) * Math.Sin(BOULDER_LONG);
//boulder_z = EARTH_RADIUS * Math.Sin(BOULDER_LAT);
//north_pole_x = EARTH_RADIUS * Math.Cos(NORTH_POLE_LAT) * Math.Cos(NORTH_POLE_LONG);
//north_pole_y = EARTH_RADIUS * Math.Cos(NORTH_POLE_LAT) * Math.Sin(NORTH_POLE_LONG);
//north_pole_z = EARTH_RADIUS * Math.Sin(NORTH_POLE_LAT);
//pacific_x = EARTH_RADIUS * Math.Cos(PACIFIC_LAT) * Math.Cos(PACIFIC_LONG);
//pacific_y = EARTH_RADIUS * Math.Cos(PACIFIC_LAT) * Math.Sin(PACIFIC_LONG);
//pacific_z = EARTH_RADIUS * Math.Sin(PACIFIC_LAT);
