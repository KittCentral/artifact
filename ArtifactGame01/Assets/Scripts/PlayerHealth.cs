using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
   Animator Anim;
    public bool isDead = false;

    public void Hit(int hits)
    {
        if(hits >= 0 && !isDead)
        {
            Death();
        }
    }

    public void Death()
    {
        Anim.SetTrigger("Death");
    }
}
