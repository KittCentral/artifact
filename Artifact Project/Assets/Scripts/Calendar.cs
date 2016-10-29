// This script controls everything for the calendar section of the progam.
// It is called when the Calendar Scene is loaded.

using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;
using System.Collections.Generic;
using DDay.iCal;

namespace Calendar
{
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
        public InputField addField, addYear, addMonth, addDay, removeField, removeYear, removeMonth, removeDay;
        public int eventFontSize;

        //Reusable variables
        DateTime displayedMonth, firstDayOfMonth;
        string nowFilename;
        int numOfWeek, counter;
        bool hold;

        //Enums and Lists
        enum Months { January = 1, February, March, April, May, June, July, August, September, October, November, December };
        Color[] colorList = {new Color(172f/255f, 210f/255f, 223f/255f, 100f/255f), new Color(223f/255f, 172f/255f, 192f/255f, 100f/255f), new Color(160f/255f, 238f/255f, 158f/255f, 100f/255f), new Color(224f/255f, 180f/255f, 225f/255f, 100f/255f), 
        new Color(150f/255f, 154f/255f, 234f/255f, 100f/255f), new Color(223f/255f, 221f/255f, 172f/255f, 100f/255f), new Color(255f/255f, 129f/255f, 129f/255f, 100f/255f), new Color(159f/255f, 231f/255f, 189f/255f, 100f/255f), 
        new Color(250f/255f, 255f/255f, 156f/255f, 100f/255f), new Color(255f/255f, 223f/255f, 156f/255f, 100f/255f), new Color(165f/255f, 165f/255f, 165f/255f, 100f/255f), new Color(255f/255f, 255f/255f, 255f/255f, 100f/255f)};
        #endregion

        #region Run at Times Functions
        void Start()
        {
            for (int i = 0; i < 42; i++)
            {
                for (int j = 0; j < cubicles[i].transform.childCount; j++)
                {
                    if (cubicles[i].transform.GetChild(j).transform.name == "Date")
                        dateNum[i] = cubicles[i].transform.GetChild(j).transform.GetComponent<Text>();

                    if (cubicles[i].transform.GetChild(j).transform.name == "Events")
                        eventText[i] = cubicles[i].transform.GetChild(j).transform.GetComponent<Text>();
                }
                cubBackground[i] = cubicles[i].GetComponent<Image>();
            }
            displayedMonth = DateTime.Now;
            DisplayMonth();
        }

        //Makes sure months can not be switched between too quickly
        void Update()
        {
            if (counter != 0)
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
            nowFilename = "Assets/Data/MonthData/" + Enum.GetName(typeof(Months), displayedMonth.Month) + displayedMonth.Year + ".xml";
            getDates();
            if (!File.Exists(nowFilename))
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
            BGImage.color = colorList[displayedMonth.Month - 1];
        }

        /// <summary>
        /// Creates a new month object in normal format
        /// </summary>
        public void CreateMonth()
        {
            CalendarEventMonth month = new CalendarEventMonth();
            month.events = ICSReader("http://" + "www.kayaposoft.com/enrico/ics/v1.0?country=usa&fromDate=01-" + 
                displayedMonth.Month + "-" +
                displayedMonth.Year + "&toDate=" + 
                DateTime.DaysInMonth(displayedMonth.Year, displayedMonth.Month) + "-" +
                displayedMonth.Month + "-" +
                displayedMonth.Year + "&region=Colorado");
            month.Save(nowFilename);
        }

        /// <summary>
        /// Displays events for any month
        /// </summary>
        void eventControl()
        {
            var monthEvents = OpenMonth(new DateTime(displayedMonth.Year, displayedMonth.Month, 1));
            for (int i = 0; i < 42; i++)
            {
                eventText[i].fontSize = eventFontSize;
                eventText[i].text = " ";
            }
            for (int i = 1; i <= DateTime.DaysInMonth(displayedMonth.Year, displayedMonth.Month); i++)
            {
                if (monthEvents.events.Exists(x => x.Day == i))
                {
                    List<CalendarEvent> dayEvent = monthEvents.events.FindAll(x => x.Day == i);
                    string eventArray = "";
                    foreach (CalendarEvent e in dayEvent.ToArray())
                    {
                        if (String.Equals(eventArray, ""))
                            eventArray = e.Info;
                        else
                            eventArray = eventArray + "\n" + e.Info;
                    }
                    eventText[i - 1 + (int)firstDayOfMonth.DayOfWeek].text = eventArray;
                }
            }
        }

        /// <summary>
        /// Finds the events for the month
        /// </summary>
        /// <param name="dateTime">Month to be opened</param>
        /// <returns>Events for the month</returns>
        public CalendarEventMonth OpenMonth(DateTime dateTime)
        {
            string filename = "Assets/Data/MonthData/" + Enum.GetName(typeof(Months), dateTime.Month) + dateTime.Year + ".xml";
            return CalendarEventMonth.Load(filename);
        }
        #endregion

        #region Event Operations
        /// <summary>
        /// Adds an event at the date
        /// </summary>
        /// <param name="item">Day and Event Info</param>
        /// <param name="month">Month Event will be added in</param>
        /// <param name="year">Year Event will be added in</param>
        public void AddEvent(CalendarEvent item, int year, int month)
        {
            string filename = "Assets/Data/MonthData/" + Enum.GetName(typeof(Months), month) + year + ".xml";
            var monthEvents = CalendarEventMonth.Load(filename);
            monthEvents.events.Add(item);
            monthEvents.Save(filename);
        }

        /// <summary>
        /// Removes an event at the date
        /// </summary>
        /// <param name="item">Day and Event Info</param>
        /// <param name="month">Month Event will be added in</param>
        /// <param name="year">Year Event will be added in</param>
        public void RemoveEvent(CalendarEvent item, int year, int month)
        {
            string filename = "Assets/Data/MonthData/" + Enum.GetName(typeof(Months), month) + year + ".xml";
            var monthEvents = CalendarEventMonth.Load(filename);
            CalendarEvent toRemove = monthEvents.events.Find(x => x.Info == item.Info);
            monthEvents.events.Remove(toRemove);
            monthEvents.Save(filename);
        }
        #endregion
        
        #region ICS Controls
        /// <summary>
        /// Gives events from Enrico Service during current month
        /// </summary>
        /// <param name="uri">Location of ICS file</param>
        /// <returns>List of events</returns>
        List<CalendarEvent> ICSReader(string uri)
        {
            var events = new List<CalendarEvent>();
            IICalendarCollection calendars = iCalendar.LoadFromUri(new System.Uri(uri));
            foreach (IICalendar calendar in calendars)
            {
                foreach (IEvent e in calendar.Events)
                    events.Add(new CalendarEvent(e.Start.Local.Day, e.Summary));
            }
            return events;
        }
        #endregion

        #region Button Functions
        /// <summary>
        /// Moves forward months
        /// </summary>
        /// <param name="change">Number of months to move forward</param>
        public void MonthChange(bool forward)
        {
            if (counter == 0)
            {
                displayedMonth = displayedMonth.AddMonths(forward?1:-1);
                DisplayMonth();
            }
            counter = 5;
        }

        public void AddEventFromInputs()
        {
            int year = !string.Equals(addYear.text, "") ? 2000 + Convert.ToInt32(addYear.text) : displayedMonth.Year;
            int month = !string.Equals(addMonth.text, "") ? Convert.ToInt32(addMonth.text) : displayedMonth.Month;
            AddEvent(new CalendarEvent(Convert.ToInt32(addDay.text), addField.text), year, month);
            addField.text = ""; addYear.text = ""; addMonth.text = ""; addDay.text = "";
            DisplayMonth();
        }

        public void RemoveEventFromInputs()
        {
            int year = !string.Equals(removeYear.text, "") ? 2000 + Convert.ToInt32(removeYear.text) : displayedMonth.Year;
            int month = !string.Equals(removeMonth.text, "") ? Convert.ToInt32(removeMonth.text) : displayedMonth.Month;
            if (!string.Equals(removeDay.text, ""))
                RemoveEvent(new CalendarEvent(Convert.ToInt32(removeDay.text), removeField.text), year, month);
            else
                RemoveEvent(OpenMonth(new DateTime(displayedMonth.Year, displayedMonth.Month, 1)).events.Find(x => x.Info == removeField.text), year, month);
            removeField.text = ""; removeYear.text = ""; removeMonth.text = ""; removeDay.text = "";
            DisplayMonth();
        }

        public void SceneClose ()
        {
            SceneControl.OpenSceneAdditive(10);
        }
        #endregion
    }
}