using UnityEngine;
using System.Collections;

public class CameraPositionControl : MonoBehaviour 
{
	public const int stateSum = 2;
	public Vector3[] positions = new Vector3[stateSum];

	int index = 0;
	public int Index
	{
		get{ return index;}

		set
		{ 
			if(index < 0)
				index = 0;
			else if(index >= stateSum)
				index = stateSum - 1;
			index = value;
		}
	}

	void Update()
	{
		Vector3 target = positions[index];
		Vector3 temp = Vector3.Lerp(transform.localPosition,target,.025f);
		transform.localPosition = temp;
	}
}
