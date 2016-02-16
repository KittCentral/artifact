using UnityEngine;

public struct NoiseSample { public float value; public Vector3 derivative; }

public delegate NoiseSample NoiseMethod(Vector3 point, float frequency);

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class SurfaceCreator : MonoBehaviour
{
    [Range(1, 200)]
    public int res = 10;
    int currentRes;

    public Vector3 offset;

    public Vector3 rotation;

    [Range(0f, 2f)]
    public float strength = 1f;

    public float frequency = 1f;

    [Range(1, 8)]
    public int octaves = 1;

    [Range(1f, 4f)]
    public float lacunarity = 2f;

    [Range(0f, 1f)]
    public float persistence = 0.5f;

    [Range(1, 3)]
    public int dimensions = 3;

    public Procedural.NoiseMethodType type;

    public Gradient coloring;

    public bool coloringForStrength = true;

    public bool damping = false;

    public bool showNormals;

    Mesh mesh;
    Vector3[] vertices;
    Color[] colors;
    Vector3[] normals;

    void OnEnable ()
    {
        if (mesh == null)
        {
            mesh = new Mesh();
            mesh.name = "Surface Mesh";
            GetComponent<MeshFilter>().mesh = mesh;
        }
        Refresh();
    }

    void OnDrawGizmosSelected()
    {
        if (showNormals && vertices != null)
        {
            Gizmos.color = Color.yellow;
            for (int v = 0; v < vertices.Length; v++)
            {
                Gizmos.DrawRay(5 * new Vector3(-1 * vertices[v].x, -1 * vertices[v].y, vertices[v].z), new Vector3(-1 * normals[v].x, -1 * normals[v].y, normals[v].z) / res);
            }
        }
    }

    public void Refresh()
    {
        if (res != currentRes)
            CreateGrid();        
        Quaternion q = Quaternion.Euler(rotation);
        Vector3 point00 = q * transform.TransformPoint(new Vector3(-0.5f, -0.5f)) + offset;
        Vector3 point10 = q * transform.TransformPoint(new Vector3( 0.5f, -0.5f)) + offset;
        Vector3 point01 = q * transform.TransformPoint(new Vector3(-0.5f,  0.5f)) + offset;
        Vector3 point11 = q * transform.TransformPoint(new Vector3( 0.5f,  0.5f)) + offset;

        float amplitude = damping ? strength / frequency : strength;
        Procedural.NoiseMethod method = Procedural.Noise.noiseMethods[(int)type][dimensions - 1];
        int n = 0;
        for (int y = 0; y <= res; y++)
        {
            Vector3 point0 = Vector3.Lerp(point00, point01, (float)y / res);
            Vector3 point1 = Vector3.Lerp(point10, point11, (float)y / res);
            for (int x = 0; x <= res; x++, n++)
            {
                Vector3 point = Vector3.Lerp(point0, point1, (float)x / res);
                float sample = Procedural.Noise.Sum(method, point, frequency, octaves, lacunarity, persistence);
                sample = type == Procedural.NoiseMethodType.Value ? (sample - 0.5f) : (sample * 0.5f);
                if (coloringForStrength)
                {
                    colors[n] = coloring.Evaluate(sample + 0.5f);
                    sample *= amplitude;
                }
                else
                {
                    sample *= amplitude;
                    colors[n] = coloring.Evaluate(sample + 0.5f);
                }
                vertices[n].y = sample;
            }
        }
        mesh.vertices = vertices;
        mesh.colors = colors;
        mesh.RecalculateNormals();
        CalculateNormals();
    }

    void CreateGrid()
    {
        currentRes = res;
        mesh.Clear();

        vertices = new Vector3[(res + 1) * (res + 1)];
        normals = new Vector3[vertices.Length];
        colors = new Color[vertices.Length];

        Vector2[] uvs = new Vector2[vertices.Length];
        int[] tris = new int[res * res * 6];
        
        int t, n = 0, triCount = 0;
        for (int x = 0; x <= res; x++)
        {
            for (int z = 0; z <= res; z++, n++)
            {
                vertices[n] = new Vector3(((float)x / res) - 0.5f, 0f, ((float)z / res) - 0.5f);
                normals[n] = Vector3.down;
                uvs[n] = new Vector2((float)x / res, (float)z / res);
                if( x != res && z != res)
                {
                    t = x * (res + 1) + z;
                    tris[6 * triCount] = t;
                    tris[6 * triCount + 1] = t + res + 1;
                    tris[6 * triCount + 2] = t + 1;
                    tris[6 * triCount + 3] = t + 1;
                    tris[6 * triCount + 4] = t + res + 1;
                    tris[6 * triCount + 5] = t + res + 2;
                    triCount++;
                }
            }
        }
        mesh.vertices = vertices;
        mesh.normals = normals;
        mesh.uv = uvs;
        mesh.triangles = tris;
    }

    void CalculateNormals ()
    {
        int v = 0;
        for (int z = 0; z <= res; z++)
        {
            for (int x = 0; x <= res; x++, v++)
            {
                normals[v] = new Vector3(GetXDerivative(x, z), 0f, GetZDerivative(x, z)).normalized;
            }
        }
    }

    float GetXDerivative (int x, int z)
    {
        int rowOffset = z * (res + 1);
        float left, right, scale;
        if (x > 0)
        {
            left = vertices[rowOffset + x - 1].y;
            if (x < res)
            {
                right = vertices[rowOffset + x + 1].y;
                scale = res / 2;
            }
            else
            {
                right = vertices[rowOffset + x].y;
                scale = res;
            }
        }
        else
        {
            left = vertices[rowOffset + x].y;
            right = vertices[rowOffset + x + 1].y;
            scale = res;
        }
        return (right - left) * scale;
    }

    float GetZDerivative(int x, int z)
    {
        int rowLength = res + 1;
        float back, forward, scale;
        if (z > 0)
        {
            back = vertices[(z - 1) * rowLength + x].y;
            if (z < res)
            {
                forward = vertices[(z + 1) * rowLength + x].y;
                scale = 0.5f * res;
            }
            else
            {
                forward = vertices[z * rowLength + x].y;
                scale = res;
            }
        }
        else
        {
            back = vertices[z * rowLength + x].y;
            forward = vertices[(z + 1) * rowLength + x].y;
            scale = res;
        }
        return (forward - back) * scale;
    }
}
