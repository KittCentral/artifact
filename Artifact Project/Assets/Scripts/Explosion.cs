using UnityEngine;
using System.Collections;

public class Explosion : MonoBehaviour 
{

	int check = 0;
	
	// Update is called once per frame
	void Update () 
	{
		check += 1;
		if(check > 300)
		{
			Destroy(gameObject);
		}
	}
}
