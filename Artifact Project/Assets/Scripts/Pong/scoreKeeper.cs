using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace Pong
{
	public class scoreKeeper : MonoBehaviour 
	{
		//variable assignment
		#region 
		public KeyCode reset;
		public GameObject ball;
		public Rigidbody ballBody;
		public GameObject p1;
		public Rigidbody p1Body;
		public GameObject p2;
		public Rigidbody p2Body;
		public Text scoreboard;
		public GameObject explosion;
		public int direction;
		int dir;
		
		Vector3 rot = new Vector3(0,0,0);
		int score;
		#endregion

		void Start () 
		{
			ballBody = ball.GetComponent<Rigidbody>();
			p1Body = p1.GetComponent<Rigidbody>();
			p2Body = p2.GetComponent<Rigidbody>();
			ballBody.AddForce(2.5f,0,0,ForceMode.VelocityChange);
		}
		
		void OnTriggerEnter(Collider col)
		{
			if(col.gameObject.tag == "Ball")
			{
				Instantiate(explosion, ball.transform.position, Quaternion.Euler(ball.transform.eulerAngles.x + 90, ball.transform.eulerAngles.y, ball.transform.eulerAngles.z));
				StartCoroutine(ResetBall());
				ResetPlayers();

				score += 10;
				scoreboard.text = score.ToString();
			}
		}

		void Update()
		{
			dir = direction/Mathf.Abs(direction);

			if(Input.GetKeyDown(reset))
			{
				StartCoroutine(ResetBall());
				ResetPlayers();
			}
		}

		void ResetPlayers()
		{
			p1.transform.position = new Vector3(7,0,0);
			p1.transform.eulerAngles = rot;
			p1Body.velocity = Vector3.zero;
			p1Body.angularVelocity = Vector3.zero;
			
			p2.transform.position = new Vector3(-7,0,0);
			p2.transform.eulerAngles = rot;
			p2Body.velocity = Vector3.zero;
			p2Body.angularVelocity = Vector3.zero;
		}

		IEnumerator ResetBall()
		{
			ball.transform.position = Vector3.zero;
			ballBody.velocity = new Vector3(0,0,0);
			yield return new WaitForSeconds(1);
			ballBody.velocity = new Vector3(5*dir,0,0);
		}
	}
}