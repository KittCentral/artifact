using UnityEngine;

namespace PlatformJump
{
    [RequireComponent(typeof(Rigidbody))]
    public class Player : MonoBehaviour
    {
        public enum JumpTypes { singleJump, doubleJump, flying };
        public JumpTypes jumpType = JumpTypes.singleJump;

        public float jumpForce;
        Rigidbody body;

        int jump = -1;

        // Use this for initialization
        void Awake()
        {
            body = GetComponent<Rigidbody>();
            JumpReset();
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space) && jump != 0)
                Jump();
            if (transform.position.y < -5)
                Fall();
        }

        void OnCollisionEnter (Collision col)
        {
            if(col.gameObject.tag == "Field")
                JumpReset();
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
            body.velocity = Vector3.zero;
            transform.position = new Vector3(0, 12, 0);
        }
    }
}
