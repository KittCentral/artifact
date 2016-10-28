using UnityEngine;

namespace Flight
{
    public class Plane : MonoBehaviour
    {
        public float thrust = 10f;
        public float rollSpeed = 1f;
        public float pitchSpeed = 1f;
        float maxVelocity = 200f;

        public GameObject bullet;
        Rigidbody body;

        bool hitCheck = false;

        int targetLoop;
        public int lastLoop;

        public float MaxVelocity
        {
            get{ return maxVelocity; }
            set{ maxVelocity = value; }
        }

        // Use this for initialization
        void Start()
        {
            targetLoop = 0;
            body = gameObject.GetComponent<Rigidbody>();
            body.useGravity = true;
            body.AddForce(transform.forward * 200, ForceMode.VelocityChange);
        }

        // Update is called once per frame
        void Update()
        {
            //Aiming Vector for the planes movement
            Vector3 targetPitch = transform.forward * Mathf.Cos(Input.GetAxis("Vertical") * (hitCheck ? 0 : 1)) + transform.up * Mathf.Sin(Input.GetAxis("Vertical") * (hitCheck ? 0 : 1));
            //Ensures behavior like stalling
            float altitudeSpeedControl = ((10000 - transform.position.y)/10000 > 0 ? (10000 - transform.position.y)/10000 : 0);
            //Controls how strong the forward push is
            Vector3 forwardForce = transform.forward * Time.deltaTime * maxVelocity * altitudeSpeedControl;
            //It acts kind of like lift although I'm not sure I understand why
            Vector3 upForce = transform.up * (Input.GetAxis("Vertical") * (hitCheck ? 0 : 1) * body.velocity.magnitude * Mathf.Pow(Vector3.Dot(transform.forward, body.velocity.normalized),2) * Time.deltaTime / 2);
            //If you are going slower you will turn faster
            body.angularDrag = (body.velocity.magnitude / 150);
            //Calculates Drag based of direction of the plane relative to the speed
            body.drag = .01f * (100 - 99 * Mathf.Abs(Vector3.Dot(transform.forward, body.velocity.normalized)));
            //Pushes vehicle
            body.AddForce((forwardForce - upForce) * thrust * 1000);
            //Rotates vehicle to appropriate Vector
            body.AddTorque(Vector3.Cross(targetPitch.normalized, body.velocity.normalized) * (100000 + 2900000 * Mathf.Abs(Input.GetAxis("Vertical") * (hitCheck ? 0 : 1))) * pitchSpeed / (100 + Mathf.Pow(body.velocity.magnitude, .75f)));
            //Roll Controls
            transform.Rotate(0.0f, 0.0f, Input.GetAxis("Horizontal") * rollSpeed * -1 * (hitCheck ? 0 : 1) * Time.deltaTime);

            if (Input.GetKeyDown(KeyCode.Space) && !hitCheck)
                Fire();

            if ((maxVelocity > 0 && Input.GetAxis("Mouse ScrollWheel") < 0) || (maxVelocity < 300 && Input.GetAxis("Mouse ScrollWheel") > 0))
            {
                maxVelocity += Input.GetAxis("Mouse ScrollWheel") * 30;
                maxVelocity = maxVelocity < 0 ? 0 : maxVelocity;
                maxVelocity = maxVelocity > 300 ? 300 : maxVelocity;
            }
        }

        void Fire()
        {
            GameObject clone = Instantiate(bullet, transform.position + transform.forward * 3, transform.rotation) as GameObject;
            clone.GetComponent<Rigidbody>().velocity = transform.forward * 500;
        }

        void OnCollisionEnter (Collision col)
        {
            body.AddForce(col.relativeVelocity * 1000);
            body.velocity = body.velocity / 10;
            thrust = 0;
            hitCheck = true;
            if(!transform.GetChild(0).gameObject.GetComponent<AudioSource>().isPlaying)
                transform.GetChild(0).gameObject.GetComponent<AudioSource>().Play();
            GetComponent<AudioSource>().enabled = false;
        }

        void OnTriggerEnter(Collider col)
        {
            if(col.gameObject.GetComponent<Loop>() != null)
            {
                if(col.gameObject.GetComponent<Loop>().ID == targetLoop)
                {
                    if (col.gameObject.GetComponent<Loop>().ID == lastLoop)
                        EndRace();
                    Destroy(col.gameObject);
                    targetLoop++;
                    
                }
            }
        }

        void EndRace()
        {
            GameObject.Find("Time").GetComponent<UIClock>().Stop = true;
        }
    }
}
