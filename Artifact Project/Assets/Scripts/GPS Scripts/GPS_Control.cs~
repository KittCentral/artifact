// This is the primary control script for the Astron GPS Viewer.  This scrip opens the serial port to the GPS receiver and 
// extracts the necessary information from the NEMA strings sent by the GPS receiver.
// This script creates two variables (gsv_info and rmc_info) to hold the gathered information.  GPS_Text.cs and GPS_Viewer.cs use 
// these variables.  More information about the NEMA strings can be found at http://aprs.gids.nl/nmea/.

// This script assumes:
//		that the artifact project is operating in Boulder, Colorado, in the Mountain Time Zone
//		that a functioning GPS receiver is connected to a USB port on the artifact computer
//
// This script produces:
//		gsv_info and rmc_info, which contain data from the GPS receiver

using UnityEngine;
using System.Collections;
using System.IO.Ports;

public class GPS_Control : MonoBehaviour 
{
	// declare member variables
	public SerialPort com_port = new SerialPort("COM4", 9600);	// a serial port variable 
	//public SerialPort com_port = new SerialPort("COM3", 9600);	// a serial port variable
	public string raw_data;		// holds the raw unprocessed data from the GPS receiver
	public string[] gsv_info;	// holds the data from the $GPGSV NEMA string from the GPS receiver
	public string[] rmc_info;	// holds the data from the $GPRMC NEMA string from the GPS receiver
	
	
	void Start ()
	{
		// open the serial port
		com_port.Open();
	}
	
	
	void Update () 
	{
		// import new data
		raw_data = com_port.ReadLine(); 
		//print ("inital: " + com_port.ReadLine());


		// look for satellite posiiton info
		// GSV Code:														  (from North)
		// $GPGSV, total messages, message #, sats in view, sat ID, elevation, azimuth, signal/noise,...,3 more max
		//   0   ,        1      ,    2     ,      3      ,    4  ,      5   ,    6   ,       7     ,...,8,9,10,11
		//																				   		    ,...,12,13,14,15
		//																							,...,16,17,18,19*checksum
		if(raw_data.Substring(0, 6) == "$GPGSV")
		{
			gsv_info = raw_data.Split(',');
			// DEBUG
			//for (int i = 0; i < gsv_info.Length; i++)
			//{
			//	print(gsv_info[i]);
			//}
			//print("raw data: " + raw_data);
		}
		
		
		// look for current coordinates, date, and time
		// RMC Code:						 (ddmm.mmmm)	 (dddmm.mmmm)
		// $GPRMC, time (hhmmss.sss), status, latitude, N/S, longitude, E/W, speed, bearing, date (ddmmyy), mag var, mode * checksum
		//   0   ,        1         ,    2  ,    3    ,  4 ,     5    ,  6 ,   7  ,    8   ,       9      ,    10  ,  11  *    12
		if(raw_data.Substring(0, 6) == "$GPRMC")
		{
			rmc_info = raw_data.Split(',');
			// debug
			/*
			for (int j = 0; j <= 12; j++)
			{
				print(rmc_info[j]);
			}
			*/
			//print(raw_data);
		}
	}
}