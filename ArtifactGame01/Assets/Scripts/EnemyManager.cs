using UnityEngine;
using System.Collections;

public class EnemyManager : MonoBehaviour {

    public GameObject Enemy;
    public float spawnTime = 3f;
    public Transform[] SpawnPoints;

    void Start ()
    {
        InvokeRepeating("Spawn", spawnTime, spawnTime);
	}
	
	
	void Spawn()
    {
        int spawnPointIndex = Random.Range(0, SpawnPoints.Length);
        Instantiate(Enemy, SpawnPoints[spawnPointIndex].position, SpawnPoints[spawnPointIndex].rotation);
	}
}
