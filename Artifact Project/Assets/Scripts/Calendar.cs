// This script controls everything for the calendar section of the progam.
// It is called when the Calendar Scene is loaded.

using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

public class Calendar : MonoBehaviour 
{
	//Initialize the day squares
	public GameObject[] eventObject = new GameObject[42];
	public GameObject[] cubicles = new GameObject[42];
	private Text[] eventText = new Text[42];
	private Text[] dateNum = new Text[42];

	//Further Initialization
	public GameObject MonthObj;
	private Text monthText;
	private DateTime now;
	DateTime firstDayOfMonth;
	int numOfWeek;
	int daysInMonth;
	public int eventFontSize;

	private DayOfWeek[] dayList = new DayOfWeek[7]{DayOfWeek.Monday,DayOfWeek.Tuesday,DayOfWeek.Wednesday,DayOfWeek.Thursday,DayOfWeek.Friday,DayOfWeek.Saturday,DayOfWeek.Sunday};
	private string[] monthList = new string[12]{"January","February","March","April","May","June","July","August","September","October","November","December"};
	
	void Start()
	{
		for(int i=0; i<42; i++)
		{
			dateNum[i] = cubicles[i].GetComponent<Text>();
		}
		for(int i=0; i<42; i++)
		{
			eventText[i] = eventObject[i].GetComponent<Text>();
		}
		monthText = MonthObj.GetComponent<Text>();
		now = DateTime.Now;
		monthText.text = monthList[4-1];
		firstDayOfMonth = new DateTime(now.Year,4,1);
		numOfWeek = Array.IndexOf(dayList, firstDayOfMonth.DayOfWeek);
		daysInMonth = System.DateTime.DaysInMonth(now.Year,4);
		getDates();
		eventControl();
	}

	//This needs cleanup but its the place where the dictionary is initialized and the squares are filled with data
	void eventControl()
	{
		Dictionary<DateTime,string[]> events = new Dictionary<DateTime, string[]>()
		{
			{new DateTime(now.Year,4,1),new string[1]{"April Fools Day"}},
			{new DateTime(now.Year,4,2),new string[1]{"Holy Thursday"}},
			{new DateTime(now.Year,4,3),new string[1]{"Good Friday"}},
			{new DateTime(now.Year,4,4),new string[1]{"Start of Passover"}},
			{new DateTime(now.Year,4,5),new string[1]{"Easter"}},
			{new DateTime(now.Year,4,6),new string[0]},
			{new DateTime(now.Year,4,7),new string[1]{"World Health Day"}},
			{new DateTime(now.Year,4,8),new string[0]},
			{new DateTime(now.Year,4,9),new string[0]},
			{new DateTime(now.Year,4,10),new string[0]},
			{new DateTime(now.Year,4,11),new string[1]{"End of Passover"}},
			{new DateTime(now.Year,4,12),new string[2]{"Divine Mercy Sunday","Orthodox Easter"}},
			{new DateTime(now.Year,4,13),new string[0]},
			{new DateTime(now.Year,4,14),new string[0]},
			{new DateTime(now.Year,4,15),new string[1]{"Tax Day"}},
			{new DateTime(now.Year,4,16),new string[1]{"Holocaust Remembrance Day"}},
			{new DateTime(now.Year,4,17),new string[0]},
			{new DateTime(now.Year,4,18),new string[0]},
			{new DateTime(now.Year,4,19),new string[0]},
			{new DateTime(now.Year,4,20),new string[1]{"LOL"}},
			{new DateTime(now.Year,4,21),new string[0]},
			{new DateTime(now.Year,4,22),new string[3]{"Administrative Professionals Day","Earth Day","Yom HaZikaron"}},
			{new DateTime(now.Year,4,23),new string[1]{"Yom HaAtzma'ut"}},
			{new DateTime(now.Year,4,24),new string[1]{"Arbor Day"}},
			{new DateTime(now.Year,4,25),new string[1]{"Anzac Day"}},
			{new DateTime(now.Year,4,26),new string[0]},
			{new DateTime(now.Year,4,27),new string[0]},
			{new DateTime(now.Year,4,28),new string[0]},
			{new DateTime(now.Year,4,29),new string[0]},
			{new DateTime(now.Year,4,30),new string[0]}
		};
		for(int i=0; i<42; i++)
		{
			eventText[i].fontSize = eventFontSize;
			eventText[i].text = " ";
		}
		for(int i=1; i<=30; i++)
		{
			string[] dailyEvents = events[new DateTime(now.Year,4,i)];

			eventText[i-1+numOfWeek].text = " ";
			for (int j=0; j<dailyEvents.Length; j++)
			{
				eventText[i-1+numOfWeek].text = eventText[i-1+numOfWeek].text + dailyEvents[j] + "\n";
			}
		}
	}

	//Dictates which numbers should go where
	void getDates()
	{
		for(int i=0; i<42; i++)
		{
			if(i>=numOfWeek && i<numOfWeek+daysInMonth)
			{
				int date = i-numOfWeek+1;
				dateNum[i].text = date.ToString();
			}
			else
			{
				dateNum[i].text = " ";
			}
		}
	}
}
