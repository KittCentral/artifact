// This is the primary script responsible for the RSS News Reader.  It displays the rss new object from rssreader.cs in a scrolling bar
// across the bottom of the screen.  When the new bar is clicked, the news story appears in the center of the screen.

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class NewsRSS : MonoBehaviour 
{
	//Initialization
	rssreader rdr;
	string info;
	float movement;
	bool infoUp;
	bool lastCheck = false;
	public int movementSpeed;
	public GUIStyle myStyle;

	public GameObject TextPanel;
	public GameObject TextBox;
	public GameObject GUICont;
	public Text TextOutput;

	GUI_Control guicontrol;

	void Start()
	{
		TextPanel.SetActive(false);
		rdr = new rssreader("http://www.npr.org/rss/rss.php?id=1001"); //draws the rss from NPR
		TextOutput = TextBox.GetComponent<Text>();
		guicontrol = GUICont.GetComponent<GUI_Control>();
	}

	//Shows the story for ten seconds then hides it
	IEnumerator ButtonWait()
	{
		TextPanel.SetActive(true);
		yield return new WaitForSeconds(10);
		TextPanel.SetActive(false);
	}

	//Legacy GUI used to create buttons per news story
	void OnGUI()
	{
		int i = 0;
		foreach (rssreader.items itm in rdr.rowNews.item)
		{
			Rect rect = new Rect(500 * i - 2 * movement + Screen.width, Screen.height * 0.9f, 400, 50);
			if (GUI.Button(rect, itm.title, myStyle))
			{
				info = itm.description;
				StartCoroutine(ButtonWait());
			}
			i++;
		}
	}

	//Ups the movement values which moves the buttons, also moves back to the start when it has gone too far
	void Update()
	{
		movement += movementSpeed * Time.deltaTime;
		if(movement > 500 * rdr.rowNews.item.Count + Screen.width)
		{
			movement -= 500 * rdr.rowNews.item.Count + Screen.width;
		}
		TextOutput.text = info;
	}
}
