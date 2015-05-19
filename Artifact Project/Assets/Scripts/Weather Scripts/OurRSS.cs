// This script controls the weather.  It creates a weather rss object and fills that object with data from an rss feed.
// It also selects what data to display on the screen and what background is appropriate depending on the the forcast.
// The actual displaying of the weather panel and icons is done in WeatherIcons.cs and WeatherPanel.cs

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System;

public class OurRSS : MonoBehaviour 
{

	public float movement;
	public int nb2;
	public string info;
	public float rateOfMovement;
	public GameObject snowmaker1;
	public GameObject snowmaker2;
	public GameObject snowmaker3;
	public GameObject rainMaker1;
	public GameObject guiControl;
	
	rssreader rdr;
	rssreader weatherRdr;
	rssreader NOAAWeatherRdr;
	rssreader NOAAWeatherRdr2;

	public string[] temperature = new string[2];
	private string[] weatherList2 = new string[18];
	private int[] indexNumbers = new int[18];
	private Dictionary<int, string> IndexToWeather = new Dictionary<int,string > ();
	private Dictionary<string, int> weatherTableDict = new Dictionary<string, int> (); 
	GUI_Control gui_control;
	private int newBackgroundIndex;
	private int oldBackgroundIndex;
	private string weatherDescription;

	void Start () 
	{
		gui_control = guiControl.GetComponent<GUI_Control>();
		weatherRdr = new rssreader ("http://rss.weather.com/weather/rss/local/USCO0038?cm_ven=LWO&cm_cat=rss&par=LWO_rss");//Three Day Forecast
		NOAAWeatherRdr = new rssreader ("http://w1.weather.gov/xml/current_obs/KBDU.rss");//Current Conditions
		string NOAAweatherDescription = NOAAWeatherRdr.rowNews.item[0].title;
		string weatherForecast = weatherRdr.rowNews.item[6].description;
		temperature = Regex.Split(NOAAweatherDescription, @"\D+");
		print (temperature[1]);
		weatherDescription = weatherRdr.rowNews.item[6].description;
		//Set all particle systems inactive
		snowmaker1.SetActive (false);
		snowmaker2.SetActive (false);
		snowmaker3.SetActive (false);
		rainMaker1.SetActive (false);
		nb2 = 0;
		//add values to the dictionaries
		weatherTableDict ["Rain"] = 0;
		weatherTableDict ["Cloudy"] = 1;
		weatherTableDict ["Mostly Cloudy"] = 2;
		weatherTableDict ["Partly Sunny"] = 2;
		weatherTableDict ["Fair"] = 2;
		weatherTableDict ["Partly Cloudy"] = 3;
		weatherTableDict ["Mostly Sunny"] = 3;
		weatherTableDict ["Sunny"] = 4;
		weatherTableDict ["Snow"] = 5;
		weatherTableDict ["Overcast"] = 6;
		weatherTableDict ["Isolated Showers"] = 7;
		weatherTableDict ["Showers"] = 8;
		weatherTableDict ["Snow Showers"] = 9;
		weatherTableDict ["Blowing Snow"] = 10;
		weatherTableDict ["Rain and Snow"] = 11;
		weatherTableDict ["Wintery Mix"] = 12;
		weatherTableDict ["Clear"] = 13;
		weatherTableDict ["Mostly Clear"] = 13;

		weatherList2 [0] = "Cloudy";
		weatherList2 [1] = "Rain";
		weatherList2 [2] = "Sunny";
		weatherList2 [3] = "Snow";
		weatherList2 [4] = "Partly Cloudy";
		weatherList2 [5] = "Mostly Cloudy";
		weatherList2 [6] = "Partly Sunny";
		weatherList2 [7] = "Mostly Sunny";
		weatherList2 [8] = "Overcast";
		weatherList2 [9] = "Fair";
		weatherList2 [10] = "Blowing Snow";
		weatherList2 [11] = "Isolated Showers";
		weatherList2 [12] = "Showers";
		weatherList2 [13] = "Snow Showers";
		weatherList2 [14] = "Rain and Snow";
		weatherList2 [15] = "Wintery Mix";
		weatherList2 [16] = "Clear";
		weatherList2 [17] = "Mostly Clear";
		//Calculate the index number for each possible forecast

		for(int i = 0; i < weatherList2.Length; i++)
		{
			indexNumbers[i] = NOAAweatherDescription.IndexOf (weatherList2[i]);

			if(indexNumbers[i] < 0)
			{
				indexNumbers[i]=10000;
			}

			IndexToWeather[indexNumbers[i]] = weatherList2[i];

		}
		int minimum = 10000;
		//Find the minimum value of the indices in the array

		for(int i = 0; i < weatherList2.Length; i++)
		{
			if(indexNumbers[i] < minimum)
			{
				minimum = indexNumbers[i];
			}
		}
		//Ensuring that we have an actual forecast
		if (minimum != -1) 
		{

			nb2 = weatherTableDict[IndexToWeather[minimum]];
			print (nb2 + "NB2");
			if((nb2 == 5) && (gui_control.weatherBackground == true))
			{
				snowmaker1.SetActive (true);
				snowmaker2.SetActive (true);
				snowmaker3.SetActive (true);
			}

			if(nb2 == 7 && gui_control.weatherBackground == true )
			{
				rainMaker1.SetActive (true);
			}
			if(nb2 == 8 && gui_control.weatherBackground == true)//"Showers"
			{
				rainMaker1.SetActive (true);
				//Alter rate of production
			}
			if(nb2 == 9 && gui_control.weatherBackground == true)//"Snow Showers"
			{
				snowmaker1.SetActive (true);
				snowmaker2.SetActive (true);
				snowmaker3.SetActive (true);
			}
			if(nb2 == 10 && gui_control.weatherBackground == true)//"Blowing Snow"
			{
				snowmaker1.SetActive (true);
				snowmaker2.SetActive (true);
				snowmaker3.SetActive (true);
				//Need wind stuff going on here
			}
			if(nb2 == 11 && gui_control.weatherBackground == true)//"Rain and snow"
			{
				rainMaker1.SetActive (true);
				snowmaker1.SetActive (true);
			}
			if(nb2 == 12 && gui_control.weatherBackground == true)//"wintery mix"
			{
				rainMaker1.SetActive (true);
				snowmaker1.SetActive (true);
				snowmaker2.SetActive (true);
				snowmaker3.SetActive (true);
			}
		}
	}
	public void BackgroundRefresh() 
	{
		snowmaker1.SetActive (false);
		snowmaker2.SetActive (false);
		snowmaker3.SetActive (false);
		rainMaker1.SetActive (false);
		NOAAWeatherRdr2 = new rssreader ("http://w1.weather.gov/xml/current_obs/KBDU.rss");

		string NOAAweatherDescription2 = NOAAWeatherRdr2.rowNews.item[0].title;
		print (NOAAweatherDescription2);

		for(int i = 0; i < weatherList2.Length; i++)
		{
			indexNumbers[i] = NOAAweatherDescription2.IndexOf (weatherList2[i]);
			
			if(indexNumbers[i] < 0)
			{
				indexNumbers[i]=10000;
			}
			
			print (weatherList2[i]);
			
			print (indexNumbers[i]);
			
			IndexToWeather[indexNumbers[i]] = weatherList2[i];
			
		}
		int minimum = 10000;
		//Find the minimum value of the indices in the array
		for(int i = 0; i < weatherList2.Length; i++)
		{
			if(indexNumbers[i] < minimum)
			{
				minimum = indexNumbers[i];
			}
		}
		print ("Minimum: " + minimum);
		//Ensuring that we have an actual forecast
		if (minimum != -1) 
		{
			
			nb2 = weatherTableDict[IndexToWeather[minimum]];
			//print (IndexToWeather[minimum]);
			print (gui_control.weatherBackground);
			if((nb2 == 5) && (gui_control.weatherBackground == true))
			{
				snowmaker1.SetActive (true);
				snowmaker2.SetActive (true);
				snowmaker3.SetActive (true);
			}
			if(nb2 == 7 && gui_control.weatherBackground == true )
			{
				rainMaker1.SetActive (true);
			}
			if(nb2 == 8 && gui_control.weatherBackground == true)//"Showers"
			{
				rainMaker1.SetActive (true);
				//Alter rate of production
			}
			if(nb2 == 9 && gui_control.weatherBackground == true)//"Snow Showers"
			{
				snowmaker1.SetActive (true);
				snowmaker2.SetActive (true);
				snowmaker3.SetActive (true);
			}
			if(nb2 == 10 && gui_control.weatherBackground == true)//"Blowing Snow"
			{
				snowmaker1.SetActive (true);
				snowmaker2.SetActive (true);
				snowmaker3.SetActive (true);
				//Need wind stuff going on here
			}
			if(nb2 == 11 && gui_control.weatherBackground == true)//"Rain and snow"
			{
				rainMaker1.SetActive (true);
				snowmaker1.SetActive (true);
			}
			if(nb2 == 12 && gui_control.weatherBackground == true)//"wintery mix"
			{
				rainMaker1.SetActive (true);
				snowmaker1.SetActive (true);
				snowmaker2.SetActive (true);
				snowmaker3.SetActive (true);
			}

		}
//		else
//		{
//			nb2 = 0;
//		}
	}
	void Update()
	{
		//newBackgroundIndex = findWeather (weatherList2, indexNumbers, IndexToWeather, weatherTableDict);
		movement = movement + rateOfMovement * Time.deltaTime * 25;

		if (System.DateTime.UtcNow.Minute == 30 && System.DateTime.UtcNow.Second == 0)
		{
			BackgroundRefresh();
		}
		if (System.DateTime.UtcNow.Minute == 0 && System.DateTime.UtcNow.Second == 0)
		{
			BackgroundRefresh();
		}
	}



}