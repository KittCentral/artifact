using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class GPS_Text : MonoBehaviour 
{
	// declare variables
	public GameObject GPS_Control;		// reference to the the GPS_Control game object for importing variables from GPS_Control.cs
	GPS_Control gps_control;			// script variable repesenting GPS_Control.cs
	private char[] error_tester;		// hold the text of an exception if it is thrown
	private bool error_thrown;			// indicates if an import error was detected
	
	public GameObject time_box;			// game object for the text box displaying current time
	public Text current_time;			// the string that is displayed in the text box
	private char[] gps_time;			// the imported time data from GPS_Control.cs
	private int int_hour0;				// first digit of the hour as an integer
	private int int_hour1;				// second digit of the hour as an integer
	private int int_hour;				// a concatenation of int_hour0 and int_hour1, makes the math easier with 1 variable
	private int int_minute0;			// first digit of the minute as an integer
	private int int_minute1;			// second digit of the minute as an integer
	private int int_second0;			// first digit of the second as an integer
	private int int_second1;			// second digit of the second as an integer
	private string string_hour;			// string representation of int_hour
	private string string_minute0;		// first digit of the minute as a string
	private string string_minute1;		// second digit of the minute as a string
	private string string_second0;		// first digit of the second as a string
	private string string_second1;		// second digit of the second as a string
	private string final_time;			// time to be displayed after all formatting and adjustments
	private bool dst = false;			// true if currently daylight savings time, otherwise false
	//		based on the date and time zone from the operating system
	
	public GameObject coordinate_box;	// game object for the text box displaying the current time
	public Text current_coordinates;	// the sting that is displayed in the text box
	private string raw_latitude;		// latitude as received from the GPS receiver
	private string latitiude_direction;	// latitude north or south of equator with "N" or "S" 
	private string raw_longitude;		// longitue as received from the GPS reveiver
	private string longitude_direction;	// longitude east or west of Prime Meridian, either "E" or "W"
	private string latitude_whole0;		// 1st digit (10's place) of the latitiude
	private string latitude_whole1;		// 2nd digit (1's place) of the latitiude
	private string latitude_decimal0;	// 1st decimal (i.e. 0.0) of the latitiude
	private string latitude_decimal1;	// 2nd decimal (i.e. 0.00) of the latitiude
	private string latitude_decimal2;	// 3rd decimal (i.e. 0.000) of the latitiude
	private string latitude_decimal3;	// 4th decimal (i.e. 0.0000) of the latitiude
	private string longitude_whole0;	// 1st digit (100's place) of the longitude
	private string longitude_whole1;	// 2nd digit (10's place) of the longitude
	private string longitude_whole2;	// 3rd digit (1's place) of the longitude
	private string longitude_decimal0;	// 1st decimal (i.e. 0.0) of the longitude
	private string longitude_decimal1;	// 2nd decimal (i.e. 0.00) of the longitude
	private string longitude_decimal2;	// 3rd decimal (i.e. 0.000) of the longitude
	private string longitude_decimal3;	// 4th decimal (i.e. 0.0000) of the longitude
	private string final_coordinates;	// the completed text string containing the current coordiates
	
	
	void Start ()
	{
		// import the GPS data from GPS_Control.cs
		gps_control = GPS_Control.GetComponent<GPS_Control>();
		// DEBUG
		//print(gps_control.gsv_info[0]);
		//print(gps_control.rmc_info[0]);
		
		// initalize the current time
		current_time = time_box.GetComponent<Text>();
		current_time.text = "No Time Data";
		
		// initalize the current coordinates
		current_coordinates = coordinate_box.GetComponent<Text>();
		current_coordinates.text = "No Coordinate Data";
		
		// initalize error tracker
		error_thrown = false;
	}
	
	
	void Update () 
	{
		// check for import errors
		try
		{
			error_tester = gps_control.rmc_info[1].ToCharArray();
		}
		catch (System.IndexOutOfRangeException)
		{
			error_thrown = true;
		}
		// DEBUG
		//print ("Passed GPS_Text");
		
		// only execute if no import errors
		if (error_thrown == false)
		{
			// display current time
			gps_time = gps_control.rmc_info[1].ToCharArray();
			// DEBUG
			//print (gps_control.rmc_info[1]);
			
			// convert to integers
			int_hour0 = (int)Char.GetNumericValue(gps_time[0]);
			int_hour1 = (int)Char.GetNumericValue(gps_time[1]);
			int_hour = (int_hour0 * 10) + int_hour1;
			int_minute0 = (int)Char.GetNumericValue(gps_time[2]);
			int_minute1 = (int)Char.GetNumericValue(gps_time[3]);
			int_second0 = (int)Char.GetNumericValue(gps_time[4]);
			int_second1 = (int)Char.GetNumericValue(gps_time[5]);
			
			// adjust hours to local time in Boulder, Colorado
			dst = DateTime.Now.IsDaylightSavingTime();
			if (dst == true)
			{
				//subtract 6 hours
				if ( (int_hour - 6) < 0)
				{
					int_hour = 24 - Math.Abs(int_hour - 6);
				}
				else  // (int_hour - 6) >= 0
				{
					int_hour = int_hour - 6;
				}
			}
			else // dst = false
			{
				//subtract 7 hours
				if ( (int_hour - 7) < 0)
				{
					int_hour = 24 - Math.Abs(int_hour - 7);
				}
				else  // (int_hour - 7) >= 0
				{
					int_hour = int_hour - 7;
				}
			}
			
			// convert to char array
			string_hour = Convert.ToString(int_hour);
			string_minute0 = Convert.ToString(int_minute0);
			string_minute1 = Convert.ToString(int_minute1);
			string_second0 = Convert.ToString(int_second0);
			string_second1 = Convert.ToString(int_second1);
			
			final_time = string_hour + ":" + string_minute0 + string_minute1 + ":" + string_second0 + string_second1;
			current_time.text = final_time;
			
			// display current coordinates
			raw_latitude = gps_control.rmc_info[3];
			latitiude_direction = gps_control.rmc_info[4];
			raw_longitude = gps_control.rmc_info[5];
			longitude_direction = gps_control.rmc_info[6];
			
			latitude_whole0 = Convert.ToString(raw_latitude[0]);
			latitude_whole1 = Convert.ToString(raw_latitude[1]);
			latitude_decimal0 = Convert.ToString(raw_latitude[2]);
			latitude_decimal1 = Convert.ToString(raw_latitude[3]);
			latitude_decimal2 = Convert.ToString(raw_latitude[5]); // raw_latitiude[4] = "." and that is not needed
			latitude_decimal3 = Convert.ToString(raw_latitude[6]);
			
			longitude_whole0 = Convert.ToString(raw_longitude[0]);
			longitude_whole1 = Convert.ToString(raw_longitude[1]);
			longitude_whole2 = Convert.ToString(raw_longitude[2]);
			longitude_decimal0 = Convert.ToString(raw_longitude[3]);
			longitude_decimal1 = Convert.ToString(raw_longitude[4]);
			longitude_decimal2 = Convert.ToString(raw_longitude[6]); // raw_longitude[5] = "." and that is not needed
			longitude_decimal3 = Convert.ToString(raw_longitude[7]);
			
			final_coordinates = latitude_whole0 + latitude_whole1 + "." + latitude_decimal0 + latitude_decimal1 + latitude_decimal2 
				+ latitude_decimal3 + " " + latitiude_direction + "\n" + longitude_whole0 + longitude_whole1 + longitude_whole2 
					+ "." + longitude_decimal0 + longitude_decimal1 + longitude_decimal2 + longitude_decimal3 + " " + longitude_direction;
			current_coordinates.text = final_coordinates;
		}
		
		// reset error tracker
		error_thrown = false;
	}
}


