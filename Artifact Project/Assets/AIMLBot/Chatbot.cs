using AIMLbot;
using System;
using System.IO;
using UnityEngine;

public class Chatbot
{
		const string UserId = "consoleUser";
		private Bot AimlBot;
		private User myUser;
	
		public Chatbot ()
		{
				AimlBot = new Bot ();
				myUser = new User (UserId, AimlBot);
				Initialize ();
		}
	
		// Loads all the AIML files in the \AIML folder         
		public void Initialize ()
		{
				AimlBot.ChangeMyPath = LoaderFiles.filePath;
				AimlBot.loadSettings ();
				AimlBot.isAcceptingUserInput = false;
				AimlBot.loadAIMLFromFiles ();
				AimlBot.isAcceptingUserInput = true;
		}
	
		// Given an input string, finds a response using AIMLbot lib
		public String getOutput (String input)
		{
				Request r = new Request (input, myUser, AimlBot);
				Result res = AimlBot.Chat (r);
				return (res.Output);
		}
}