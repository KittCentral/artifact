using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PlatformJump
{
    public class PlatformControl : MonoBehaviour
    {
        public GameObject platformPrefab;
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
                for (int i = 1; i <= platCount; i++)
                {
                    Transform trans = GetComponentsInChildren<Transform>()[i].transform;
                    trans.Translate(-1 * speed * Time.deltaTime, 0, 0);
                    if (trans.position.x < -10)
                        trans.position = new Vector3(10 * (platCount - 1), Random.Range(-2f, 5f));
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
    }
}
