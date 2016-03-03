using UnityEngine;
using System.Collections.Generic;

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
        Vector3[] uVerts;

        float noiseScale = 0.05f;

        Vector3[] tetraPoints = { new Vector3(0, 0, 0), new Vector3(0, 0, 2), new Vector3(1.5f, 0, 1), new Vector3(1.5f, 0, -1),
            new Vector3(.5f, 2f / 3f, 1), new Vector3(.5f, -1f / 3f, 1),
            new Vector3(1, 1f / 3f, 0), new Vector3(1, -2f / 3f, 0)};
        static int[] tetra1 = { 0, 1, 2, 4 };
        static int[] tetra2 = { 0, 1, 2, 5 };
        static int[] tetra3 = { 0, 2, 3, 6 };
        static int[] tetra4 = { 0, 2, 3, 7 };
        static int[][] tetras = { tetra1, tetra2, tetra3, tetra4 };

        void Start()
        {
            hits = new bool[chunkSize, chunkSize, chunkSize - 1];
            GenerateMesh(chunkSize);
            
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
                        //currentPoint.y = GetNoise(new Vector3(currentPoint.x, Time.frameCount, currentPoint.z), .04f, (int)maxHeight) + 
                            //GetNoise(new Vector3(currentPoint.x, Time.frameCount, currentPoint.z), .5f, (int)maxHeight / 2) +
                            //GetNoise(new Vector3(currentPoint.x, Time.frameCount, currentPoint.z), .002f, (int)maxHeight * 25);
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

            uVerts = new Vector3[wid * (wid - 1)];
            
            int i = 0;
            foreach (Vector3[] v in verts)
            {
                v.CopyTo(uVerts, i * wid);
                i++;
            }
            ShowVertices(uVerts);
        }

        void OnDrawGizmosSelected ()
        {
            for(int i = 0; i < chunkSize; i++)
            {
                for (int j = 0; j < chunkSize; j++)
                {
                    for (int k = 0; k < chunkSize - 1; k++)
                    {
                        if (hits[i, j, k])
                        {
                            Vector3 vert = HexToPos(new WorldPos(i, j, k));
                            vert = new Vector3(vert.x * Mathf.Sqrt(3) / 1.5f, vert.y * 2, vert.z);
                            Gizmos.color = Color.gray;
                            Gizmos.DrawSphere(vert, .2f);
                            if (show)
                            {
                                Vector3 dir = Procedural.Noise.noiseMethods[1][2](vert, noiseScale).derivative.normalized;
                                Gizmos.color = Color.yellow;
                                Gizmos.DrawRay(vert, dir);
                            }
                        }
                    }
                }
            }
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
                    if (Land(center) && GradientCheck(center))
                    {
                        hits[PosToHex(center).x, PosToHex(center).y, PosToHex(center).z] = true;
                        //GameObject copy = Instantiate(dot, new Vector3(center.x * Mathf.Sqrt(3) / 1.5f, center.y * 2, center.z) , new Quaternion(0, 0, 0, 0)) as GameObject;
                        //copy.transform.parent = gameObject.transform;
                    }
                }
            }
            for (int x = 0; x < chunkSize; x++)
            {
                for (int y = 0; y < chunkSize; y++)
                {
                    for (int z = 0; z < chunkSize - 1; z++)
                    {
                        FaceBuilder(HexToPos(new WorldPos(x, y, z)), ref verts, ref tris, ref i);
                    }
                }
            }
            i = 0;
            foreach (var vert in verts)
            {
                verts[i] = new Vector3(vert.x * Mathf.Sqrt(3) / 1.5f, vert.y * 2, vert.z);
                i++;
            }
            MeshFilter filter = gameObject.GetComponent<MeshFilter>();
            filter.mesh.Clear();
            filter.mesh.vertices = verts.ToArray();
            filter.mesh.triangles = tris.ToArray();
            filter.mesh.uv = uvs.ToArray();
            filter.mesh.RecalculateBounds();
            filter.mesh.RecalculateNormals();
        }

        bool GradientCheck(Vector3 point)
        {
            Vector3 normal = Procedural.Noise.noiseMethods[1][2](point, noiseScale).derivative.normalized * 2.2f;
            if (GetNoise(point + normal, noiseScale, 15) > .75f && GetNoise(point - normal, noiseScale, 15) < .75f)
                return true;
            return false;
        }

        bool Land(Vector3 point)
        {
            if(zeroed)
                return point.y <= 3;
            else
                return GetNoise(point, noiseScale, 15) < .75f;
        }

        void FaceBuilder(Vector3 center, ref List<Vector3> verts, ref List<int> tris, ref int i)
        {
            bool[] checks = new bool[8];
            bool pullout = true;
            int vertCount = verts.Count;
            for (int index = 0; index < 8; index++)
            {
                if (CheckHit(center + tetraPoints[index]))
                {
                    checks[index] = true;
                    pullout = false;
                }
                else
                    checks[index] = false;
                verts.Add(center + tetraPoints[index]);
            }
            if (pullout)
                return;
            for (int index = 1; index < 3; index++)
            {
                List<Vector3> points = new List<Vector3>();
                int fail = -1;
                for (int j = 0; j < 4; j++)
                {
                    if (checks[tetras[index][j]])
                        points.Add(center + tetraPoints[tetras[index][j]]);
                    else
                        fail = tetras[index][j];
                }
                if (points.Count == 4)
                {
                    List<int>[] triTemp = BuildTetra(index);
                    for(int triSet = 0; triSet < 4; triSet++)
                    {
                        foreach(var tri in triTemp[triSet])
                            tris.Add(tri + vertCount);
                    }
                    //GameObject copy = Instantiate(dot, new Vector3(center.x * Mathf.Sqrt(3) / 1.5f, center.y * 2, center.z), new Quaternion(0, 0, 0, 0)) as GameObject;
                    //copy.transform.parent = gameObject.transform;
                }
                else if (points.Count == 3)
                {

                    List<int> triTemp = BuildInnerFace(index, fail);
                    if(TriNormCheck(center, tetraPoints[fail]))
                        for (int tri = 0; tri < 3; tri++) { tris.Add(triTemp.ToArray()[tri] + vertCount); }
                    else
                        for (int tri = 0; tri < 3; tri++) { tris.Add(triTemp.ToArray()[2-tri] + vertCount); }
                }
            }
        }

        List<int>[] BuildTetra(int tetra)
        {
            switch (tetra)
            {
                case 0:
                    return new List<int>[] { new List<int> { 2, 1, 0 }, new List<int> { 2, 4, 1 }, new List<int> { 2, 0, 4 }, new List<int> { 0, 1, 4 } };
                case 1:
                    return new List<int>[] { new List<int> { 0, 1, 2 }, new List<int> { 2, 1, 5 }, new List<int> { 2, 5, 0 }, new List<int> { 0, 5, 1 } };
                case 2:
                    return new List<int>[] { new List<int> { 3, 2, 0 }, new List<int> { 2, 3, 6 }, new List<int> { 3, 0, 6 }, new List<int> { 0, 2, 6 } };
                case 3:
                    return new List<int>[] { new List<int> { 0, 2, 3 }, new List<int> { 3, 2, 7 }, new List<int> { 0, 3, 7 }, new List<int> { 2, 0, 7 } };
                default:
                    return new List<int>[] { };
            }
        }

        List<int> BuildInnerFace(int tetra, int fail)
        {
            switch (tetra)
            {
                case 0:
                    switch (fail)
                    {
                        case 0:
                            return new List<int> { 2, 1, 4 };
                        case 1:
                            return new List<int> { 2, 4, 0 };
                        case 2:
                            return new List<int> { 4, 1, 0 };
                        case 4:
                            return new List<int> { 2, 0, 1 };
                        default:
                            return new List<int> { };
                    }
                case 1:
                    switch (fail)
                    {
                        case 0:
                            return new List<int> { 2, 5, 1 };
                        case 1:
                            return new List<int> { 2, 0, 5 };
                        case 2:
                            return new List<int> { 5, 0, 1 };
                        case 5:
                            return new List<int> { 2, 1, 0 };
                        default:
                            return new List<int> { };
                    }
                case 2:
                    switch (fail)
                    {
                        case 0:
                            return new List<int> { 2, 6, 3 };
                        case 2:
                            return new List<int> { 3, 6, 0 };
                        case 3:
                            return new List<int> { 6, 2, 0 };
                        case 6:
                            return new List<int> { 3, 0, 2 };
                        default:
                            return new List<int> { };
                    }
                case 3:
                    switch (fail)
                    {
                        case 0:
                            return new List<int> { 3, 7, 2 };
                        case 2:
                            return new List<int> { 0, 7, 3 };
                        case 3:
                            return new List<int> { 2, 7, 0 };
                        case 7:
                            return new List<int> { 0, 3, 2 };
                        default:
                            return new List<int> { };
                    }
                default:
                    return new List<int> { };
            }
        }

        bool TriNormCheck(Vector3 center, Vector3 normal)
        {
            return 90 > Vector3.Angle(Procedural.Noise.noiseMethods[1][2](center, noiseScale).derivative, normal);
        }

        public static float GetNoise(Vector3 pos, float scale, int max)
        {
            return Procedural.Noise.noiseMethods[1][2](pos, scale).value * 20 + 10;
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
            float y = point.y - Mathf.Abs((((point.x + Mathf.Abs(point.z % 2f) - (int)posOffset.x - 15)) % 3f) / 3f);
            Vector3 output = new Vector3(x,y,point.z);
            return output;
        }
    }
}
