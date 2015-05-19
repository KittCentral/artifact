using UnityEngine;
using System.Collections;

public class Exit : MonoBehaviour 
{

	public void ExitProgram()
	{
		Application.Quit();
		print ("You exited");
	}
}
