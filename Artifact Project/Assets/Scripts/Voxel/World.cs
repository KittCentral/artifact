using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Voxel
{
    public class World : MonoBehaviour
    {
        public Dictionary<WorldPos, Chunk> chunks = new Dictionary<WorldPos, Chunk>();
        public GameObject chunkPrefab;

        // Use this for initialization
        void Start()
        {
            for (int x = -2; x < 2; x++)
            {
                for (int y = -1; y < 1; y++)
                {
                    for (int z = -1; z < 1; z++)
                    {
                        CreateChunk(new WorldPos(x * Chunk.chunkSize, y * Chunk.chunkSize, z * Chunk.chunkSize));
                    }
                }
            }
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void CreateChunk(WorldPos pos)
        {
            GameObject newChunkObject = Instantiate(chunkPrefab, new Vector3(pos.x, pos.y, pos.z), Quaternion.Euler(Vector2.zero)) as GameObject;
            Chunk newChunk = newChunkObject.GetComponent<Chunk>();
            newChunk.posC = pos;
            newChunk.world = this;
            chunks.Add(pos, newChunk);
            for (int xi = 0; xi < Chunk.chunkSize; xi++)
            {
                for (int yi = 0; yi < Chunk.chunkSize; yi++)
                {
                    for (int zi = 0; zi < Chunk.chunkSize; zi++)
                    {
                        if (yi <= 7)
                            SetBlock(new WorldPos(pos.x + xi, pos.y + yi, pos.z + zi), new BlockStone());
                        else
                            SetBlock(new WorldPos(pos.x + xi, pos.y + yi, pos.z + zi), new BlockAir());
                    }
                }
            }
        }

        public void DestroyChunk(WorldPos pos)
        {
            Chunk chunk = null;
            if (chunks.TryGetValue(pos, out chunk))
            {
                Destroy(chunk.gameObject);
                chunks.Remove(pos);
            }
        }

        public Chunk GetChunk(WorldPos pos)
        {
            WorldPos position = new WorldPos();
            float floatChunkSize = Chunk.chunkSize;
            position.x = Mathf.FloorToInt(pos.x / floatChunkSize) * Chunk.chunkSize;
            position.y = Mathf.FloorToInt(pos.y / floatChunkSize) * Chunk.chunkSize;
            position.z = Mathf.FloorToInt(pos.z / floatChunkSize) * Chunk.chunkSize;
            Chunk containerChunk = null;
            chunks.TryGetValue(position, out containerChunk);
            return containerChunk;
        }

        public Block GetBlock(WorldPos pos)
        {
            Chunk containerChunk = GetChunk(pos);
            if (containerChunk != null)
            {
                Block block = containerChunk.GetBlock(new WorldPos(pos.x - containerChunk.posC.x, pos.y - containerChunk.posC.y, pos.z - containerChunk.posC.z));
                return block;
            }
            else
                return new BlockAir();
        }

        public void SetBlock(WorldPos pos, Block block)
        {
            Chunk chunk = GetChunk(pos);
            if(chunk != null)
            {
                chunk.SetBlock(new WorldPos(pos.x - chunk.posC.x, pos.y - chunk.posC.y, pos.z - chunk.posC.z), block);
                chunk.update = true;
            }
        }
    } 
}
