using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Runner
{
    public class Enemies : MonoBehaviour
    {
        public Vector2 enemyBlockNum;
        public Vector2 enemyBlockSize;
        public int enemyCount;
        
        public GameObject enemyPrefab;
        public World world;

        World.EnemyOrganization organization;
        MovementScheme movementScheme;

        bool sweepLeft;
        public bool SweepLeft{ get{ return sweepLeft; } set{ sweepLeft = value; } }

        public Dictionary<Vector2, Enemy> enemyArray = new Dictionary<Vector2, Enemy>();

        // Use this for initialization
        void Start()
        {
            world = GameObject.Find("World").GetComponent<World>();
            organization = world.organization;
            movementScheme = world.movementScheme;

            Organize();
        }

        // Update is called once per frame
        void Update()
        {
            enemyCount = GameObject.Find("Enemies").transform.childCount;
            if (enemyCount == 0 && !world.GameOver)
                world.LevelUp();
        }

        void Organize()
        {
            SweepLeft = false;
            if (organization == World.EnemyOrganization.Block)
            {
                for (int i = 0; i < enemyBlockNum.y; i++)
                {
                    for (int j = 0; j < enemyBlockNum.x; j++)
                    {
                        Vector3 enemyPos = new Vector3(-enemyBlockSize.x / 2 + enemyBlockSize.x / enemyBlockNum.x * j, 1, 17 - enemyBlockSize.y / enemyBlockNum.y * i);
                        GameObject enemy = Instantiate(enemyPrefab, enemyPos, Quaternion.Euler(90, 0, 0)) as GameObject;
                        enemy.transform.parent = transform;
                        enemy.GetComponent<Animator>().SetInteger("EnemyID", i);
                        enemy.GetComponent<Enemy>().movementScheme = movementScheme;
                        enemy.GetComponent<Enemy>().level = i;
                        enemy.GetComponent<Enemy>().column = j;
                        enemyArray.Add(new Vector2(i, j), enemy.GetComponent<Enemy>());
                        if(i == (int)enemyBlockNum.y - 1)
                            enemy.GetComponent<Enemy>().canFire = true;
                    }
                }
            }
            StartCoroutine(FireRandom());
        }

        public void Reset()
        {
            List<Vector2> tempList = new List<Vector2>();
            foreach (KeyValuePair<Vector2, Enemy> entry in enemyArray)
            {
                GameObject obj = entry.Value.gameObject;
                tempList.Add(entry.Key);
                Destroy(obj);
            }
            foreach (var item in tempList)
            {
                enemyArray.Remove(item);
            }
            Organize();
        }

        IEnumerator FireRandom()
        {
            while (enemyArray.Count>0)
            {
                Vector2[] keyArray = new Vector2[enemyArray.Count];
                enemyArray.Keys.CopyTo(keyArray, 0);
                int rand = Random.Range(0, enemyArray.Count);
                Enemy selected;
                if (enemyArray.TryGetValue(keyArray[rand], out selected))
                    selected.Fire();
                yield return new WaitForSeconds(.5f / world.level);
            }
        }
    }
}