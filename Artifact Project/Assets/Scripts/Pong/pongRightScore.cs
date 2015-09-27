using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class pongRightScore : MonoBehaviour 
{
	public GameObject ball;
	public Rigidbody ballBody;
	public GameObject p1;
	public Rigidbody p1Body;
	public GameObject p2;
	public Rigidbody p2Body;
	public Text scoreboard;
	public GameObject explosion;

	Vector3 rot = new Vector3(0,0,0);
	int score;

	void Start () 
	{
		ballBody = ball.GetComponent<Rigidbody>();
		p1Body = p1.GetComponent<Rigidbody>();
		p2Body = p2.GetComponent<Rigidbody>();
	}
	
	void OnTriggerEnter(Collider col)
	{
		if(col.gameObject.tag == "Ball")
		{
			Instantiate(explosion, ball.transform.position, Quaternion.Euler(ball.transform.eulerAngles.x + 90, ball.transform.eulerAngles.y, ball.transform.eulerAngles.z));
			ball.transform.position = Vector3.zero;
			ballBody.velocity = new Vector3(10,0,0);
			p1.transform.position = new Vector3(7,0,0);
			p1.transform.eulerAngles = rot;
			p1Body.velocity = Vector3.zero;
			p1Body.angularVelocity = Vector3.zero;
			p2.transform.position = new Vector3(-7,0,0);
			p2.transform.eulerAngles = rot;
			p2Body.velocity = Vector3.zero;
			p2Body.angularVelocity = Vector3.zero;
			score += 10;
			scoreboard.text = score.ToString();
		}
	}
}
