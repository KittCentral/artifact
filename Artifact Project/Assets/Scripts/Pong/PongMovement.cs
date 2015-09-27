using UnityEngine;
using System.Collections;

public class PongMovement : MonoBehaviour 
{
	public Rigidbody rigidbody;
	public GameObject dust;
	int counter;

	void Start()
	{
		rigidbody = GetComponent<Rigidbody>();
		rigidbody.AddForce(5,0,0,ForceMode.VelocityChange);
	}

	void Update()
	{
		counter+=1;
		if (counter == 25)
		{
			Instantiate(dust, new Vector3(transform.position.x + Random.Range(0,1),transform.position.y + Random.Range(0,1),transform.position.z + Random.Range(0,1)), transform.rotation);
			counter = 0;
		}
	}
}
