// This script gives the Earth an inital eastward rotation when the RSS Scene is loaded.

using UnityEngine;
using System.Collections;

public class Spin : MonoBehaviour 
{
	
	void Start () 
	{
	
	}

	void Update () 
	{
		transform.Rotate(0,0,3);
	}
}
