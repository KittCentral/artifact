using UnityEngine;
using System.Collections;
using UnityEditor.Animations;

namespace Runner
{
    public class Enemy : MonoBehaviour
    {
        public float idleSpeed;
        public Sprite explosion;
        public AnimatorController explosionAnim;

        bool stopped = false;

        void OnTriggerEnter (Collider col)
        {
            Destroy(col.gameObject);
            StartCoroutine(Explode());
        }

        void Update()
        {
            if(!stopped)
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

        IEnumerator Explode()
        {
            stopped = true;
            gameObject.GetComponent<SpriteRenderer>().sprite = explosion;
            gameObject.GetComponent<Animator>().runtimeAnimatorController = explosionAnim;
            yield return new WaitForSeconds(.5f);
            Destroy(gameObject);
        }
    }
}
