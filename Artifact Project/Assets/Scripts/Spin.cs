// This script gives basic rotation for the loading icon.
// It is assigned to an object called FlatEarth in Loading Scene.

using UnityEngine;

public class Spin : MonoBehaviour 
{
	void Update () 
	{
		transform.Rotate(0,0,3);
	}
}