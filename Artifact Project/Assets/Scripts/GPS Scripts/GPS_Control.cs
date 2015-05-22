// This is the primary control script for the Astron GPS Viewer.  This script opens the serial port to the GPS receiver and 
// extracts the necessary information from the NEMA text strings sent by the GPS receiver.
// This script creates two variables, gsv_info and rmc_info, to hold the gathered information, and GPS_Text.cs and GPS_Viewer.cs use 
// these variables.  More information about the NEMA strings can be found at http://aprs.gids.nl/nmea/.

// This script assumes:
//		that the artifact project is operating in Boulder, Colorado, in the Mountain Time Zone
//		that a functioning GPS receiver is connected to a USB port on the artifact computer, and the necessary drivers are installed
//
// This script produces:
//		gsv_info and rmc_info, which contain strings of data from the GPS receiver
//
// NOTE: If the error "IOException: Access is denied." is thrown, then this issue is probably in the void Start() function.
//		 	See that function (around line 32) for a comment on a possible resolution.

// directives
using UnityEngine;
using System.Collections;
using System.IO.Ports;

public class GPS_Control : MonoBehaviour 
{
	// declare member variables
	public SerialPort com_port;			// variable to hold the serial port object for communicating with the GPS receiver
	private string raw_data;			// holds the raw unprocessed data from the GPS receiver
	public string[] gsv_info;			// holds the data from the $GPGSV NEMA string from the GPS receiver
	public string[] rmc_info;			// holds the data from the $GPRMC NEMA string from the GPS receiver
	
	
	void Start ()
	{
		// search for the open the serial port (USB) with the connected GPS receiver
		//		this opens every possible serial port, so if more than one USB serial device is decected, then there will be problems
		//		this code does not detect the COM port for my USB mouse, so I suspect that is only catched device which are not used 
		//		by other programs.  The code below automatically locates and sets the COM port.
		//		If the error "IOException: Access is denied." is thrown, then this issue is probably here.  You can comment out the 
		//		code below and manually set the COM port by replacing it with the following line:
		//						
		//			public SerialPort com_port = new SerialPort("XXXX", 9600)
		//
		//		where XXXX is the name of the COM port (ex: COM1, COM2, COM3, COM4).
		//		To determine the correct COM port in Windows, open the Device Manager and expand the Ports(COM & LPT) section.
		//		The serial devices are listed with their associated COM port.  The COM port number for a device is different for 
		//		EVERY computer.
		foreach(string port in SerialPort.GetPortNames())
		{
			com_port = new SerialPort(port, 9600);
			if(!com_port.IsOpen)
			{
				com_port.Open();
			}
		}
	}
	
	
	void Update () 
	{
		// search for the open the serial port (USB) with the connected GPS receiver
//		foreach(string port in SerialPort.GetPortNames())
//		{
//			com_port = new SerialPort(port, 9600);
//			if(!com_port.IsOpen)
//			{
//				print(port);
//				com_port.Open();
//			}
//		}

		// import new data
		raw_data = com_port.ReadLine(); 

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