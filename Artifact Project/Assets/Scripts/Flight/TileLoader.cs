using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Flight
{
    public class TileLoader : MonoBehaviour
    {
        public GameObject[] tileObjects;
        public float size;
        public int[] tileNum;
        Dictionary<Vector2, GameObject> tiles = new Dictionary<Vector2, GameObject>();
        Vector2 currentTile; Vector2 oldTile;

        // Use this for initialization
        void Start()
        {
            for (int x = -1*tileNum[0]; x <= tileNum[0]; x++)
            {
                for (int z = -1*tileNum[0]; z <= tileNum[0]; z++)
                {
                    if((x >= -tileNum[2] && x <= tileNum[2]) && (z >= -tileNum[2] && z <= tileNum[2]))
                        LoadTile(new Vector3(x, z), 0);
                    else if ((x >= -tileNum[1] && x <= tileNum[1]) && (z >= -tileNum[1] && z <= tileNum[1]))
                        LoadTile(new Vector3(x, z), 1);
                    else
                        LoadTile(new Vector3(x, z), 2);
                }
            }
            currentTile = Vector2.zero;
        }
        
        // Update is called once per frame
        void Update()
        {
            currentTile = new Vector2(Mathf.Floor(transform.position.x / size), Mathf.Floor(transform.position.z / size));
            if((int)currentTile.x - (int)oldTile.x == 1)
            {
                for (int z = -tileNum[0]; z <= tileNum[0]; z++)
                {
                    LoadTile(new Vector2(currentTile.x + tileNum[0], z + currentTile.y), 2);
                    DeleteTile(new Vector2(currentTile.x - (tileNum[0] + 1), z + currentTile.y));
                    if(z >= -tileNum[1] && z <= tileNum[1])
                    {
                        DeleteTile(new Vector2(currentTile.x + tileNum[1], z + currentTile.y));
                        LoadTile(new Vector2(currentTile.x + tileNum[1], z + currentTile.y), 1);
                        DeleteTile(new Vector2(currentTile.x - (tileNum[1] + 1), z + currentTile.y));
                        LoadTile(new Vector2(currentTile.x - (tileNum[1] + 1), z + currentTile.y), 0);
                    }
                    if (z >= -tileNum[2] && z <= tileNum[2])
                    {
                        DeleteTile(new Vector2(currentTile.x + tileNum[2], z + currentTile.y));
                        LoadTile(new Vector2(currentTile.x + tileNum[2], z + currentTile.y), 0);
                        DeleteTile(new Vector2(currentTile.x - (tileNum[2] + 1), z + currentTile.y));
                        LoadTile(new Vector2(currentTile.x - (tileNum[2] + 1), z + currentTile.y), 1);
                    }
                }
            }
            else if ((int)currentTile.x - (int)oldTile.x == -1)
            {
                for (int z = -tileNum[0]; z <= tileNum[0]; z++)
                {
                    LoadTile(new Vector2(currentTile.x - tileNum[0], z + currentTile.y), 2);
                    DeleteTile(new Vector2(currentTile.x + (tileNum[0] + 1), z + currentTile.y));
                    if (z >= -tileNum[1] && z <= tileNum[1])
                    {
                        DeleteTile(new Vector2(currentTile.x - tileNum[1], z + currentTile.y));
                        LoadTile(new Vector2(currentTile.x - tileNum[1], z + currentTile.y), 1);
                        DeleteTile(new Vector2(currentTile.x + (tileNum[1] + 1), z + currentTile.y));
                        LoadTile(new Vector2(currentTile.x + (tileNum[1] + 1), z + currentTile.y), 0);
                    }
                    if (z >= -tileNum[2] && z <= tileNum[2])
                    {
                        DeleteTile(new Vector2(currentTile.x - tileNum[2], z + currentTile.y));
                        LoadTile(new Vector2(currentTile.x - tileNum[2], z + currentTile.y), 0);
                        DeleteTile(new Vector2(currentTile.x + (tileNum[2] + 1), z + currentTile.y));
                        LoadTile(new Vector2(currentTile.x + (tileNum[2] + 1), z + currentTile.y), 1);
                    }
                }
            }
            if ((int)currentTile.y - (int)oldTile.y == 1)
            {
                for (int x = -tileNum[0]; x <= tileNum[0]; x++)
                {
                    LoadTile(new Vector2(x + currentTile.x, currentTile.y + tileNum[0]), 2);
                    DeleteTile(new Vector2(x + currentTile.x, currentTile.y - (tileNum[0] + 1)));
                    if (x >= -tileNum[1] && x <= tileNum[1])
                    {
                        DeleteTile(new Vector2(x + currentTile.x, currentTile.y + tileNum[1]));
                        LoadTile(new Vector2(x + currentTile.x, currentTile.y + tileNum[1]), 1);
                        DeleteTile(new Vector2(x + currentTile.x, currentTile.y - (tileNum[1] + 1)));
                        LoadTile(new Vector2(x + currentTile.x, currentTile.y - (tileNum[1] + 1)), 0);
                    }
                    if (x >= -tileNum[2] && x <= tileNum[2])
                    {
                        DeleteTile(new Vector2(x + currentTile.x, currentTile.y + tileNum[2]));
                        LoadTile(new Vector2(x + currentTile.x, currentTile.y + tileNum[2]), 0);
                        DeleteTile(new Vector2(x + currentTile.x, currentTile.y - (tileNum[2] + 1)));
                        LoadTile(new Vector2(x + currentTile.x, currentTile.y - (tileNum[2] + 1)), 1);
                    }
                }
            }
            else if ((int)currentTile.y - (int)oldTile.y == -1)
            {
                for (int x = -tileNum[0]; x <= tileNum[0]; x++)
                {
                    LoadTile(new Vector2(x + currentTile.x, currentTile.y - tileNum[0]), 2);
                    DeleteTile(new Vector2(x + currentTile.x, currentTile.y + (tileNum[0] + 1)));
                    if (x >= -tileNum[1] && x <= tileNum[1])
                    {
                        DeleteTile(new Vector2(x + currentTile.x, currentTile.y - tileNum[1]));
                        LoadTile(new Vector2(x + currentTile.x, currentTile.y - tileNum[1]), 1);
                        DeleteTile(new Vector2(x + currentTile.x, currentTile.y + (tileNum[1] + 1)));
                        LoadTile(new Vector2(x + currentTile.x, currentTile.y + (tileNum[1] + 1)), 0);
                    }
                    if (x >= -tileNum[2] && x <= tileNum[2])
                    {
                        DeleteTile(new Vector2(x + currentTile.x, currentTile.y - tileNum[2]));
                        LoadTile(new Vector2(x + currentTile.x, currentTile.y - tileNum[2]), 0);
                        DeleteTile(new Vector2(x + currentTile.x, currentTile.y + (tileNum[2] + 1)));
                        LoadTile(new Vector2(x + currentTile.x, currentTile.y + (tileNum[2] + 1)), 1);
                    }
                }
            }
            oldTile = currentTile;
        }
        

        void LoadTile(Vector2 position, int tile)
        {
            GameObject clone = Instantiate(tileObjects[tile], new Vector3(position.x, 0, position.y), Quaternion.identity) as GameObject;
            clone.transform.parent = GameObject.Find("Terrain").transform;
            clone.transform.localScale = new Vector3(size, size, size);
            clone.transform.position = new Vector3(position.x, 0, position.y) * size;
            if (!tiles.ContainsKey(position))
                tiles.Add(position, clone);
        }

        void DeleteTile(Vector2 position)
        {
            GameObject clone;
            if (tiles.TryGetValue(position, out clone))
            {
                tiles.Remove(position);
                Destroy(clone);
            }
        }
    }
}
