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
        public bool dynamic;
        public bool approximate;
        public bool distorted;
        public bool zeroed;
        public bool show;
        public int chunkSize;
        public int chunkHeight;
        public float spacing;
        public float maxHeight;
        public Vector2 posOffset = new Vector2();
        public TriWorld world;
        public GameObject dot;
        bool[,,] hits;

        void Start()
        {
            hits = new bool[chunkSize, chunkSize, chunkSize - 1];
            GenerateMesh(chunkSize);
            Vector3 testPoint = new Vector3(1f, -2f / 3f, 2f);
            print(PosToHex(testPoint).x + ", " + PosToHex(testPoint).y + ", " + PosToHex(testPoint).z);
            print(HexToPos(PosToHex(testPoint)));
        }

        void Update()
        {
            if(dynamic)
                GenerateMesh(chunkSize);
        }

        void GenerateMesh(int wid)
        {
            float startTime = Time.time;

            List<Vector3[]> verts = new List<Vector3[]>();
            List<int> tris = new List<int>();
            List<Vector2> uvs = new List<Vector2>();
            for (int z = 0; z < wid - 1; z++)
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
                    if (!zeroed)
                        currentPoint.y = GetNoise(new Vector3(currentPoint.x, Time.frameCount, currentPoint.z), .04f, (int)maxHeight) + 
                            //GetNoise(new Vector3(currentPoint.x, Time.frameCount, currentPoint.z), .5f, (int)maxHeight / 2) +
                            GetNoise(new Vector3(currentPoint.x, Time.frameCount, currentPoint.z), .002f, (int)maxHeight * 25);
                    if (approximate)
                    {
                        float tempH = Mathf.Round(currentPoint.y);
                        currentPoint.y += (2 * x + (offset == 1 ? 2 : 3)) % 3;
                        currentPoint.y = (tempH - Mathf.Round(currentPoint.y))/3 + tempH;
                    }
                    
                    if (distorted)
                        currentPoint = Distort(currentPoint);
                    verts[z][x] = currentPoint;
                    uvs.Add(new Vector2(x, z));
                    int currentX = x + (1 - offset);
                    if (!(currentX <= 0 || z <= 0 || currentX >= wid))
                    {
                        tris.Add(x + z * wid);
                        tris.Add(currentX + (z - 1) * wid);
                        tris.Add((currentX - 1) + (z - 1) * wid);
                    }
                    if (!(x <= 0 || z <= 0))
                    {
                        tris.Add(x + z * wid);
                        tris.Add((currentX - 1) + (z - 1) * wid);
                        tris.Add((x - 1) + z * wid);
                    }
                }
            }

            Vector3[] uVerts = new Vector3[wid * (wid - 1)];
            int i = 0;
            foreach (Vector3[] v in verts)
            {
                v.CopyTo(uVerts, i * wid);
                i++;
            }
            if (show)
                ShowVertices(uVerts);
            /*
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
            */
        }

        

        public static Vector3 Distort(Vector3 input)
        {
            input.x += GetNoise(new Vector3(input.x + 5000, input.y, input.z), .15f, 1);
            input.y += GetNoise(new Vector3(input.x, input.y + 5000, input.z), .15f, 1);
            input.z += GetNoise(new Vector3(input.x, input.y, input.z + 5000), .15f, 1);
            return input;
        }

        public void ShowVertices(Vector3[] basePoints)
        {
            List<Vector3> verts = new List<Vector3>();
            List<int> tris = new List<int>();
            List<Vector2> uvs = new List<Vector2>();

            int i = 0;
            foreach (var basePoint in basePoints)
            {
                for (int y = 0; y < chunkHeight; y++)
                {
                    Vector3 center = new Vector3(basePoint.x, basePoint.y + y, basePoint.z);
                    if (Land(center))
                    {
                        GameObject copy = Instantiate(dot, center, new Quaternion(0, 0, 0, 0)) as GameObject;
                        copy.transform.parent = gameObject.transform;
                        hits[PosToHex(center).x, PosToHex(center).y, PosToHex(center).z] = true;
                    }
                }
            }
            for (int x = 0; x < chunkSize; x++)
            {
                for (int y = 0; y < chunkSize; y++)
                {
                    for (int z = 0; z < chunkSize-1; z++)
                    {
                        if(hits[x,y,z])
                            FaceBuilder(HexToPos(new WorldPos(x, y, z)), ref verts, ref tris, ref i);
                    }
                }
            }
            MeshFilter filter = gameObject.GetComponent<MeshFilter>();
            filter.mesh.Clear();
            filter.mesh.vertices = verts.ToArray();
            filter.mesh.triangles = tris.ToArray();
            filter.mesh.uv = uvs.ToArray();
            filter.mesh.RecalculateBounds();
            filter.mesh.RecalculateNormals();
        }

        bool Land(Vector3 point)
        {
            if(zeroed)
                return point.y <= 3;
            else
                return GetNoise(point, .05f, 15) < .75f;
        }

        void FaceBuilder(Vector3 center, ref List<Vector3> verts, ref List<int> tris, ref int i)
        {
            if (TopFlatCheck(center))
            {
                verts.Add(center);
                verts.Add(center + new Vector3(1.5f, 0, 1f));
                verts.Add(center + new Vector3(1.5f, 0, -1f));
                for (int index = 0; index < 3; index++) { tris.Add(index + 3 * i);}
                i++;
            }
            if(SideFlatCheck(center))
            {
                verts.Add(center);
                verts.Add(center + new Vector3(1.5f, 0, -1f));
                verts.Add(center + new Vector3(0, 0, -2f));
                for (int index = 0; index < 3; index++) { tris.Add(index + 3 * i);}
                i++;
            }
            if (TopFlatCheckD(center))
            {
                verts.Add(center);
                verts.Add(center + new Vector3(1.5f, 0, -1f));
                verts.Add(center + new Vector3(1.5f, 0, 1f));
                for (int index = 0; index < 3; index++) { tris.Add(index + 3 * i); }
                i++;
            }
            if (SideFlatCheckD(center))
            {
                verts.Add(center);
                verts.Add(center + new Vector3(0, 0, -2f));
                verts.Add(center + new Vector3(1.5f, 0, -1f));
                for (int index = 0; index < 3; index++) { tris.Add(index + 3 * i); }
                i++;
            }
        }

        bool TopFlatCheck(Vector3 center)
        {
            return (CheckHit(center + new Vector3(1.5f, 0, 1f)) && CheckHit(center + new Vector3(1.5f, 0, -1f)) && 
                !CheckHit(center + new Vector3(1f, 1f/3f, 0)));
        }

        bool SideFlatCheck(Vector3 center)
        {
            return (CheckHit(center + new Vector3(1.5f, 0, -1f)) && CheckHit(center + new Vector3(0, 0, -2f)) && 
                !CheckHit(center + new Vector3(.5f, 2f/3f, -1f)) && !TopFlatCheck(center + new Vector3(-.5f, 1f / 3f, -1f)));
        }

        bool TopFlatCheckD(Vector3 center)
        {
            return (CheckHit(center + new Vector3(1.5f, 0, 1f)) && CheckHit(center + new Vector3(1.5f, 0, -1f)) &&
                !CheckHit(center + new Vector3(1f, -(2f / 3f), 0)) && !SideFlatCheckD(center + new Vector3(.5f, -(1f / 3f), 1f)));
        }

        bool SideFlatCheckD(Vector3 center)
        {
            return (CheckHit(center + new Vector3(1.5f, 0, -1f)) && CheckHit(center + new Vector3(0, 0, -2f)) &&
                !CheckHit(center + new Vector3(.5f, -(1f / 3f), -1f)));
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

        bool CheckHit(Vector3 point)
        {
            bool output;
            try { output = hits[PosToHex(point).x, PosToHex(point).y, PosToHex(point).z]; }
            catch { output = false; }
            return output;
        }

        WorldPos PosToHex (Vector3 point)
        {
            WorldPos output = new WorldPos(Mathf.CeilToInt(point.x), Mathf.CeilToInt(point.y), (int)point.z);
            output.x -= (int)posOffset.x;
            output.z -= (int)posOffset.y;
            return output;
        }

        Vector3 HexToPos (WorldPos point)
        {
            point.x += (int)posOffset.x;
            point.z += (int)posOffset.y;
            float x = point.z % 2 == 0 ? point.x : point.x - 0.5f;
            float y = point.y - Mathf.Abs(((point.x + Mathf.Abs(point.z % 2f) + 1) % 3f) / 3f);
            Vector3 output = new Vector3(x,y,point.z);
            return output;
        }
    }
}
