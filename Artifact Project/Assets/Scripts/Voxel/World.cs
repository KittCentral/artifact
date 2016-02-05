using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Voxel
{
    public class World : MonoBehaviour
    {
        public string name = "world";

        public Dictionary<WorldPos, Chunk> chunks = new Dictionary<WorldPos, Chunk>();
        public GameObject chunkPrefab;

        public void CreateChunk(WorldPos pos)
        {
            GameObject newChunkObject = Instantiate(chunkPrefab, new Vector3(pos.x, pos.y, pos.z), Quaternion.Euler(Vector2.zero)) as GameObject;
            Chunk newChunk = newChunkObject.GetComponent<Chunk>();
            newChunk.posC = pos;
            newChunk.world = this;
            chunks.Add(pos, newChunk);
            var terrainGen = new TerrainGen();
            newChunk = terrainGen.ChunkGen(newChunk);
            newChunk.SetBlocksUnmodified();
            bool loaded = Serialization.Load(newChunk);
        }

        public void DestroyChunk(WorldPos pos)
        {
            Chunk chunk = null;
            if (chunks.TryGetValue(pos, out chunk))
            {
                Serialization.SaveChunk(chunk);
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

                UpdateIfEqual(pos.x - chunk.posC.x, 0, new WorldPos(pos.x - 1, pos.y, pos.z));
                UpdateIfEqual(pos.x - chunk.posC.x, Chunk.chunkSize - 1, new WorldPos(pos.x + 1, pos.y, pos.z));
                UpdateIfEqual(pos.y - chunk.posC.y, 0, new WorldPos(pos.x, pos.y - 1, pos.z));
                UpdateIfEqual(pos.y - chunk.posC.y, Chunk.chunkSize - 1, new WorldPos(pos.x, pos.y + 1, pos.z));
                UpdateIfEqual(pos.z - chunk.posC.z, 0, new WorldPos(pos.x, pos.y, pos.z - 1));
                UpdateIfEqual(pos.z - chunk.posC.z, Chunk.chunkSize - 1, new WorldPos(pos.x, pos.y, pos.z + 1));
            }
        }

        void UpdateIfEqual(int value1, int value2, WorldPos pos)
        {
            if (value1 == value2)
            {
                Chunk chunk = GetChunk(pos);
                if (chunk != null)
                    chunk.update = true;
            }
        }
    } 
}
