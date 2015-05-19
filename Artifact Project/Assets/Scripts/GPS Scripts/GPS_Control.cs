using UnityEngine;
using System.Collections;
using System.IO.Ports;

public class GPS_Control : MonoBehaviour 
{
	// declare member variables
	public SerialPort com_port = new SerialPort("COM4", 9600);
	//public SerialPort com_port = new SerialPort("COM3", 9600);
	public string raw_data;
	public string[] gsv_info;
	public string[] rmc_info;
	
	
	void Start ()
	{
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