// This script controls everything for the calendar section of the progam.
// It is called when the Calendar Scene is loaded.

using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Collections.Generic;

public class Calendar : MonoBehaviour 
{
    #region Variable Initialization
    //Initialize the day squares
    public GameObject[] cubicles = new GameObject[42];
	Text[] eventText = new Text[42];
	Text[] dateNum = new Text[42];
	Image[] cubBackground = new Image[42];

    //Set in Inspector
	public Image BGImage;
	public Text monthText;
    public int eventFontSize;

    //Reusable variables
    DateTime displayedMonth, firstDayOfMonth;
	string nowFilename;
    int numOfWeek, counter;
    string[] events;

    //Enums and Lists
    enum Months { January = 1, February, March, April, May, June, July, August, September, October, November, December };
    Color[] colorList ={new Color(172f/255f,210f/255f,223f/255f),new Color(223f/255f,172f/255f,192f/255f),new Color(160f/255f,238f/255f,158f/255f),new Color(224f/255f,180f/255f,225f/255f),
        new Color(150f/255f,154f/255f,234f/255f),new Color(223f/255f,221f/255f,172f/255f),new Color(255f/255f,129f/255f,129f/255f),new Color(159f/255f,231f/255f,189f/255f),
        new Color(250f/255f,255f/255f,156f/255f),new Color(255f/255f,223f/255f,156f/255f),new Color(165f/255f,165f/255f,165f/255f),new Color(255f/255f,255f/255f,255f/255f)};
    #endregion

    #region Run at Times Functions
    void Start()
	{
		for(int i=0; i<42; i++)
		{
			for(int j=0; j<cubicles[i].transform.childCount;j++)
			{
				if(cubicles[i].transform.GetChild(j).transform.name=="Date")
					dateNum[i] = cubicles[i].transform.GetChild(j).transform.GetComponent<Text>();

				if(cubicles[i].transform.GetChild(j).transform.name=="Events")
					eventText[i] = cubicles[i].transform.GetChild(j).transform.GetComponent<Text>();
			}
			cubBackground[i] = cubicles[i].GetComponent<Image>();
		}
		displayedMonth = DateTime.Now;
		DisplayMonth ();
	}

    //Makes sure months can not be switched between too quickly
	void Update()
	{
		if(counter != 0)
			counter -= 1;
	}
    #endregion

    #region Month Initialization
    /// <summary>
    /// Shows the month held in the displayedMonth variable
    /// </summary>
    void DisplayMonth()
    {
        monthText.text = Enum.GetName(typeof(Months), displayedMonth.Month);
        firstDayOfMonth = new DateTime(displayedMonth.Year, displayedMonth.Month, 1);
        numOfWeek = (int)firstDayOfMonth.DayOfWeek;
        nowFilename = "Assets/Shortcuts/MonthData/" + Enum.GetName(typeof(Months), displayedMonth.Month) + displayedMonth.Year + ".dat";
        getDates();
        CreateMonth();
        eventControl();
    }

    /// <summary>
    /// Retrieves Dates and places them in the correct position for the month
    /// </summary>
	void getDates()
    {
        for (int i = 0; i < 42; i++)
        {
            if (i >= numOfWeek && i < numOfWeek + System.DateTime.DaysInMonth(displayedMonth.Year, displayedMonth.Month))
            {
                int date = i - numOfWeek + 1;
                dateNum[i].text = date.ToString();
                cubBackground[i].fillCenter = false;
            }
            else
            {
                dateNum[i].text = " ";
                cubBackground[i].fillCenter = true;
            }
        }
        print(displayedMonth.Month);
        BGImage.color = colorList[displayedMonth.Month - 1];
    }

    /// <summary>
    /// Creates a new month object in normal format
    /// </summary>
	public void CreateMonth()
    {
        if (!File.Exists(nowFilename))
        {
            var file = File.Open(nowFilename, FileMode.CreateNew);
            string[] monthEvents = new string[System.DateTime.DaysInMonth(displayedMonth.Year, displayedMonth.Month)];
            for (int i = 0; i < System.DateTime.DaysInMonth(displayedMonth.Year, displayedMonth.Month); i++)
            {
                monthEvents[i] = "";
            }
            Serialize(monthEvents, file);
            file.Close();
        }
    }

    /// <summary>
    /// Displays events for any month
    /// </summary>
    void eventControl()
	{
		events = OpenMonth (new DateTime(displayedMonth.Year,displayedMonth.Month-1,1));
		for(int i=0; i<42; i++)
		{
			eventText[i].fontSize = eventFontSize;
			eventText[i].text = " ";
		}
		for(int i=1; i<=System.DateTime.DaysInMonth(displayedMonth.Year,displayedMonth.Month); i++)
		{
			string dailyEvents = events[i-1];
			string[] split = dailyEvents.Split(new char[]{'.'});
			eventText[i-1+numOfWeek].text = " ";
			for (int j=0; j<split.Length; j++)
			{
				eventText[i-1+numOfWeek].text = eventText[i-1+numOfWeek].text + split[j] + "\n";
			}
		}
	}

    /// <summary>
    /// Finds the events for the month
    /// </summary>
    /// <param name="dateTime">Month to be opened</param>
    /// <returns>Events for the month</returns>
	public string[] OpenMonth(DateTime dateTime)
    {
        string filename = "Assets/Shortcuts/MonthData/" + Enum.GetName(typeof(Months), dateTime.Month) + dateTime.Year + ".dat";
        var file = File.Open(filename, FileMode.Open);
        BinaryReader reader = new BinaryReader(file);
        string input = reader.ReadString();
        string[] monthEvents = input.Split(new char[] { ',' });
        file.Close();
        return monthEvents;
    }
    #endregion

    #region Event Operations
    /// <summary>
    /// Adds an event at the date
    /// </summary>
    /// <param name="date">Date and Event Info</param>
    public void AddEvent(string date)
	{
		string[] split = date.Split(new char[]{'.'});
		DateTime dateTime = new DateTime (Convert.ToInt32(split [0]), Convert.ToInt32(split [1]), 1);
		string[] monthEvents = OpenMonth (dateTime);
		string stuff = monthEvents [Convert.ToInt32(split [2])];
		if (!Convert.ToBoolean(String.Compare(stuff,"")))
		{
			monthEvents[Convert.ToInt32(split[2])] = split[3];
		}
		else
		{
			monthEvents[Convert.ToInt32(split[2])] = stuff + "." + split[3];
		}
		string filename = "Assets/Shortcuts/MonthData/" + Enum.GetName(typeof(Months), Convert.ToInt32(split[1])) + split[0] + ".dat";
		var file = File.Open (filename, FileMode.Open);
		Serialize (monthEvents, file);
		file.Close ();
	}

    /// <summary>
    /// Removes an event at the date
    /// </summary>
    /// <param name="date">Date and Event Info</param>
	public void DeleteEvents(string date)
	{
		string[] split = date.Split(new char[]{'.'});
		DateTime dateTime = new DateTime (Convert.ToInt32(split [0]), Convert.ToInt32(split [1]), 1);
		string[] monthEvents = OpenMonth (dateTime);
		monthEvents[Convert.ToInt32(split[2])] = "";
		string filename = "Assets/Shortcuts/MonthData/" + Enum.GetName(typeof(Months), Convert.ToInt32(split[1])) + split[0] + ".dat";
		var file = File.Open (filename, FileMode.Open);
		Serialize (monthEvents, file);
		file.Close ();
	}
    #endregion

    #region Month Changers
    /// <summary>
    /// Moves forward months
    /// </summary>
    /// <param name="change">Number of months to move forward</param>
    public void AddMonth(int change)
	{
		if (counter == 0)
		{
			displayedMonth = displayedMonth.AddMonths (change);
			DisplayMonth();
		}
		counter = 5;
	}

    /// <summary>
    /// Moves backward months
    /// </summary>
    /// <param name="change">Number of months to move backward</param>
	public void LoseMonth(int change)
	{
		if (counter == 0)
		{
			displayedMonth = displayedMonth.AddMonths (-1*change);
			DisplayMonth();
		}
		counter = 5;
	}
    #endregion

    #region Serializer
    /// <summary>
    /// Saves the data held in an event string
    /// </summary>
    /// <param name="monthEvents">Event String</param>
    /// <param name="stream">Where to save it to</param>
    public void Serialize(string[] monthEvents, Stream stream)
    {
        BinaryWriter writer = new BinaryWriter(stream);
        string output;
        if (monthEvents.Length > 0)
        {
            output = monthEvents[0];
        }
        else
        {
            output = "";
        }
        for (int i = 1; i < monthEvents.Length; i++)
        {
            output = output + "," + monthEvents[i];
        }
        writer.Write(output);
        writer.Flush();
    }
    #endregion
}

#region Custom Classes
public class CalendarEvent
{
    DateTime date;
    [XmlAttribute("year")]
    int year;
    [XmlAttribute("month")]
    int month;
    [XmlAttribute("day")]
    int day;
    [XmlAttribute("info")]
    string info;

    public int Year
    {
        get { return year; }
        set
        {
            date = new DateTime(value, month, day);
            year = value;
        }
    }

    public int Month
    {
        get { return month; }
        set
        {
            date = new DateTime(year, value, day);
            month = value;
        }
    }

    public int Day
    {
        get { return day; }
        set
        {
            date = new DateTime(year, month, value);
            day = value;
        }
    }

    public DateTime Date
    {
        get { return date; }
        set
        {
            year = value.Year;
            month = value.Month;
            day = value.Day;
            date = value;
        }
    }

    public string Info
    {
        get { return info; }
        set { info = value; }
    }

    public CalendarEvent(DateTime dateIn, string infoIn)
    {
        Date = dateIn;
        Year = dateIn.Year;
        Month = dateIn.Month;
        Day = dateIn.Day;
        Info = infoIn;
    }
}

[XmlRoot("Month")]
public class CalendarEventMonth
{
    [XmlArray("Events")]
    [XmlArrayItem("Event")]
    public List<CalendarEvent> Monsters = new List<CalendarEvent>();
}
#endregion