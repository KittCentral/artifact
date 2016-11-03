using UnityEngine;
using System.Collections;

namespace Runner
{
    public class Player : MonoBehaviour
    {
        World world;

        public float horiSpeed;
        public float vertSpeed;
        public bool upMove;
        public bool jump;

        bool canJump;
        bool canFire = true;
        bool dead;

        public float bulletSpeed;
        public float fireDelay;

        public GameObject bullet;
        GameObject gameOver;

        // Use this for initialization
        void Start()
        {
            world = GameObject.Find("World").GetComponent<World>();
            gameOver = GameObject.Find("Game Over");
            gameOver.SetActive(false);
        }

        // Update is called once per frame
        void Update()
        {
            Vector3 velTar = (new Vector3(Input.GetAxis("Horizontal") * Time.deltaTime * horiSpeed * 100, 0, upMove ? Input.GetAxis("Vertical") * Time.deltaTime * vertSpeed * 100 : 0));
            transform.GetComponent<Rigidbody>().velocity = Vector3.Lerp(transform.GetComponent<Rigidbody>().velocity, velTar, .5f);
            PositionLock();
            if(Input.GetKeyDown(KeyCode.Return) && canFire)
            {
                GameObject bulletClone = Instantiate(bullet, transform.position, Quaternion.Euler(90f,0f,0f)) as GameObject;
                bulletClone.GetComponent<Rigidbody>().velocity = Vector3.forward * bulletSpeed;
                StartCoroutine("HoldFire");
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

        void OnTriggerEnter(Collider col)
        {
            if (col.tag == "Enemy")
            {
                Destroy(col.gameObject);
                StartCoroutine(GameOver());
            }
        }

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

        IEnumerator HoldFire()
        {
            canFire = false;
            yield return new WaitForSeconds(fireDelay);
            canFire = true;
        }

        IEnumerator GameOver()
        {
            dead = true;
            gameObject.GetComponent<Animator>().SetBool("Destroyed", true);
            yield return new WaitForSeconds(.5f);
            gameOver.SetActive(true);
            world.GameOver = true;
            Destroy(gameObject);
        }
    }
}
