using UnityEngine;
using System.Collections;
using SimplexNoise;

namespace Voxel
{
    public class TerrainGen
    {
        float stoneBaseHeight = -24;
        float stoneBaseNoise = 0.05f;
        float stoneBaseNoiseHeight = 4;

        float stoneMountainHeight = 48;
        float stoneMountainFrequency = 0.008f;
        float stoneMinHeight = -12;

        float dirtBaseHeight = 1;
        float dirtNoise = 0.04f;
        float dirtNoiseHeight = 3;

        float caveFrequency = 0.025f;
        int caveSize = 7;

        float treeFrequency = 0.2f;
        int treeDensity = 3;

        public Chunk ChunkGen(Chunk chunk)
        {
            for (int x = chunk.posC.x; x < chunk.posC.x + Chunk.chunkSize; x++)
            {
                for (int z = chunk.posC.z; z < chunk.posC.z + Chunk.chunkSize; z++)
                {
                    chunk = ChunkColumnGen(chunk, x, z);
                }
            }
            return chunk;
        }

        public Chunk ChunkColumnGen(Chunk chunk, int x, int z)
        {
            int stoneHeight = Mathf.FloorToInt(stoneBaseHeight);
            stoneHeight += GetNoise(new WorldPos(x, 0, z), stoneMountainFrequency, Mathf.FloorToInt(stoneMountainHeight));
            if (stoneHeight < stoneMinHeight)
                stoneHeight = Mathf.FloorToInt(stoneMinHeight);
            stoneHeight += GetNoise(new WorldPos(x, 0, z), stoneBaseNoise, Mathf.FloorToInt(stoneBaseNoiseHeight));
            int dirtHeight = stoneHeight + Mathf.FloorToInt(dirtBaseHeight);
            dirtHeight += GetNoise(new WorldPos(x, 100, z), dirtNoise, Mathf.FloorToInt(dirtNoiseHeight));
            for (int y = chunk.posC.y; y < chunk.posC.y + Chunk.chunkSize; y++)
            {
                int caveChance = GetNoise(new WorldPos(x, y, z), caveFrequency, 100);
                Block block = new Block();
                if (chunk.world.queue.TryGetValue(new WorldPos(x, y, z), out block))
                {
                    SetBlock(new WorldPos(x, y, z), block, chunk);
                    chunk.world.queue.Remove(new WorldPos(x, y, z));
                }
                else if (y <= stoneHeight && caveSize < caveChance)
                    SetBlock(new WorldPos(x, y, z), new BlockStone(), chunk);
                else if (y <= dirtHeight && caveSize < caveChance)
                {
                    SetBlock(new WorldPos(x, y, z), new BlockGrass(), chunk);
                    if (y == dirtHeight && GetNoise(new WorldPos(x, 0, z), treeFrequency, 100) < treeDensity)
                        CreateTree(new WorldPos(x, y + 1, z), chunk);
                }
                else
                    SetBlock(new WorldPos(x, y, z), new BlockAir(), chunk);
            }
            return chunk;
        }

        public static int GetNoise(WorldPos pos, float scale, int max)
        {
            return Mathf.FloorToInt((Noise.Generate(pos.x * scale, pos.y * scale, pos.z * scale) + 1.0f) * (max / 2.0f));
        }

        public static void SetBlock(WorldPos pos, Block block, Chunk chunk, bool replace = false)
        {
            pos.x -= chunk.posC.x;
            pos.y -= chunk.posC.y;
            pos.z -= chunk.posC.z;
            //if(Chunk.InRange(pos.x) && Chunk.InRange(pos.y) && Chunk.InRange(pos.z))
            //{
                if (replace || chunk.blocks[pos.x, pos.y, pos.z] == null)
                    chunk.SetBlock(pos, block);
            //}
        }

        void CreateTree(WorldPos pos, Chunk chunk)
        {
            for (int xi = -2; xi <= 2; xi++)
            {
                for (int yi = 4; yi <= 8; yi++)
                {
                    for (int zi = -2; zi <= 2; zi++)
                    {
                        SetBlock(new WorldPos(pos.x + xi, pos.y + yi, pos.z + zi), new BlockLeaves(), chunk, true);
                    }
                }
            }
            for (int yt = 0; yt < 6; yt++)
            {
                SetBlock(new WorldPos(pos.x, pos.y + yt, pos.z), new BlockWood(), chunk, true);
            }
        }
    }
}
