using UnityEngine;
using System.Collections;

public class PlayerHealth : MonoBehaviour {

    public int startingHealth = 1;
    public int currentHealth;

    Animator anim;
    //NavAgentScript playerMovement; could be used to stop the player from moving after death.
    bool isDead;

    void Awake()
    {
        anim = GetComponent<Animator>();
        currentHealth = startingHealth;
        //playerMovement = GetComponent<NavAgentScript>;
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;

        if (currentHealth <= 0 && !isDead)
        {
            Death();
        }
    }

    IEnumerator wait()
    {
        yield return new WaitForSeconds(4);
        Application.LoadLevel(0);
    }

    public void Death()
    {
        isDead = true;
        anim.SetTrigger("Explosion");
        StartCoroutine(wait());
    }
}
