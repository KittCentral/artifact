using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Collections;

namespace Runner
{
    public class World : MonoBehaviour
    {
        public enum Dimension { Second, Third };
        public enum EnemyOrganization { Block };

        public Dimension dimension;
        public MovementScheme movementScheme;
        public EnemyOrganization organization;

        public float courseSpeed;
        
        public GameObject flat2DPrefab;
        public GameObject player;

        public GameObject levelObj; public int level = 1;
        public GameObject scoreObj;
        int score;
        public int Score { get { return score; } set
            {
                score += value * level;
                if (value == 0)
                    score = 0;
                scoreObj.GetComponent<Text>().text = "Score: " + score;
            } }

        List<GameObject> pieces = new List<GameObject>();

        bool gameOver;
        public bool GameOver { get { return gameOver; } set { gameOver = value; } }

        // Use this for initialization
        void Start()
        {
            StartCoroutine(ShowLevel());
            if (dimension == Dimension.Second)
            {
                Transform camTran = transform.FindChild("Main Camera");
                camTran.position = new Vector3(0, 10, 8);
                camTran.rotation = Quaternion.Euler(90, 0, 0);
                camTran.gameObject.GetComponent<Camera>().orthographic = true;
            }
            else
            {
                Transform camTran = transform.FindChild("Main Camera");
                camTran.position = new Vector3(0, 10, -8);
                camTran.rotation = Quaternion.Euler(30, 0, 0);
                camTran.gameObject.GetComponent<Camera>().orthographic = false;
            }
            for (int i = 0; i < 3; i++)
            {
                GameObject ground = Instantiate(flat2DPrefab, new Vector3(0, 0, 30 * i), Quaternion.identity) as GameObject;
                ground.transform.parent = transform;
                pieces.Add(ground);
            }
        }

        // Update is called once per frame
        void Update()
        {
            foreach (GameObject obj in pieces)
            {
                obj.transform.Translate(Vector3.forward * -Time.deltaTime * courseSpeed);
                if (obj.transform.position.z < -30f)
                    obj.transform.position = new Vector3(transform.position.x, transform.position.y, obj.transform.position.z + 90f);
            }
            if (GameOver && Input.GetKeyDown(KeyCode.Return))
                Reset();
        }

        void Reset()
        {
            level = 1;
            Score = 0;
            GameObject playerInstance = Instantiate(player, new Vector3(0, 1, 0), Quaternion.Euler(90, 0, 0)) as GameObject;
            GameObject.Find("Enemies").GetComponent<Enemies>().Reset();
            GameOver = false;
            foreach (var obj in GameObject.FindGameObjectsWithTag("Projectile"))
            {
                Destroy(obj.transform.parent.gameObject);
            }
        }

        public void LevelUp()
        {
            GameObject.Find("Enemies").GetComponent<Enemies>().Reset();
            foreach (var obj in GameObject.FindGameObjectsWithTag("Projectile"))
            {
                Destroy(obj.transform.parent.gameObject);
            }
            level++;
            StartCoroutine(ShowLevel());
        }

        IEnumerator ShowLevel()
        {
            levelObj.SetActive(true);
            levelObj.GetComponent<Text>().text = "Level " + level;
            yield return new WaitForSeconds(1f);
            levelObj.SetActive(false);
        }
    }

    public enum MovementScheme { Pulse, Sweep };
}
