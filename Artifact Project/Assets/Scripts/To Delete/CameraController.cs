// I believe this script was a part of the Roll-a-Ball Scene Test

using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour 
{
	public GameObject earth;

	void Start () 
	{
		
	}

	void Update () 
	{
		transform.position = new Vector3 (earth.transform.position.x, earth.transform.position.y + 2, earth.transform.position.z - 5);
		transform.rotation = Quaternion.LookRotation (earth.transform.position - transform.position,Vector3.up);
	}
}
