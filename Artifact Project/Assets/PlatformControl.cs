using UnityEngine;

namespace PlatformJump
{
    public class PlatformControl : MonoBehaviour
    {
        public GameObject platformPrefab;
        public int platCount;
        public float speed;

        // Use this for initialization
        void Start()
        {
            for (int i = 0; i < platCount; i++)
            {
                GameObject plat = Instantiate(platformPrefab, new Vector3(10 * i, -1), Quaternion.identity) as GameObject;
                plat.transform.parent = transform;
            }
        }

        // Update is called once per frame
        void Update()
        {
            for (int i = 1; i < platCount + 1; i++)
            {
                Transform trans = GetComponentsInChildren<Transform>()[i].transform;
                trans.Translate(-1 * speed * Time.deltaTime, 0, 0);
                if (trans.position.x < -10)
                    trans.position = new Vector3(10 * (platCount - 1), Random.Range(-2f, 5f));
            }
        }
    }
}
