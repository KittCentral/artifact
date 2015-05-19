using System;
using System.IO;
using UnityEngine;
using System.Collections;

public class LoaderFiles : MonoBehaviour
{
		public static string filePath = "";

		// Use this for initialization
		void Awake ()
		{
				//Sets the path of the files for the Chat Bot Running
				if (Application.platform == RuntimePlatform.Android) {
						filePath = Application.persistentDataPath;
				} else {
						filePath = Application.streamingAssetsPath;
				}				
		}

}//close classe
