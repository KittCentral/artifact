using UnityEngine;
using System.Collections;

public class EnemyHealth : MonoBehaviour {

    public int startingHealth = 1;
    public int currentHealth;
    public int scoreValue = 100;
    ScoreManager scoreManager;

    Animator anim;
    BoxCollider boxCollider;
    bool isDead;

    void Awake()
    {
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider>();
        currentHealth = startingHealth;
    }

    public void TakeDamage(int amount)
    {
        if (isDead)
            return;
        currentHealth -= amount;

        if (currentHealth <= 0)
            Death();   
    }

    void Death()
    {
        isDead = true;
        Destroy(gameObject, .1f);
        scoreManager.increment(100);
    }
}
