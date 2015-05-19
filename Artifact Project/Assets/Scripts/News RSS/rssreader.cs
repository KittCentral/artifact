// Using the rss reader game object created in rsstester.cs, this script fills the game object with data
// The output is a game object which contains all of the information in the new rss feed.  
// It is now ready to be displayed on the screen, which is handled in NewsRSS.cs.

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

public class rssreader
{
	XmlTextReader rssReader;
	XmlDocument rssDoc;
	XmlNode nodeRss;
	XmlNode nodeChannel;
	XmlNode nodeItem;
	public channel rowNews;
	
	// this is the root channel information within the RSS
	// I have supplied the fields here that are in the UNITY RSS feed
	public struct channel
	{
		public string title;
		public string link;
		public string description;
		public string docs;
		public string managingEditor;
		public string webMaster;
		public string lastBuildDate;
        // this is our collection of RSS items
		public List<items> item;
	}
	
	// again, all values here are the same as what exists in the UNITY RSS feed
	public struct items
	{
		public string title;
		public string category;
		public string creator;
		public string guid;
		public string link;
		public string pubDate;
		public string description;
	}	
	
	// our constructor takes the URL to the feed
	public rssreader (string feedURL)
	{
		// setup the channel structure
		rowNews = new channel ();
        // make the list available to write to
		rowNews.item = new List<items>();
		rssReader = new XmlTextReader (feedURL);
		rssDoc = new XmlDocument ();
		rssDoc.Load (rssReader);
		// Loop for the <rss> tag
		for (int i = 0; i < rssDoc.ChildNodes.Count; i++) 
		{
			// If it is the rss tag
			if (rssDoc.ChildNodes[i].Name == "rss") {
				// <rss> tag found
				nodeRss = rssDoc.ChildNodes[i];
			}
		}
		// Loop for the <channel> tag
		for (int i = 0; i < nodeRss.ChildNodes.Count; i++) {
			// If it is the channel tag
			if (nodeRss.ChildNodes[i].Name == "channel") {
				// <channel> tag found
				nodeChannel = nodeRss.ChildNodes[i];
			}
		}
		// this is our channel header information
		rowNews.title = nodeChannel["title"].InnerText;
		rowNews.link = nodeChannel["link"].InnerText;
		rowNews.description = nodeChannel["description"].InnerText;
		//rowNews.docs = nodeChannel["docs"].InnerText;
		rowNews.lastBuildDate = nodeChannel["lastBuildDate"].InnerText;
		//rowNews.managingEditor = nodeChannel["managingEditor"].InnerText;
		//rowNews.webMaster = nodeChannel["webMaster"].InnerText;

		// here we have our feed items
		for (int i = 0; i < nodeChannel.ChildNodes.Count; i++) {
			if (nodeChannel.ChildNodes[i].Name == "item") {
				nodeItem = nodeChannel.ChildNodes[i];
				// create an empty item to fill
				items itm = new items();
				itm.title = nodeItem["title"].InnerText;
				itm.link = nodeItem["link"].InnerText;
				//itm.category = nodeItem["category"].InnerText;
				//itm.creator = nodeItem["dc:creator"].InnerText;
				itm.guid = nodeItem["guid"].InnerText;
				//itm.pubDate = nodeItem["pubDate"].InnerText;
				itm.description = nodeItem["description"].InnerText;
				// add the item to the channel items list
				rowNews.item.Add(itm);
			}
		}
	}
}