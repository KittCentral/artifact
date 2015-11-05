using UnityEngine;
using System.Collections;

public class Explosion : MonoBehaviour 
{
	public int length;
	int check = 0;
	
	// Update is called once per frame
	void Update () 
	{
		check += 1;
		if(check > length)
		{
			Destroy(gameObject);
		}
	}
}
