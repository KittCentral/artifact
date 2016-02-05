using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Voxel
{
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]
    [RequireComponent(typeof(MeshCollider))]

    public class Chunk : MonoBehaviour
    {
        public Block[,,] blocks = new Block[chunkSize, chunkSize, chunkSize];
        public World world;
        public WorldPos posC;
        public static int chunkSize = 16;
        public bool update = false;
        public bool rendered;

        MeshFilter filter;
        MeshCollider coll;

        // Use this for initialization
        void Start()
        {
            filter = gameObject.GetComponent<MeshFilter>();
            coll = gameObject.GetComponent<MeshCollider>();
        }

        // Update is called once per frame
        void Update()
        {
            if (update)
            {
                update = false;
                UpdateChunk();
            }
        }

        public void SetBlock(WorldPos pos, Block block)
        {
            if (InRange(pos.x) && InRange(pos.y) && InRange(pos.z))
                blocks[pos.x, pos.y, pos.z] = block;
            else
                world.SetBlock(new WorldPos(posC.x + pos.x, posC.y + pos.y, posC.z + pos.z), block);
        }

        public Block GetBlock(WorldPos pos)
        {
            if (InRange(pos.x) && InRange(pos.y) && InRange(pos.z))
                return blocks[pos.x, pos.y, pos.z];
            return world.GetBlock(new WorldPos(posC.x + pos.x, posC.y + pos.y, posC.z + pos.z));
        }

        public static bool InRange(int i)
        {
            if (i < 0 || i >= chunkSize)
                return false;
            return true;
        }

        public void SetBlocksUnmodified()
        {
            foreach (Block block in blocks)
            {
                block.changed = false;
            }
        }

        void UpdateChunk()
        {
            rendered = true;
            MeshData meshData = new MeshData();
            for (int x = 0; x < chunkSize; x++)
            {
                for (int y = 0; y < chunkSize; y++)
                {
                    for (int z = 0; z < chunkSize; z++)
                    {
                        meshData = blocks[x, y, z].Blockdata(this, x, y, z, meshData);
                    }
                }
            }
            RenderMesh(meshData);
        }

        void RenderMesh(MeshData meshData)
        {
            filter.mesh.Clear();
            filter.mesh.vertices = meshData.vertices.ToArray();
            filter.mesh.triangles = meshData.triangles.ToArray();
            filter.mesh.uv = meshData.uv.ToArray();
            filter.mesh.RecalculateNormals();

            coll.sharedMesh = null;
            Mesh mesh = new Mesh();
            mesh.vertices = meshData.colVertices.ToArray();
            mesh.triangles = meshData.colTriangles.ToArray();
            mesh.RecalculateNormals();
            coll.sharedMesh = mesh;
        }
    }
}