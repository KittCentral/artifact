using UnityEngine;

namespace PipeDream
{
    public class Avatar : MonoBehaviour
    {
        public ParticleSystem trail, burst;
        Player player;

        public float deathCountdown = -1f;

        void Awake ()
        {
            player = transform.root.GetComponent<Player>();
        }

        void Update ()
        {
            if (deathCountdown >= 0f)
            {
                deathCountdown -= Time.deltaTime;
                if (deathCountdown <= 0f)
                {
                    deathCountdown = -1f;
                    trail.enableEmission = true;
                    player.Die();
                }
            }
        }

        void OnTriggerEnter (Collider col)
        {
            if (deathCountdown < 0f)
            {
                print((int)player.distanceTravelled);
                trail.enableEmission = false;
                burst.Emit(burst.maxParticles);
                player.velocity = 0f; player.rotationalVelocity = 0f;
                deathCountdown = burst.startLifetime;
            }
        }
    }
}