//Just a script for the function that exits the program when built

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
