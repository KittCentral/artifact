using UnityEngine;

namespace Flight
{
    public class Plane : MonoBehaviour
    {
        public float thrust = 1f;
        public float rollSpeed = 1f;
        public float pitchSpeed = 1f;
        public float maxVelocity = 200f;

        public GameObject bullet;
        Rigidbody body;

        bool hitCheck = false;

        // Use this for initialization
        void Start()
        {
            body = gameObject.GetComponent<Rigidbody>();
            body.useGravity = true;
        }

        // Update is called once per frame
        void Update()
        {
            //Aiming Vector for the planes movement
            Vector3 targetPitch = transform.forward + transform.up * (Input.GetAxis("Vertical") * (hitCheck ? 0 : 1));
            //Ensures behavior like stalling
            float altitudeSpeedControl = (200 - Mathf.Pow(3, transform.position.y / 20) > 0 ? (200 - Mathf.Pow(3, transform.position.y / 20)) : 0);
            //Ensures a top speed
            float dragSpeedControl = (maxVelocity - body.velocity.magnitude) / maxVelocity;
            //Controls how strong the forward push is
            Vector3 forwardForce = transform.forward * dragSpeedControl * altitudeSpeedControl;
            //It acts kind of like lift although I'm not sure I understand why
            Vector3 upForce = transform.up * (Input.GetAxis("Vertical") * (hitCheck ? 0 : 1) * body.velocity.magnitude / 100);
            //If you are going slower you will turn faster
            body.angularDrag = (body.velocity.magnitude / 20);
            //Roll Controls
            transform.Rotate(0.0f, 0.0f, Input.GetAxis("Horizontal") * rollSpeed * -1 * (hitCheck ? 0 : 1));
            //Pushes vehicle
            body.AddForce((forwardForce - upForce) * thrust * 100000);
            //Rotates vehicle to appropriate Vector
            body.AddTorque(Vector3.Cross(targetPitch.normalized, body.velocity.normalized) * Mathf.Pow(body.velocity.magnitude, 2));
        }

        void Fire()
        {
            GameObject clone = Instantiate(bullet, transform.position + transform.forward * 3, Quaternion.identity) as GameObject;
        }

        void OnCollisionEnter (Collision col)
        {
            body.AddForce(col.relativeVelocity * 1000);
            body.velocity = body.velocity / 4;
            thrust = 0;
            hitCheck = true;
        }
    }
}
