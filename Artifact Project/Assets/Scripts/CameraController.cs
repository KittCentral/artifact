// General Camera Control Script
// Works best with the Camera as a child of an Empty Object where the focus is

using UnityEngine;
using System.Collections;
using System;

public class CameraController : MonoBehaviour 
{
	public CameraStateNames state;
	public Transform target;
	public bool targetFocus = true;
	public Vector3 followPosition;
	delegate void Control();
	Control control;

	void Start ()
	{
		if(target == null) {target = transform.parent.transform;}
		else {transform.parent.transform.position = target.position;}

		if(target.GetComponent<Rigidbody>() == null) {target.gameObject.AddComponent<Rigidbody>();}

		if(state == CameraStateNames.Follow) {control = Follow;}
	}

	void Update () 
	{
		control();
		if(targetFocus) transform.LookAt(target.position);
	}

	Action Follow()
	{
		transform.localPosition = followPosition;
	}

	Action FollowVelocity()
	{
		Vector2 unitVel = new Vector2(target.GetComponent<Rigidbody>().velocity.x,target.GetComponent<Rigidbody>().velocity.z).normalized;
		transform.localPosition = new Vector3(followPosition.x * unitVel.x, followPosition.y, followPosition.z * unitVel.y);
	}

	Action MultipleFixed()
	{
		
	}

	Action Rotating()
	{
		
	}
}

public enum CameraStateNames
{
	Follow, FollowVelocity, MultipleFixed, Rotating
}