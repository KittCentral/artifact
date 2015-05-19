using UnityEngine;
using System.Collections;
using System;

public class TestPic : MonoBehaviour {
	public GameObject earth;
	private string url;
	private string nextTime;
	private int counter;
	// Use this for initialization



	IEnumerator earthLight(int counter)
	{
		if(counter == 0)
		{
			url = "http://api.usno.navy.mil/imagery/earth.png?view=full&date=1/13/2015&time=" + System.DateTime.UtcNow.Hour.ToString () + ":00";
		}

		if(counter == 1)
		{
			url = "http://api.usno.navy.mil/imagery/earth.png?view=full&date=1/13/2015&time=" + System.DateTime.UtcNow.Hour.ToString () + ":30";
		}
	
		WWW www = new WWW (url);
		yield return www;
		earth.GetComponent<Renderer>().material.mainTexture = www.texture;
	
	}



	IEnumerator Start () 
	{
		//at :00-:09 the code breaks, no picture is downloaded.  When we get to the update, figure out how to download the next picture ahead of time.
		int minute = System.DateTime.UtcNow.Minute;
		if (minute < 10) 
		{
			url = "http://api.usno.navy.mil/imagery/earth.png?view=full&date=1/13/2015&time=" + System.DateTime.UtcNow.Hour.ToString () + ":0" + System.DateTime.UtcNow.Minute.ToString ();
		}
		else
		{
			url = "http://api.usno.navy.mil/imagery/earth.png?view=full&date=1/13/2015&time=" + System.DateTime.UtcNow.Hour.ToString() + ":" + System.DateTime.UtcNow.Minute.ToString ();
		}
		WWW www = new WWW (url);
		yield return www;
		earth.GetComponent<Renderer>().material.mainTexture = www.texture;
		print (System.DateTime.Now.ToString());
		print (url);
		counter = 0;
	}
	void Update () 
	{
		if(System.DateTime.UtcNow.Second.ToString() == "5")
		{
			counter = 0;
		}
		if(System.DateTime.UtcNow.Second.ToString() == "30")
		{
			counter = 1;
		}

		if (counter == 0) 
		{
			earthLight(counter);

			counter = 2;
			//WWW www = new WWW (url);
		}
		else if (counter == 1)
		{
			earthLight(counter);
			counter = 2;
		}
	}
}
