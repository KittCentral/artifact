using UnityEngine;
using System.Collections;

public class NavAgentScript : MonoBehaviour
{
    int attackDamage = 1;
    NavMeshAgent nav;
    Transform player;
    PlayerHealth playerHealth;
    bool playerInRange;
    Animator anim;
    Vector3 Destination = new Vector3(3, 0, -1);


    void Awake ()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        nav = GetComponent<NavMeshAgent>();
        playerHealth = player.GetComponent<PlayerHealth>(); 
    }

    void Update()
    {
        nav.SetDestination(player.position);

        if (Vector3.Distance(transform.position, Destination) < 1.0f)
        {
            playerHealth.Death();
        }
    }
}
