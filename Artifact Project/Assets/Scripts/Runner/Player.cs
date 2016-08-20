using UnityEngine;

namespace Runner
{
    public class Player : MonoBehaviour
    {
        public float horiSpeed;
        public float vertSpeed;
        public bool upMove;
        public bool jump;
        bool canJump;
        public float bulletSpeed;

        public GameObject bullet;

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            Vector3 velTar = (new Vector3(Input.GetAxis("Horizontal") * Time.deltaTime * horiSpeed * 100, 0, upMove ? Input.GetAxis("Vertical") * Time.deltaTime * vertSpeed * 100 : 0));
            transform.GetComponent<Rigidbody>().velocity = Vector3.Lerp(transform.GetComponent<Rigidbody>().velocity, velTar, .5f);
            PositionLock();
            if(Input.GetKeyDown(KeyCode.Return))
            {
                GameObject bulletClone = Instantiate(bullet, transform.position, Quaternion.Euler(90f,0f,0f)) as GameObject;
                bulletClone.GetComponent<Rigidbody>().velocity = Vector3.forward * bulletSpeed;
            }
            if (canJump)
            {
                if (Input.GetKeyDown(KeyCode.Space) && jump)
                    Jump();
            }
            else
                GetComponent<Rigidbody>().AddForce(Vector3.down * 600);
        }

        void OnCollisionStay() { canJump = true; }
        void OnCollisionExit() { canJump = false; }

        void PositionLock ()
        {
            if (transform.position.x < -12f)
                transform.position = new Vector3(-12f, transform.position.y, transform.position.z);
            else if (transform.position.x > 12f)
                transform.position = new Vector3(12f, transform.position.y, transform.position.z);
            if (transform.position.z < -2f)
                transform.position = new Vector3(transform.position.x, transform.position.y, -2f);
            else if (transform.position.z > 2f)
                transform.position = new Vector3(transform.position.x, transform.position.y, 2f);
        }

        void Jump ()
        {
            GetComponent<Rigidbody>().AddForce(Vector3.up * 10000);
        }
    }
}
