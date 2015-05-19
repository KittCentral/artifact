// This is the primary script responsible for the RSS News Reader.  It displays the rss new object from rssreader.cs in a scrolling bar
// across the bottom of the screen.  When the new bar is clicked, the news story appears in the center of the screen.

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class NewsRSS : MonoBehaviour 
{
	rssreader rdr;
	string info;
	float movement;
	bool infoUp;
	bool lastCheck;
	public GUIStyle myStyle;

	public GameObject TextPanel;
	public GameObject TextBox;
	public GameObject GUICont;
	public Text TextOutput;

	GUI_Control guicontrol;

	void Start()
	{
		TextPanel.SetActive(false);
		lastCheck=false;
		rdr = new rssreader("http://www.npr.org/rss/rss.php?id=1001");
		TextOutput = TextBox.GetComponent<Text>();
		guicontrol = GUICont.GetComponent<GUI_Control>();
	}

	IEnumerator ButtonWait()
	{
		TextPanel.SetActive(true);
		yield return new WaitForSeconds(10);
		TextPanel.SetActive(false);
	}


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

	void Update()
	{
		movement = movement + 25 * Time.deltaTime;
		TextOutput.text = info;
	}
}
