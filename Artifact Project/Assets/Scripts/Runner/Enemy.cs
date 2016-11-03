using UnityEngine;
using System.Collections;

namespace Runner
{
    public class Enemy : MonoBehaviour
    {
        Enemies enemies;
        public float idleSpeed, speed;
        public MovementScheme movementScheme;
        public delegate void Idle();
        public bool canFire;
        public float bulletSpeed;
        public int level;
        public int column;
        Idle idle;

        bool stopped;

        public GameObject bullet;

        void Start()
        {
            enemies = transform.parent.GetComponent<Enemies>();
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
            if (col.tag != "Enemy")
            {
                enemies.enemyArray.Remove(new Vector2(level, column));
                Destroy(col.gameObject);
                StartCoroutine(Explode());
            }
        }

        void Update()
        {
            if (!stopped)
                idle();
        }

        public void Fire()
        {
            if (canFire)
            {
                GameObject bulletClone = Instantiate(bullet, transform.position, Quaternion.Euler(90f, 0f, 0f)) as GameObject;
                bulletClone.GetComponent<Rigidbody>().velocity = -Vector3.forward * bulletSpeed / (level + 1);
            }
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

        void Sweep()
        {
            if (transform.position.x > 12)
                enemies.SweepLeft = true;
            else if (transform.position.x < -12)
                enemies.SweepLeft = false;
            speed = enemies.enemyCount / (enemies.enemyBlockNum.x * enemies.enemyBlockNum.y);
        }

        IEnumerator SweepMove()
        {
            while (true)
            {
                while(!enemies.SweepLeft)
                {
                    transform.position += new Vector3(.5f, 0, 0);
                    yield return new WaitForSeconds(1f * speed /enemies.world.level);
                }
                transform.position += new Vector3(0, 0, -1f);
                yield return new WaitForSeconds(1f * speed / enemies.world.level);
                while (enemies.SweepLeft)
                {
                    transform.position += new Vector3(-.5f, 0, 0);
                    yield return new WaitForSeconds(1f * speed / enemies.world.level);
                }
                transform.position += new Vector3(0, 0, -1f);
                yield return new WaitForSeconds(1f * speed / enemies.world.level);
            }
        }

        IEnumerator Explode()
        {
            stopped = true;
            enemies.world.Score = 100 * ((int)enemies.enemyBlockNum.y - level);
            Enemy nextEnemy;
            if (enemies.enemyArray.TryGetValue(new Vector2(level - 1, column), out nextEnemy))
                nextEnemy.canFire = true;
            gameObject.GetComponent<Animator>().SetBool("Destroyed", true);
            yield return new WaitForSeconds(.5f);
            Destroy(gameObject);
        }
    }
}
