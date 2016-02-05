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
                if (y <= stoneHeight)
                    chunk.SetBlock(new WorldPos(x - chunk.posC.x, y - chunk.posC.y, z - chunk.posC.z), new BlockStone());
                else if (y<=dirtHeight)
                    chunk.SetBlock(new WorldPos(x - chunk.posC.x, y - chunk.posC.y, z - chunk.posC.z), new BlockGrass());
                else
                    chunk.SetBlock(new WorldPos(x - chunk.posC.x, y - chunk.posC.y, z - chunk.posC.z), new BlockAir());
            }
            return chunk;
        }

        public static int GetNoise(WorldPos pos, float scale, int max)
        {
            return Mathf.FloorToInt((Noise.Generate(pos.x * scale, pos.y * scale, pos.z * scale) + 1.0f) * (max / 2.0f));
        }
    }
}
