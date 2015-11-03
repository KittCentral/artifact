using UnityEngine;
using System;
using System.Xml;
using System.Xml.XPath;
using System.Net;
using System.Collections;

public class REST : MonoBehaviour 
{
	static string BingMapsKey = "Ar83wUfPWS_rWAQAQTN2UTDWQ0PPccW73r6Vc0opnqtct0-O8153Z4aW6_SAxt5Y";

	void Start ()
	{
		try
		{
			//Create the REST Services 'Find by Query' request
			string locationsRequest = CreateRequest("Camano%20Island");
			XmlDocument locationsResponse = MakeRequest(locationsRequest);
			Debug.Log(LongAndLat(locationsResponse));
		}
		catch (Exception e)
		{
			Debug.Log(e.Message);
		}
	}
	
	//Create the request URL
	public static string CreateRequest(string queryString)
	{
		string UrlRequest = "http://dev.virtualearth.net/REST/v1/Locations/" +
				queryString +
				"?output=xml" +
				" &key=" + BingMapsKey;
		return (UrlRequest);
	}
	
	//Submit the HTTP Request and return the XML response
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

	static public string LongAndLat(XmlDocument response)
	{
		//Create namespace manager
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
}
