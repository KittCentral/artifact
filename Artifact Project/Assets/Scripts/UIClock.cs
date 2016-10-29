using UnityEngine;
using UnityEngine.UI;

public class UIClock : MonoBehaviour
{
    bool stop;
    public bool Stop { get { return stop; } set { stop = value; } }

    // Update is called once per frame
    void Update ()
    {
        if (!Stop)
        {
            float seconds = Time.time % 60;
            int minutes = Mathf.FloorToInt(Time.time / 60);
            string secondString = seconds < 10 ? "0" + (Mathf.Floor(seconds * 100) / 100).ToString() : (Mathf.Floor(seconds * 100) / 100).ToString();
            string minuteString = minutes < 10 ? "0" + minutes.ToString() : minutes.ToString();
            string time = minuteString + ":" + secondString;
            GetComponent<Text>().text = "Time=" + time;
        }
	}
}
