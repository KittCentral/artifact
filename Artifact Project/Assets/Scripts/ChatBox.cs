// This script creates Fezzik's chat panel.  Fezzik is an AI which is 
// controlled in the folder AIMLbot.

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ChatBox : MonoBehaviour 
{
	//Initialization
	private Chatbot bot;
	public GameObject messBox;
	private Text messBoard;
	public string mess;
	public string ask;
	private string answer;
	public GameObject field;
	private InputField inputField;
	private Vector2 scrollPosition;
	private int counter;

	void Start () 
	{
		inputField = field.GetComponent<InputField>();
		messBoard = messBox.GetComponent<Text>();
		bot = new Chatbot();
	}

	//Sends the input to the response tree and saves the response
	//This is called in two place right now End Edit and Enter
	//Interestingly enough the Send Button technically does not send
	public void Send()
	{
		if(counter == 0 && !string.Equals(ask,""))
		{
			mess += "Me: " + ask + "\n" + "\n";
			answer = bot.getOutput(ask);
			mess += "Fezzik: " + answer + "\n" + "\n";
			inputField.text = "";
		}
	}

	//Changes the string value based on what is typed into input
	public void changeAsk(string arg)
	{
		ask = arg;
	}

	//Sends on Enter. Once it has sent it will not send again for 30 updates to prevent spamming
	void Update () 
	{
		if(Input.GetKeyUp(KeyCode.KeypadEnter))
		{
			Send();
			counter = 30;
		}
		if(counter != 0)
		{
			counter -= 1;
		}
		messBoard.text = mess;
	}
}
