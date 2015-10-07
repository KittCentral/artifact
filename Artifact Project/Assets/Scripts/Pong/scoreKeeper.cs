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
		
		public bool resetBool;
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

			if(resetBool)
			{
				p1.transform.position = new Vector3(Mathf.Lerp(p1.transform.position.x,7,.1f),0,Mathf.Lerp(p1.transform.position.z,0,.1f));
				Vector3 angles1 = new Vector3(0,Mathf.Lerp(p1.transform.eulerAngles.y,0,.1f),0);
				p1.transform.eulerAngles = angles1;

				p2.transform.position = new Vector3(Mathf.Lerp(p2.transform.position.x,-7,.1f),0,Mathf.Lerp(p2.transform.position.z,0,.1f));
				Vector3 angles2 = new Vector3(0,Mathf.Lerp(p2.transform.eulerAngles.y,0,.1f),0);
				p2.transform.eulerAngles = angles2;
			}
		}

		void ResetPlayers()
		{
			p1Body.velocity = Vector3.zero;
			p1Body.angularVelocity = Vector3.zero;
			p2Body.velocity = Vector3.zero;
			p2Body.angularVelocity = Vector3.zero;

			resetBool = true;
		}

		IEnumerator ResetBall()
		{
			ball.transform.position = Vector3.zero;
			ballBody.velocity = new Vector3(0,0,0);
			yield return new WaitForSeconds(1);
			ballBody.velocity = new Vector3(5*dir,0,0);
			resetBool = false;
		}
	}
}