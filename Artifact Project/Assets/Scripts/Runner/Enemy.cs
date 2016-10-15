using UnityEngine;
using System.Collections;

namespace Runner
{
    public class Enemy : MonoBehaviour
    {
        public float idleSpeed;
        public MovementScheme movementScheme;
        public delegate void Idle();
        Idle idle;

        bool stopped = false;
        int waitIndex;

        void Start()
        {
            switch (movementScheme)
            {
                case MovementScheme.Pulse:
                    idle = Pulse;
                    break;
                case MovementScheme.Sweep:
                    idle = Sweep;
                    StartCoroutine(SweepMove());
                    break;
                default:
                    idle = Pulse;
                    break;
            }
        }

        void OnTriggerEnter (Collider col)
        {
            Destroy(col.gameObject);
            StartCoroutine(Explode());
        }

        void Update()
        {
            if(!stopped)
                idle();
        }

        void Pulse()
        {
            Vector3 pulseCenter = new Vector3(0, 1, 17);
            Vector3 changeVector = pulseCenter - transform.position;
            if ((int)(Time.time / 3) % 2 == 0)
                transform.position += changeVector * idleSpeed * Time.deltaTime;
            else
                transform.position -= changeVector * idleSpeed * Time.deltaTime;
        }

        void Sweep(){ }

        IEnumerator SweepMove()
        {
            while (true)
            {
                for (int i = 0; i < 2; i++)
                {
                    transform.position += new Vector3(.5f, 0, 0);
                    yield return new WaitForSeconds(1f);
                }
                transform.position += new Vector3(0, 0, -.5f);
                yield return new WaitForSeconds(1f);
                for (int i = 0; i < 2; i++)
                {
                    transform.position += new Vector3(-.5f, 0, 0);
                    yield return new WaitForSeconds(1f);
                }
                transform.position += new Vector3(0, 0, -.5f);
                yield return new WaitForSeconds(1f);
            }
        }

        IEnumerator Explode()
        {
            stopped = true;
            gameObject.GetComponent<Animator>().SetBool("Destroyed", true);
            yield return new WaitForSeconds(.5f);
            Destroy(gameObject);
        }
    }
}
