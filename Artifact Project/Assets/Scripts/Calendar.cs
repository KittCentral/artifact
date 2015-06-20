// This script controls everything for the calendar section of the progam.
// It is called when the Calendar Scene is loaded.

using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

public class Calendar : MonoBehaviour 
{
	//Initialize the day squares
	public GameObject[] cubicles = new GameObject[42];
	private Text[] eventText = new Text[42];
	private Text[] dateNum = new Text[42];
	private Image[] cubBackground = new Image[42];

	//Further Initialization
	public GameObject MonthObj;
	public GameObject Background;
	private Image BGImage;
	private Text monthText;
	private DateTime displayedMonth;
	string nowFilename;
	DateTime firstDayOfMonth;
	int numOfWeek;
	int counter;
	public int eventFontSize;

	private Color[] colorList ={new Color(172f/255f,210f/255f,223f/255f),new Color(223f/255f,172f/255f,192f/255f),new Color(160f/255f,238f/255f,158f/255f),new Color(224f/255f,180f/255f,225f/255f),new Color(150f/255f,154f/255f,234f/255f),new Color(223f/255f,221f/255f,172f/255f),new Color(255f/255f,129f/255f,129f/255f),new Color(159f/255f,231f/255f,189f/255f),new Color(250f/255f,255f/255f,156f/255f),new Color(255f/255f,223f/255f,156f/255f),new Color(165f/255f,165f/255f,165f/255f),new Color(255f/255f,255f/255f,255f/255f)};
	private DayOfWeek[] dayList = new DayOfWeek[7]{DayOfWeek.Monday,DayOfWeek.Tuesday,DayOfWeek.Wednesday,DayOfWeek.Thursday,DayOfWeek.Friday,DayOfWeek.Saturday,DayOfWeek.Sunday};
	private string[] monthList = new  string[12]{"January","February","March","April","May","June","July","August","September","October","November","December"};
	string[] events;

	void Start()
	{
		for(int i=0; i<42; i++)
		{
			for(int j=0; j<cubicles[i].transform.childCount;j++)
			{
				if(cubicles[i].transform.GetChild(j).transform.name=="Date")
				{
					dateNum[i] = cubicles[i].transform.GetChild(j).transform.GetComponent<Text>();
				}
				if(cubicles[i].transform.GetChild(j).transform.name=="Events")
				{
					eventText[i] = cubicles[i].transform.GetChild(j).transform.GetComponent<Text>();
				}
			}
			cubBackground[i] = cubicles[i].GetComponent<Image>();
		}
		BGImage = Background.GetComponent<Image> ();
		monthText = MonthObj.GetComponent<Text> ();
		displayedMonth = DateTime.Now;
		DisplayMonth ();
	}

	void Update()
	{
		if(counter != 0)
		{
			counter -= 1;
		}
	}

	//This needs cleanup but its the place where the dictionary is initialized and the squares are filled with data
	void eventControl()
	{
		/*events = new Dictionary<Int32, string>()
		{
			{1,"April Fools Day"},
			{2,"Holy Thursday"},
			{3,"Good Friday"},
			{4,"Start of Passover"},
			{5,"Easter"},
			{6,""},
			{7,"World Health Day"},
			{8,""},
			{9,""},
			{10,""},
			{11,"End of Passover"},
			{12,"Divine Mercy Sunday.Orthodox Easter"},
			{13,""},
			{14,""},
			{15,"Tax Day"},
			{16,"Holocaust Remembrance Day"},
			{17,""},
			{18,""},
			{19,""},
			{20,"LOL"},
			{21,""},
			{22,"Administrative Professionals Day.Earth Day.Yom HaZikaron"},
			{23,"Yom HaAtzma'ut"},
			{24,"Arbor Day"},
			{25,"Anzac Day"},
			{26,""},
			{27,""},
			{28,""},
			{29,""},
			{30,""}
		};*/
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

	public void Serialize(string[] monthEvents, Stream stream)
	{
		BinaryWriter writer = new BinaryWriter (stream);
		string output;
		if(monthEvents.Length > 0)
		{
			output = monthEvents[0];
		}
		else
		{
			output = "";
		}
		for(int i=1;i<monthEvents.Length;i++)
		{
			output = output + "," + monthEvents[i];
		}
		writer.Write (output);
		writer.Flush ();
	}

	public string[] OpenMonth(DateTime dateTime)
	{
		string filename = "Assets/Shortcuts/MonthData/" + monthList [dateTime.Month] + dateTime.Year + ".dat";
		var file = File.Open (filename, FileMode.Open);
		BinaryReader reader = new BinaryReader (file);
		string input = reader.ReadString();
		string[] monthEvents = input.Split(new char[]{','});
		file.Close ();
		return monthEvents;
	}

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
		string filename = "Assets/Shortcuts/MonthData/" + monthList [Convert.ToInt32(split[1])] + split[0] + ".dat";
		var file = File.Open (filename, FileMode.Open);
		Serialize (monthEvents, file);
		file.Close ();
	}

	public void DeleteEvents(string date)
	{
		string[] split = date.Split(new char[]{'.'});
		DateTime dateTime = new DateTime (Convert.ToInt32(split [0]), Convert.ToInt32(split [1]), 1);
		string[] monthEvents = OpenMonth (dateTime);
		monthEvents[Convert.ToInt32(split[2])] = "";
		string filename = "Assets/Shortcuts/MonthData/" + monthList [Convert.ToInt32(split[1])] + split[0] + ".dat";
		var file = File.Open (filename, FileMode.Open);
		Serialize (monthEvents, file);
		file.Close ();
	}

	public void AddMonth(int change)
	{
		if (counter == 0)
		{
			displayedMonth = displayedMonth.AddMonths (change);
			DisplayMonth();
		}
		counter = 5;
	}

	public void LoseMonth(int change)
	{
		if (counter == 0)
		{
			displayedMonth = displayedMonth.AddMonths (-1*change);
			DisplayMonth();
		}
		counter = 5;
	}

	void DisplayMonth()
	{
		monthText.text = monthList[displayedMonth.Month-1];
		firstDayOfMonth = new  DateTime(displayedMonth.Year,displayedMonth.Month,1);
		numOfWeek = Array.IndexOf(dayList, firstDayOfMonth.DayOfWeek);
		nowFilename = "Assets/Shortcuts/MonthData/" + monthList [displayedMonth.Month-1] + displayedMonth.Year + ".dat";
		getDates();
		CreateMonth ();
		eventControl();
	}

	public void CreateMonth ()
	{
		if(!File.Exists(nowFilename))
		{
			var file = File.Open (nowFilename, FileMode.CreateNew);
			string[] monthEvents = new string[System.DateTime.DaysInMonth(displayedMonth.Year,displayedMonth.Month)];
			for(int i=0; i<System.DateTime.DaysInMonth(displayedMonth.Year,displayedMonth.Month);i++)
			{
				monthEvents[i] = "";
			}
			Serialize(monthEvents,file);
			file.Close();
		}
	}

	//Dictates which numbers should go where
	void getDates()
	{
		for(int i=0; i<42; i++)
		{
			if(i>=numOfWeek && i<numOfWeek+System.DateTime.DaysInMonth(displayedMonth.Year,displayedMonth.Month))
			{
				int date = i-numOfWeek+1;
				dateNum[i].text = date.ToString();
				cubBackground[i].fillCenter = false;
			}
			else
			{
				dateNum[i].text = " ";
				cubBackground[i].fillCenter = true;
			}
		}
		print (displayedMonth.Month);
		BGImage.color = colorList [displayedMonth.Month - 1];
	}
}