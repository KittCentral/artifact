using UnityEngine;
using System.Collections;

public class EnemyManager : MonoBehaviour
{
    public GameObject enemy;
    public float spawnTime = 3f;
    public Transform[] spawnPoints;

    void Start()
    {
        InvokeRepeating("Spawn", spawnTime, spawnTime);
    }

    void Spawn()
    {
        int spawnPointIndex = Random.Range(0, spawnPoints.Length);

        Instantiate(enemy, spawnPoints[spawnPointIndex].position, spawnPoints[spawnPointIndex].rotation);
    }
	
}
