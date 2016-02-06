using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimplexNoise;

namespace Voxel
{
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]
    [RequireComponent(typeof(MeshCollider))]

    public class TriChunk : MonoBehaviour
    {
        public bool colliderBool;
        public bool updateCrazy;
        public int chunkSize = 16;
        public float spacing = 1f;
        public float maxHeight = 3f;
        public Vector2 posOffset = new Vector2();

        // Use this for initialization
        void Start()
        {
            GenerateMesh(chunkSize);
        }

        // Update is called once per frame
        void Update()
        {
            if(updateCrazy)
                GenerateMesh(chunkSize);
        }

        void GenerateMesh(int wid)
        {
            float startTime = Time.time;

            List<Vector3[]> verts = new List<Vector3[]>();
            List<int> tris = new List<int>();
            List<Vector2> uvs = new List<Vector2>();

            for (int z = 0; z < wid; z++)
            {
                verts.Add(new Vector3[wid]);
                for (int x = 0; x < wid; x++)
                {
                    Vector3 currentPoint = new Vector3();
                    currentPoint.x = x * spacing + posOffset.x;
                    currentPoint.z = z * spacing + posOffset.y;
                    int offset = z % 2;
                    if (offset == 1)
                        currentPoint.x -= spacing * 0.5f;
                    currentPoint.y = Mathf.Floor(GetNoise(new Vector3(currentPoint.x, Time.frameCount, currentPoint.z), .04f, (int)maxHeight) + 
                        //GetNoise(new Vector3(currentPoint.x, Time.frameCount, currentPoint.z), .5f, (int)maxHeight / 2) +
                        GetNoise(new Vector3(currentPoint.x, Time.frameCount, currentPoint.z), .002f, (int)maxHeight * 25));
                    currentPoint = Distort(currentPoint);
                    verts[z][x] = currentPoint;
                    uvs.Add(new Vector2(x, z));
                    int currentX = x + (1 - offset);
                    if (!(currentX - 1 <= 0 || z <= 0 || currentX >= wid))
                    {
                        tris.Add(x + z * wid);
                        tris.Add(currentX + (z - 1) * wid);
                        tris.Add((currentX - 1) + (z - 1) * wid);
                    }
                    if (!(x - 1 <= 0 || z <= 0))
                    {
                        tris.Add(x + z * wid);
                        tris.Add((currentX - 1) + (z - 1) * wid);
                        tris.Add((x - 1) + z * wid);
                    }
                }
            }

            Vector3[] uVerts = new Vector3[wid * wid];
            int i = 0;
            foreach (Vector3[] v in verts)
            {
                v.CopyTo(uVerts, i * wid);
                i++;
            }
            MeshFilter filter = gameObject.GetComponent<MeshFilter>();
            filter.mesh.Clear();
            filter.mesh.vertices = uVerts;
            filter.mesh.triangles = tris.ToArray();
            filter.mesh.uv = uvs.ToArray();
            filter.mesh.RecalculateBounds();
            filter.mesh.RecalculateNormals();

            if (colliderBool)
            {
                MeshCollider coll = gameObject.GetComponent<MeshCollider>();
                coll.sharedMesh = null;
                Mesh mesh = new Mesh();
                mesh.vertices = uVerts;
                mesh.triangles = tris.ToArray();
                mesh.RecalculateNormals();
                coll.sharedMesh = mesh;
            }
        }

        public static Vector3 Distort(Vector3 input)
        {
            input.x += GetNoise(new Vector3(input.x + 5000, input.y, input.z), .15f, 1);
            input.y += GetNoise(new Vector3(input.x, input.y + 5000, input.z), .15f, 1);
            input.z += GetNoise(new Vector3(input.x, input.y, input.z + 5000), .15f, 1);
            return input;
        }

        public static float GetNoise(Vector3 pos, float scale, int max)
        {
            return (Noise.Generate(pos.x * scale, pos.y * scale, pos.z * scale) + 1.0f) * (max / 2.0f);
        }

        float GetHeight(float x, float z)
        {
            float y = Mathf.Min(0, maxHeight - Vector2.Distance(Vector2.zero, new Vector2(x, z)));
            return y;
        }
    }
}
