using UnityEngine;

namespace PlatformJump
{
    [RequireComponent(typeof(Rigidbody))]
    public class Player : MonoBehaviour
    {
        public enum JumpTypes { singleJump, doubleJump, flying };
        public JumpTypes jumpType = JumpTypes.singleJump;

        public bool yLoop;

        public float jumpForce;
        Rigidbody body;

        public GameObject gameOverText;
        public GameObject field;
        PlatformControl platformControl;

        int jump = -1;

        // Use this for initialization
        void Awake()
        {
            body = GetComponent<Rigidbody>();
            platformControl = field.GetComponent<PlatformControl>();
            JumpReset();
            gameOverText.SetActive(false);
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space) && jump != 0)
                Jump();
            if (transform.position.y < -5)
                Fall();
            if (Input.GetKeyDown(KeyCode.Return) && gameOverText.activeInHierarchy)
                Restart();
        }

        void OnCollisionEnter (Collision col)
        {
            if(col.gameObject.tag == "Field")
                JumpReset();
            if (col.gameObject.tag == "Enemy")
                GameOver();
        }

        void Jump ()
        {
            body.AddTorque(Vector3.forward * 50 * -1);
            body.AddForce(Vector3.up * 100 * jumpForce);
            jump--;
        }

        void JumpReset ()
        {
            if (jumpType == JumpTypes.singleJump)
                jump = 1;
            else if (jumpType == JumpTypes.doubleJump)
                jump = 2;
        }

        void Fall ()
        {
            if (yLoop)
            {
                body.velocity = Vector3.zero;
                transform.position = new Vector3(0, 12, 0);
            }
            else
                GameOver();
        }

        void GameOver()
        {
            gameOverText.SetActive(true);
            platformControl.Stopped = true;
        }

        void Restart()
        {
            body.velocity = Vector3.zero;
            transform.position = Vector3.zero;
            transform.rotation = Quaternion.identity;
            platformControl.KillPlatforms();
            gameOverText.SetActive(false);
            platformControl.Stopped = false;
            platformControl.BeginPlatforms();
        }
    }
}
