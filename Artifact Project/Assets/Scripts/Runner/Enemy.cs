using UnityEngine;

namespace Runner
{
    public class Enemy : MonoBehaviour
    {
        public float idleSpeed;

        void OnTriggerEnter (Collider col)
        {
            Destroy(col.gameObject);
            Destroy(gameObject);
        }

        void Update()
        {
            Idle();
        }

        void Idle()
        {
            Vector3 pulseCenter = new Vector3(0, 1, 17);
            Vector3 changeVector = pulseCenter - transform.position;
            if ((int)(Time.time / 3) % 2 == 0)
                transform.position += changeVector * idleSpeed * Time.deltaTime;
            else
                transform.position -= changeVector * idleSpeed * Time.deltaTime;
            print((int)(Time.time / 3) % 2 == 0);
        }
    }
}
