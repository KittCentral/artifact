using UnityEngine;
using System.Collections;

public class pongPlayer1 : MonoBehaviour 
{
	public Rigidbody rigidbody;
	
	void Start()
	{
		rigidbody = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(transform.position.x >= 5 && transform.position.x <= 8)
		{
			if(Input.GetKey(KeyCode.LeftArrow))
			{
				rigidbody.AddForce(-10,0,0,ForceMode.Acceleration);
			}
			if(Input.GetKey(KeyCode.RightArrow))
			{
				rigidbody.AddForce(10,0,0,ForceMode.Acceleration);
			}
		}
		else if(transform.position.x < 5)
		{
			rigidbody.AddForce(-5*(transform.position.x-5),0,0,ForceMode.Acceleration);
		}
		else
		{
			rigidbody.AddForce(-5*(transform.position.x-8),0,0,ForceMode.Acceleration);
		}
		if(Input.GetKey(KeyCode.UpArrow))
		{
			rigidbody.AddForce(0,0,20,ForceMode.Acceleration);
		}
		if(Input.GetKey(KeyCode.DownArrow))
		{
			rigidbody.AddForce(0,0,-20,ForceMode.Acceleration);
		}
		if(Input.GetKey(KeyCode.RightControl))
		{
			rigidbody.AddTorque(0,30,0);
		}
		if(Input.GetKey(KeyCode.KeypadEnter))
		{
			rigidbody.AddTorque(0,-30,0);
		}
		if(Input.GetKey(KeyCode.RightShift))
		{
			rigidbody.angularVelocity = Vector3.zero;
		}
	}
}
