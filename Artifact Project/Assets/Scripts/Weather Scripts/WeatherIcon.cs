// This script creates and displays the weather panel on the screen.  
// It uses information from OurRSS.cs and WeatherPanel.cs

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class WeatherIcon : MonoBehaviour 
{
	int nb3;
	public GameObject WeatherRSS;
	public GameObject ImageObject;
	public GameObject TextTitle;
	public Image parentImage;
	public Image image;
	public Text titleText;
	OurRSS ourRSS;
	public Sprite[] icons = new Sprite[7];
	private string title;
	private string temperature;

	void Start () 
	{
		ourRSS = WeatherRSS.GetComponent<OurRSS>();
		image = ImageObject.GetComponent<Image>();
		parentImage = gameObject.GetComponent<Image>();
		titleText = TextTitle.GetComponent<Text>();
		StartCoroutine(Wait());
	}

	void IconRefresh()
	{
		if(nb3 == 1 || nb3 == 6)
		{
			image.sprite = icons[0];
		}
		else if(nb3 == 2)
		{
			image.sprite = icons[1];
		}
		else if(nb3 == 3 || nb3 == 4 || nb3 == 13)
		{
			image.sprite = icons[2];
		}
		else if(nb3 == 8 || nb3 == 0)
		{
			image.sprite = icons[3];
		}
		else if(nb3 == 7)
		{
			image.sprite = icons[4];
		}
		else if(nb3 == 11 || nb3 == 12)
		{
			image.sprite = icons[6];
		}
		else
		{
			image.sprite = icons[5];
		}
		switch (nb3)
		{
			case 0:
				title = "Rain";
				break;
			case 1:
				title = "Cloudy";
				break;
			case 2:
				title = "Mostly Cloudy";
				break;
			case 3:
				title = "Mostly Sunny";
				break;
			case 4:
				title = "Sunny";
				break;
			case 5:
				title = "Snow";
				break;
			case 6:
				title = "Overcast";
				break;
			case 7:
				title = "Isolated Showers";
				break;
			case 8:
				title = "Showers";
				break;
			case 9:
				title = "Snow Showers";
				break;
			case 10:
				title = "Blowing Snow";
				break;
			case 11:
				title = "Rain and Snow";
				break;
			case 12:
				title = "Wintery Mix";
				break;
			case 13:
				title = "Clear";
				break;
			default:
				title = "Unknown Error has Occured";
				break;
		}
	}

	IEnumerator Wait()
	{
		yield return new WaitForSeconds (1);
		IconRefresh();
	}

	void Update () 
	{
		nb3 =  ourRSS.nb2;
		temperature = ourRSS.temperature[1];
		titleText.text = "The Current Conditions are " + title + " and " + temperature + "℉";
		if (System.DateTime.UtcNow.Minute == 30 && System.DateTime.UtcNow.Second == 0)
		{
			IconRefresh();
		}
		if (System.DateTime.UtcNow.Minute == 0 && System.DateTime.UtcNow.Second == 0)
		{
			IconRefresh();
		}
	}
}
