using UnityEngine;
using System.Collections;

public class MouseOverObject : MonoBehaviour
{
    ScoreManager scoreManager;

    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Destroy(gameObject, 0f);
            //scoreManager.increment(100);
        }
    }
}
