using UnityEngine;
using System.Collections;

public class ChatWindowExample1 : MonoBehaviour
{
		private Chatbot bot;
		public GUISkin myskin;
		private string messBox = "", ask = "", user = "Me";
		private Vector2 scrollPosition;
		private Rect windowRect = new Rect (200, 200, 300, 450);
	
		// Use this for initialization
		void Start ()
		{	
				bot = new Chatbot ();
		}//close Start


		void OnGUI ()
		{
				GUI.skin = myskin;
				windowRect = GUI.Window (1, windowRect, windowFunc, "Chat");

		}

		private void windowFunc (int id)
		{			
				//=============== Start Scroll =====================
				scrollPosition = GUILayout.BeginScrollView (scrollPosition, /*GUILayout.Width (100),*/GUILayout.Height (350));
				//--------------------
				ObjectsInsideTheScroll ();
				//--------------------
				//=============== End Scroll =====================
				GUILayout.EndScrollView ();
				//
				if (GUILayout.Button ("Clear")) {
						messBox = "";
				}
				GUILayout.BeginHorizontal ();
				// Where the player put the text
				ask = GUILayout.TextField (ask);
				//=================================================
				if (GUILayout.Button ("Send", GUILayout.Width (75))) {	
						messBox += user + ": " + ask + "\n" + "\n";				
						//Response Bot AIML
						var answer = bot.getOutput (ask);
						//Response BotAIml in the Chat window
						messBox += "BOT: " + answer + "\n" + "\n";	
						ask = "";
				}
				//==================================================
				GUILayout.EndHorizontal ();
				GUILayout.BeginHorizontal ();
				GUILayout.Label ("User:");
				//The player places where his name
				user = GUILayout.TextField (user);

				GUILayout.EndHorizontal ();

				GUI.DragWindow (new Rect (0, 0, Screen.width, Screen.height));
		}//close windowFunc


		private void ObjectsInsideTheScroll ()
		{
				GUILayout.Label (messBox);
		}
}//close Classe
