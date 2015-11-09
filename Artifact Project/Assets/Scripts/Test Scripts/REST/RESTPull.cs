using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using REST;

public class RESTPull : MonoBehaviour
{
	public InputField inputField;
	public Button LongAndLatButton, ElevationButton;

	public void LongLatCall () 
	{
		Debug.Log(RESTXML.LongAndLat(RESTXML.BingLocation(inputField.text)));
		inputField.text = "";
	}

	public void ElevationCall () 
	{
		Debug.Log(RESTXML.Elevation(RESTXML.BingElevation(inputField.text)));
		inputField.text = "";
	}

	public void TemperatureCall()
	{
		StartCoroutine(RESTXML.WeatherUndergroundCheck(inputField.text));
		inputField.text = "";
	}

	public void WolframAlphaCall()
	{
		StartCoroutine(RESTXML.WolframAlphaCheck(inputField.text));
		inputField.text = "";
	}
}
