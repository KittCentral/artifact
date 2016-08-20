using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PlatformJump
{
    public class PlatformControl : MonoBehaviour
    {
        public GameObject platformPrefab;
        public GameObject[] obstaclePrefabs;
        public int platCount;
        public float startSpeed;
        float speed;
        public float acceleration;

        bool stopped;
        public bool Stopped{ get{ return stopped;} set{ stopped = value;} }

        public GameObject scoreText;
        float score = 0;

        // Use this for initialization
        void Start()
        {
            BeginPlatforms();
        }

        // Update is called once per frame
        void Update()
        {
            if (!Stopped)
            {
                foreach (Transform child in transform)
                {
                    child.Translate(-1 * speed * Time.deltaTime, 0, 0);
                    if (child.position.x < -10)
                        PlatformRecreate(child);
                }
                speed += acceleration * Time.deltaTime;
                score += speed * Time.deltaTime;
                scoreText.GetComponent<Text>().text = ((int)score).ToString();
            }
        }

        public void BeginPlatforms()
        {
            for (int i = 0; i < platCount; i++)
            {
                GameObject plat = Instantiate(platformPrefab, new Vector3(10 * i, -1), Quaternion.identity) as GameObject;
                plat.transform.parent = transform;
            }
            speed = startSpeed;
            score = 0;
            scoreText.GetComponent<Text>().text = score.ToString();
        }

        public void KillPlatforms()
        {
            var children = new List<GameObject>();
            foreach (Transform child in transform) children.Add(child.gameObject);
            children.ForEach(child => Destroy(child));
        }

        void PlatformRecreate(Transform trans)
        {
            trans.position = new Vector3(10 * (platCount - 1), Random.Range(-2f, 5f));
            var children = new List<GameObject>();
            foreach (Transform child in trans) children.Add(child.gameObject);
            children.ForEach(child => Destroy(child));
            GameObject obst = Instantiate(obstaclePrefabs[0]) as GameObject;
            ObstaclePositioning(obst, trans);
        }

        void ObstaclePositioning(GameObject obj, Transform parent)
        {
            Vector3 posKey = obj.GetComponent<Obstacle>().RandomPosition();
            Vector3 pos = Vector3.zero;
            float rot = 0;
            switch ((int)posKey.x)
            {
                case 0:
                    pos.x = -0.6f;
                    rot = 90;
                    break;
                case 1:
                    pos.x = -0.4f;
                    break;
                case 2:
                    pos.x = 0;
                    break;
                case 3:
                    pos.x = 0.4f;
                    break;
                case 4:
                    pos.x = 0.6f;
                    rot = -90;
                    break;
                default:
                    pos.x = 0;
                    break;
            }
            switch((int)posKey.y)
            {
                case 0:
                    pos.y = -1;
                    rot = 180;
                    break;
                case 1:
                    pos.y = 0;
                    break;
                case 2:
                    pos.y = 1;
                    break;
                default:
                    pos.y = 0;
                    break;
            }
            obj.transform.SetParent(parent);
            obj.transform.localPosition = pos;
            float xScale = parent.localScale.x * Mathf.Abs(Mathf.Cos(rot * Mathf.PI / 180)) + parent.localScale.y * Mathf.Abs(Mathf.Sin(rot * Mathf.PI / 180));
            float yScale = parent.localScale.y * Mathf.Abs(Mathf.Cos(rot * Mathf.PI / 180)) + parent.localScale.x * Mathf.Abs(Mathf.Sin(rot * Mathf.PI / 180));
            obj.transform.localScale = new Vector3(1.0f/xScale, 1.0f / yScale, 1.0f / parent.localScale.z);
            obj.transform.localRotation = Quaternion.Euler(0, 0, rot);
        }
    }
}
