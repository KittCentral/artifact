using System;
using System.Xml.Serialization;
using System.IO;
using System.Collections.Generic;

namespace Calendar
{
    public class CalendarEvent
    {
        [XmlAttribute("day")]
        int day;
        [XmlAttribute("info")]
        string info;

        public int Day
        {
            get { return day; }
            set{day = value;}
        }

        public string Info
        {
            get { return info; }
            set { info = value; }
        }

        public CalendarEvent()
        {
            Day = 1;
            Info = "";
        }

        public CalendarEvent(int dateIn, string infoIn)
        {
            Day = dateIn;
            Info = infoIn;
        }

        public CalendarEvent(DateTime dateIn, string infoIn)
        {
            Day = dateIn.Day;
            Info = infoIn;
        }
    }

    [XmlRoot("Month")]
    public class CalendarEventMonth
    {
        [XmlArray("Events"), XmlArrayItem("Event")]
        public List<CalendarEvent> events = new List<CalendarEvent>();

        public CalendarEventMonth()
        {
            
        }

        public void Save(string path)
        {
            var serializer = new XmlSerializer(typeof(CalendarEventMonth));
            using (var stream = new FileStream(path, FileMode.Create))
            {
                serializer.Serialize(stream, this);
            }
        }

        public static CalendarEventMonth Load(string path)
        {
            var serializer = new XmlSerializer(typeof(CalendarEventMonth));
            using (var stream = new FileStream(path, FileMode.Open))
            {
                return serializer.Deserialize(stream) as CalendarEventMonth;
            }
        }
    }
}
