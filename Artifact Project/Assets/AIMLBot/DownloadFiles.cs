using UnityEngine;
using System.Collections;
using System.IO;

public class DownloadFiles : MonoBehaviour
{
		private string[] AimlFiles = {			
				"AI.aiml",
				"Atomic.aiml",
				"Biography.aiml",
				"Bot.aiml",
				"Botmaster.aiml",
				"Client.aiml",
				"Computers.aiml",
				"CustomTagTest.aiml",
				"Default.aiml",
				"Emotion.aiml",
				"Food.aiml",
				"Geography.aiml",
				"History.aiml",
				"Inquiry.aiml",
				"Interjection.aiml",
				"IU.aiml",
				"Knowledge.aiml",
				"Literature.aiml",
				"Money.aiml",
				"Movies.aiml",
				"Music.aiml",
				"Personality.aiml",
				"Philosophy.aiml",
				"Pickup.aiml",
				"Predicates.aiml",
				"Reduce.aiml",
				"Reductions.aiml",
				"Salutations.aiml",
				"Science.aiml",
				"Stack.aiml",
				"Stories.aiml",
				"That.aiml"
		
		};
		private string[] Configfiles = {			
				"DefaultPredicates.xml",
				"GenderSubstitutions.xml",
				"Person2Substitutions.xml",
				"PersonSubstitutions.xml",
				"Settings.xml",
				"Splitters.xml",
				"Substitutions.xml"
		};
	
		// Use this for initialization
		IEnumerator Start ()
		{	
				if (Application.platform == RuntimePlatform.Android) {
						//Not change the position of the lines
						for (int i = 0; i < Configfiles.Length; i++) {
								yield return StartCoroutine (CONFIGFilesDownload (Configfiles, i));
						}
						for (int i = 0; i < AimlFiles.Length; i++) {
								yield return StartCoroutine (AIMLFilesDownload (AimlFiles, i));
						}
				}
				//Go to the game scene
				Application.LoadLevel (1);
		}
	

		//Copy the files to the folder * .aiml"Application.persistentDataPath/aiml" 
		private IEnumerator AIMLFilesDownload (string[] files, int CurrentFile)
		{
				string origin = Application.streamingAssetsPath + "/aiml/" + files [CurrentFile];
				string NewFolder = Application.persistentDataPath + "/aiml";
				string destinationFile = NewFolder + "/" + files [CurrentFile];
				DirectoryInfo DirNewFolder = new DirectoryInfo (NewFolder);
				DirNewFolder.Refresh ();
				if (DirNewFolder.Exists == false) {
						DirNewFolder.Create ();	
				}
				WWW www = new WWW (origin);
				yield return www;
				File.WriteAllBytes (destinationFile, www.bytes);
		}//close IEnumerator

		//Copy the files to the folder * .xml "Application.persistentDataPath/config" 
		private IEnumerator CONFIGFilesDownload (string[] files, int CurrentFile)
		{
				string origin = Application.streamingAssetsPath + "/config/" + files [CurrentFile];
				string NewFolder = Application.persistentDataPath + "/config";
				string destinationFile = NewFolder + "/" + files [CurrentFile];
				DirectoryInfo DirNewFolder = new DirectoryInfo (NewFolder);
				DirNewFolder.Refresh ();
				if (DirNewFolder.Exists == false) {
						DirNewFolder.Create ();	
				}
				WWW www = new WWW (origin);
				yield return www;
				File.WriteAllBytes (destinationFile, www.bytes);
		}//close IEnumerator

}//close classe
