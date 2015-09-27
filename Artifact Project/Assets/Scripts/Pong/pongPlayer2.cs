using UnityEngine;
using System.Collections;

public class pongPlayer2 : MonoBehaviour 
{
	public Rigidbody rigidbody;
	
	void Start()
	{
		rigidbody = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(transform.position.x <= -5 && transform.position.x >= -8)
		{
			if(Input.GetKey(KeyCode.A))
			{
				rigidbody.AddForce(-10,0,0,ForceMode.Acceleration);
			}
			if(Input.GetKey(KeyCode.D))
			{
				rigidbody.AddForce(10,0,0,ForceMode.Acceleration);
			}
		}
		else if(transform.position.x > -5)
		{
			rigidbody.AddForce(-5*(transform.position.x+5),0,0,ForceMode.Acceleration);
		}
		else
		{
			rigidbody.AddForce(-5*(transform.position.x+8),0,0,ForceMode.Acceleration);
		}
		if(Input.GetKey(KeyCode.W))
		{
			rigidbody.AddForce(0,0,20,ForceMode.Acceleration);
		}
		if(Input.GetKey(KeyCode.S))
		{
			rigidbody.AddForce(0,0,-20,ForceMode.Acceleration);
		}
		if(Input.GetKey(KeyCode.Q))
		{
			rigidbody.AddTorque(0,30,0);
		}
		if(Input.GetKey(KeyCode.E))
		{
			rigidbody.AddTorque(0,-30,0);
		}
		if(Input.GetKey(KeyCode.X))
		{
			rigidbody.angularVelocity = Vector3.zero;
		}
	}
}