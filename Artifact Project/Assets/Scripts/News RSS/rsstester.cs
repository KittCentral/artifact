// This script initializes the rss reader game object, which is then passed to the rssreader.cs script.

using UnityEngine;
using System.Collections;

public class rsstester : MonoBehaviour
{
	rssreader rdr;
	// Use this for initialization
	void Start ()
	{
		// connect to the rss feed and pull it
		rdr = new rssreader("http://www.npr.org/rss/rss.php?id=1001");

		// show feed header
		Debug.Log("Title: "+rdr.rowNews.title);
		Debug.Log("Link: "+rdr.rowNews.link);
		Debug.Log("Description: "+rdr.rowNews.description);
		Debug.Log("Docs: "+rdr.rowNews.docs);
		Debug.Log("Last Build Date: "+rdr.rowNews.lastBuildDate);
		Debug.Log("Managing Editor: " + rdr.rowNews.managingEditor);
		Debug.Log("Web Master: " + rdr.rowNews.webMaster);

		// now display the feed items
		foreach(rssreader.items itm in rdr.rowNews.item)
		{
			Debug.Log("Item Title: " + itm.title);
			Debug.Log("Item Category: " + itm.category);
			Debug.Log("Item Creator: " + itm.creator);
			Debug.Log("Item guid: " + itm.guid);
			Debug.Log("Item link: " + itm.link);
			Debug.Log("Item publication date: " + itm.pubDate);
			Debug.Log("Item description: " + itm.description);
		}
	}

	// Update is called once per frame
	void Update ()
	{
		
	}
}

