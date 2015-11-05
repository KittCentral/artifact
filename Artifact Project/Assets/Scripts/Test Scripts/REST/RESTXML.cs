using UnityEngine;
using System;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Linq;
using System.Net;
using System.Collections;

public class RESTXML : MonoBehaviour 
{
	#region Keys
	static string BingMapsKey = "Ar83wUfPWS_rWAQAQTN2UTDWQ0PPccW73r6Vc0opnqtct0-O8153Z4aW6_SAxt5Y";
	//static string OpenWeatherKey = "fe7c73fe5a9132447a38e10f9f185b5b";
	#endregion

	#region URL Retrievers
	/// <summary>
	/// Bings the location.
	/// </summary>
	/// <returns>The XmlDocument holding necessary info</returns>
	/// <param name="location">Location</param>
	static public XmlDocument BingLocation (string location)
	{
		try
		{
			string request = "http://dev.virtualearth.net/REST/v1/Locations/" +
				location +
					"?output=xml" +
					" &key=" + 
					BingMapsKey;;
			XmlDocument response = MakeRequest(request);
			return response;
		}
		catch (Exception e)
		{
			Debug.Log(e.Message);
			return null;
		}
	}

	/// <summary>
	/// Bings the elevation.
	/// </summary>
	/// <returns>The XmlDocument holding necessary info</returns>
	/// <param name="location">Location</param>
	static public XmlDocument BingElevation (string location)
	{
		try
		{
			string request = "http://dev.virtualearth.net/REST/v1/Elevation/List?points=" +
				LongAndLatUnformatted(BingLocation(location)) +
					"&output=xml" +
					" &key=" + 
					BingMapsKey;;
			XmlDocument response = MakeRequest(request);
			return response;
		}
		catch (Exception e)
		{
			Debug.Log(e.Message);
			return null;
		}
	}
	#endregion
	
	#region URL to XML
	/// <summary>
	/// Submits HTML request
	/// </summary>
	/// <returns>The XmlDocument holding necessary info</returns>
	/// <param name="requestUrl">Request URL</param>
	public static XmlDocument MakeRequest(string requestUrl)
	{
		try
		{
			HttpWebRequest request = WebRequest.Create(requestUrl) as HttpWebRequest;
			HttpWebResponse response = request.GetResponse() as HttpWebResponse;
			XmlDocument xmlDoc = new XmlDocument();
			xmlDoc.Load(response.GetResponseStream());
			return (xmlDoc);
		}
		catch (Exception e)
		{
			Debug.Log(e.Message);
			return null;
		}
	}
	#endregion

	#region XML Extractors
	//Creates Generic Request
	[System.Obsolete("Use specific request creator")]
	static public string CreateRequest(string queryString)
	{
		/*string UrlRequest = "http://api.openweathermap.org/data/2.5/weather?q=" + 
				queryString +
				"&mode=xml" + 
				"&appid=" + 
				OpenWeatherKey;*/
		string UrlRequest = "http://dev.virtualearth.net/REST/v1/Locations/" +
			queryString +
				"?output=xml" +
				" &key=" + 
				BingMapsKey;
		return (UrlRequest);
	}

	/// <summary>
	/// Extracts Longitude and Latitude from XML in readable format
	/// </summary>
	/// <returns>Formatted Latitude and Longitude</returns>
	/// <param name="response">XMLDocument</param>
	static public string LongAndLat(XmlDocument response)
	{
		XmlNamespaceManager nsmgr = new XmlNamespaceManager(response.NameTable);
		nsmgr.AddNamespace("rest", "http://schemas.microsoft.com/search/local/ws/rest/v1");
		XmlNodeList locationElements = response.SelectNodes("//rest:Location", nsmgr);
		string output = locationElements.Item(0).SelectSingleNode(".//rest:FormattedAddress", nsmgr).InnerText + 
				" has coordinates of " + 
				locationElements.Item(0).SelectSingleNode(".//rest:Latitude", nsmgr).InnerText + 
				", " + 
				locationElements.Item(0).SelectSingleNode(".//rest:Longitude", nsmgr).InnerText;
		return output;
	}

	/// <summary>
	/// Extracts Longitude and Latitude from XML in passable format
	/// </summary>
	/// <returns>Unformatted Latitude and Longitude</returns>
	/// <param name="response">XMLDocument</param>
	static string LongAndLatUnformatted(XmlDocument response)
	{
		XmlNamespaceManager nsmgr = new XmlNamespaceManager(response.NameTable);
		nsmgr.AddNamespace("rest", "http://schemas.microsoft.com/search/local/ws/rest/v1");
		XmlNodeList locationElements = response.SelectNodes("//rest:Location", nsmgr);
		string output = locationElements.Item(0).SelectSingleNode(".//rest:Latitude", nsmgr).InnerText + "," +
			locationElements.Item(0).SelectSingleNode(".//rest:Longitude", nsmgr).InnerText;
		return output;
	}

	/// <summary>
	/// Extracts Elevation from XML in meters
	/// </summary>
	/// <returns>Elevation</returns>
	/// <param name="response">XMLDocument</param>
	static public string Elevation(XmlDocument response)
	{
		XmlNamespaceManager nsmgr = new XmlNamespaceManager(response.NameTable);
		nsmgr.AddNamespace("rest", "http://schemas.microsoft.com/search/local/ws/rest/v1");
		string output = response.SelectNodes(".//rest:int", nsmgr).Item(0).InnerText + " meters";
		return output;
	}

	#region Original Extractor
	/// <summary>
	/// Extracts lots of info of possible locations for any name given
	/// </summary>
	/// <returns>Printed info about the Location</returns>
	/// <param name="response">XMLDocument</param>
	static public void ProcessResponse(XmlDocument locationsResponse)
	{
		//Create namespace manager
		XmlNamespaceManager nsmgr = new XmlNamespaceManager(locationsResponse.NameTable);
		nsmgr.AddNamespace("rest", "http://schemas.microsoft.com/search/local/ws/rest/v1");
		
		//Get formatted addresses: Option 1
		//Get all locations in the response and then extract the formatted address for each location
		XmlNodeList locationElements = locationsResponse.SelectNodes("//rest:Location", nsmgr);
		Debug.Log("Show all formatted addresses: Option 1");
		foreach (XmlNode location in locationElements)
		{
			Debug.Log(location.SelectSingleNode(".//rest:FormattedAddress", nsmgr).InnerText);
		}
		
		//Get formatted addresses: Option 2
		//Get all formatted addresses directly. This works because there is only one formatted address for each location.
		XmlNodeList formattedAddressElements = locationsResponse.SelectNodes("//rest:FormattedAddress", nsmgr);
		Debug.Log("Show all formatted addresses: Option 2");
		foreach (XmlNode formattedAddress in formattedAddressElements)
		{
			Debug.Log(formattedAddress.InnerText);
		}
		
		//Get the Geocode Points to use for display for each Location
		XmlNodeList locationElementsForGP = locationsResponse.SelectNodes("//rest:Location", nsmgr);
		Debug.Log("Show Geocode Point Data");
		foreach (XmlNode location in locationElements)
		{
			XmlNodeList displayGeocodePoints = location.SelectNodes(".//rest:GeocodePoint/rest:UsageType[.='Display']/parent::node()", nsmgr);
			Debug.Log(location.SelectSingleNode(".//rest:FormattedAddress", nsmgr).InnerText);
			//Debug.Log(" has " + displayGeocodePoints.Count.ToString() + " display geocode point(s).");
			Debug.Log(location.SelectSingleNode(".//rest:Longitude", nsmgr).InnerText);
		}
		
		//Get all locations that have a MatchCode=Good and Confidence=High
		XmlNodeList matchCodeGoodElements = locationsResponse.SelectNodes("//rest:Location/rest:MatchCode[.='Good']/parent::node()", nsmgr);
		Debug.Log("Show all addresses with MatchCode=Good and Confidence=High");
		foreach (XmlNode location in matchCodeGoodElements)
		{
			if (location.SelectSingleNode(".//rest:Confidence", nsmgr).InnerText == "High")
			{
				Debug.Log(location.SelectSingleNode(".//rest:FormattedAddress", nsmgr).InnerText);
			}
		}
	}
	#endregion
	#endregion
}