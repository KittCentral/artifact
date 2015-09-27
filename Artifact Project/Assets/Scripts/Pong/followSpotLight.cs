using UnityEngine;
using System.Collections;

public class followSpotLight : MonoBehaviour 
{
	public Transform target;

	void Update() {
		Vector3 relativePos = -1 * target.position - transform.position;
		Quaternion rotation = Quaternion.LookRotation(relativePos);
		transform.rotation = rotation;
	}
}
