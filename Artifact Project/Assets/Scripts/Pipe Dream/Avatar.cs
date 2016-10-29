using UnityEngine;
using UnityEngine.UI;

namespace PipeDream
{
    public class Avatar : MonoBehaviour
    {
        public ParticleSystem trail, burst, body;
        public Text score;
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
                    body.enableEmission = true;
                }
            }
            score.text = "Score: " + ((int)Mathf.Pow(1.05f,player.distanceTravelled)*100).ToString();
        }

        void OnTriggerEnter (Collider col)
        {
            if (deathCountdown < 0f)
            {
                trail.enableEmission = false;
                body.enableEmission = false;
                burst.Emit(burst.maxParticles);
                player.velocity = 0f; player.rotationalVelocity = 0f;
                deathCountdown = burst.startLifetime;
            }
        }
    }
}