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
	public GameObject[] eventObject = new GameObject[42];
	public GameObject[] cubicles = new GameObject[42];
	private Text[] eventText = new Text[42];
	private Text[] dateNum = new Text[42];

	//Further Initialization
	public GameObject MonthObj;
	private Text monthText;
	private DateTime displayedMonth;
	string nowFilename;
	DateTime firstDayOfMonth;
	int numOfWeek;
	public int eventFontSize;

	private DayOfWeek[] dayList = new DayOfWeek[7]{DayOfWeek.Monday,DayOfWeek.Tuesday,DayOfWeek.Wednesday,DayOfWeek.Thursday,DayOfWeek.Friday,DayOfWeek.Saturday,DayOfWeek.Sunday};
	private string[] monthList = new  string[12]{"January","February","March","April","May","June","July","August","September","October","November","December"};
	Dictionary<Int32,string> events;

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
		monthText = MonthObj.GetComponent<Text> ();
		displayedMonth = DateTime.Now;
		DisplayMonth ();
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
			string dailyEvents = events[i];
			string[] split = dailyEvents.Split(new char[]{'.'});
			eventText[i-1+numOfWeek].text = " ";
			for (int j=0; j<split.Length; j++)
			{
				eventText[i-1+numOfWeek].text = eventText[i-1+numOfWeek].text + split[j] + "\n";
			}
		}
	}

	public void Serialize(Dictionary<Int32,string> dictionary, Stream stream)
	{
		BinaryWriter writer = new BinaryWriter (stream);

		writer.Write (dictionary.Count);
		foreach(var events in dictionary)
		{
			writer.Write(events.Key);
			writer.Write(events.Value);
		}
		writer.Flush ();
	}

	public Dictionary<Int32,string> OpenMonth(DateTime dateTime)
	{
		string filename = "Assets/Shortcuts/MonthData/" + monthList [dateTime.Month] + dateTime.Year + ".dat";
		var file = File.Open (filename, FileMode.Open);
		BinaryReader reader = new BinaryReader (file);
		int count = reader.ReadInt32 ();
		var dictionary = new Dictionary<Int32,string> (count);
		for (int i = 0; i < count; i++)
		{
			var key = reader.ReadInt32();
			var value = reader.ReadString();
			dictionary.Add(key,value);
		}
		file.Close ();
		return dictionary;
	}

	public void AddEvent(string date)
	{
		string[] split = date.Split(new char[]{'.'});
		DateTime dateTime = new DateTime (Convert.ToInt32(split [0]), Convert.ToInt32(split [1]), 1);
		Dictionary<Int32,string> dict = OpenMonth (dateTime);
		string stuff = dict [Convert.ToInt32(split [2])];
		if (!Convert.ToBoolean(String.Compare(stuff,"")))
		{
			dict[Convert.ToInt32(split[2])] = split[3];
		}
		else
		{
			dict[Convert.ToInt32(split[2])] = stuff + "." + split[3];
		}
		string filename = "Assets/Shortcuts/MonthData/" + monthList [Convert.ToInt32(split[1])] + split[0] + ".dat";
		var file = File.Open (filename, FileMode.Open);
		Serialize (dict, file);
		file.Close ();
	}

	public void DeleteEvents(string date)
	{
		string[] split = date.Split(new char[]{'.'});
		DateTime dateTime = new DateTime (Convert.ToInt32(split [0]), Convert.ToInt32(split [1]), 1);
		Dictionary<Int32,string> dict = OpenMonth (dateTime);
		dict[Convert.ToInt32(split[2])] = "";
		string filename = "Assets/Shortcuts/MonthData/" + monthList [Convert.ToInt32(split[1])] + split[0] + ".dat";
		var file = File.Open (filename, FileMode.Open);
		Serialize (dict, file);
		file.Close ();
	}

	public void AddMonth(int change)
	{
		for(int i = 0; i < change; i++)
		{
			if(displayedMonth.Month != 12)
			{
				displayedMonth = new DateTime(displayedMonth.Year, displayedMonth.Month + 1, 1);
			}
			else
			{
				displayedMonth = new DateTime(displayedMonth.Year + 1, 1, 1);
			}
		}
		DisplayMonth();
	}

	public void LoseMonth(int change)
	{
		for(int i = 0; i < change; i++)
		{
			if(displayedMonth.Month != 1)
			{
				displayedMonth = new DateTime(displayedMonth.Year, displayedMonth.Month - 1, 1);
			}
			else
			{
				displayedMonth = new DateTime(displayedMonth.Year - 1, 12, 1);
			}
		}
		DisplayMonth();
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
			Dictionary<Int32,string> dict = new Dictionary<int, string>();
			for(int i=0; i<System.DateTime.DaysInMonth(displayedMonth.Year,displayedMonth.Month);i++)
			{
				dict.Add(i+1,"");
			}
			Serialize(dict,file);
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
			}
			else
			{
				dateNum[i].text = " ";
			}
		}
	}
}