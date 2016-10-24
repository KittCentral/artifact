using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Flight
{
    public class TileLoader : MonoBehaviour
    {
        public GameObject tile;
        public float size;
        public int tileNum;
        Dictionary<Vector2, GameObject> tiles = new Dictionary<Vector2, GameObject>();
        Vector2 currentTile; Vector2 oldTile;

        // Use this for initialization
        void Start()
        {
            for (int x = -tileNum; x <= tileNum; x++)
            {
                for (int z = -tileNum; z <= tileNum; z++)
                {
                    LoadTile(new Vector3(x, z));
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
                for (int z = -tileNum; z <= tileNum; z++)
                {
                    LoadTile(new Vector2(currentTile.x + tileNum, z + currentTile.y));
                    DeleteTile(new Vector2(currentTile.x - (tileNum + 1), z + currentTile.y));
                }
                print(currentTile);
            }
            else if ((int)currentTile.x - (int)oldTile.x == -1)
            {
                for (int z = -tileNum; z <= tileNum; z++)
                {
                    LoadTile(new Vector2(currentTile.x - tileNum, z + currentTile.y));
                    DeleteTile(new Vector2(currentTile.x + (tileNum + 1), z + currentTile.y));
                }
                print(currentTile);
            }
            if ((int)currentTile.y - (int)oldTile.y == 1)
            {
                for (int x = -tileNum; x <= tileNum; x++)
                {
                    LoadTile(new Vector2(x + currentTile.x, currentTile.y + tileNum));
                    DeleteTile(new Vector2(x + currentTile.x, currentTile.y - (tileNum + 1)));
                }
                print(currentTile);
            }
            else if ((int)currentTile.y - (int)oldTile.y == -1)
            {
                for (int x = -tileNum; x <= tileNum; x++)
                {
                    LoadTile(new Vector2(x + currentTile.x, currentTile.y - tileNum));
                    DeleteTile(new Vector2(x + currentTile.x, currentTile.y + (tileNum + 1)));
                }
                print(currentTile);
            }
            oldTile = currentTile;
        }

        void LoadTile(Vector2 position)
        {
            GameObject clone = Instantiate(tile, new Vector3(position.x, 0, position.y), Quaternion.identity) as GameObject;
            clone.transform.parent = GameObject.Find("Terrain").transform;
            clone.transform.localScale = new Vector3(size, size, size);
            clone.transform.position = new Vector3(position.x, 0, position.y) * size;
            if(!tiles.ContainsKey(position))
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
