using UnityEngine;
using System.Collections;

namespace Pong
{
	public class player : MonoBehaviour 
	{
		public Rigidbody body;
		public KeyCode up, down, left, right, spinR, spinL;
		public int lowerBoundY, upperBoundY;
		
		void Start()
		{
			body = GetComponent<Rigidbody>();
		}

		void Update () 
		{
			if(transform.position.x >= lowerBoundY && transform.position.x <= upperBoundY)
			{
				if(Input.GetKey(left))
					body.AddForce(-10,0,0,ForceMode.Acceleration);

				if(Input.GetKey(right))
					body.AddForce(10,0,0,ForceMode.Acceleration);
			}

			else if(transform.position.x < lowerBoundY)
				body.AddForce(-5*(transform.position.x-lowerBoundY),0,0,ForceMode.Acceleration);

			else
				body.AddForce(-5*(transform.position.x-upperBoundY),0,0,ForceMode.Acceleration);

			if(Input.GetKey(up))
				body.AddForce(0,0,20,ForceMode.Acceleration);

			if(Input.GetKey(down))
				body.AddForce(0,0,-20,ForceMode.Acceleration);

			if(Input.GetKey(spinL))
				body.AddTorque(0,30,0);

			if(Input.GetKey(spinR))
				body.AddTorque(0,-30,0);

		}
	}
}