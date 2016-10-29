using UnityEngine;

namespace PlatformJump
{
    public class Obstacle : MonoBehaviour
    {
        public virtual Vector3 RandomPosition()
        {
            int key = Random.Range(0, 7);
            Vector3[] positions = new Vector3[] { new Vector3(1, 0), new Vector3(2, 0), new Vector3(3, 0), new Vector3(0, 1), new Vector3(4, 1), new Vector3(1, 2), new Vector3(2, 2), new Vector3(3, 2) };
            return positions[key];
        }
    }
}
