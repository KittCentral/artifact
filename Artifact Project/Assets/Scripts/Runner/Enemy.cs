using UnityEngine;

namespace Runner
{
    public class Enemy : MonoBehaviour
    {
        void OnTriggerEnter (Collider col)
        {
            Destroy(col.gameObject);
            Destroy(gameObject);
        }
    }
}
