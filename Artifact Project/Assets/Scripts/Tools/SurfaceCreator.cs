using UnityEngine;

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

    public bool analyticalDerivatives;

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
                Gizmos.DrawRay(vertices[v],normals[v] / res);
            }
        }
    }

    public void Refresh()
    {
        if (res != currentRes)
            CreateGrid();        
        Quaternion q = Quaternion.Euler(rotation);
        Quaternion qInv = Quaternion.Inverse(q);
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
                NoiseSample sample = Procedural.Noise.Sum(method, point, frequency, octaves, lacunarity, persistence);
                sample = type == Procedural.NoiseMethodType.Value ? (sample - 0.5f) : (sample * 0.5f);
                if (coloringForStrength)
                {
                    colors[n] = coloring.Evaluate(sample.value + 0.5f);
                    sample *= amplitude;
                }
                else
                {
                    sample *= amplitude;
                    colors[n] = coloring.Evaluate(sample.value + 0.5f);
                }
                vertices[n].y = sample.value;
                sample.derivative = qInv * sample.derivative;
                if (analyticalDerivatives)
                    normals[n] = new Vector3(-sample.derivative.x, 1f, -sample.derivative.y).normalized;
            }
        }
        mesh.vertices = vertices;
        mesh.colors = colors;
        mesh.RecalculateNormals();
        if (!analyticalDerivatives)
            CalculateNormals();
    }

    void CreateGrid()
    {
        currentRes = res;
        mesh.Clear();
        vertices = new Vector3[(res + 1) * (res + 1)];
        colors = new Color[vertices.Length];
        normals = new Vector3[vertices.Length];
        Vector2[] uv = new Vector2[vertices.Length];
        float stepSize = 1f / res;
        for (int v = 0, z = 0; z <= res; z++)
        {
            for (int x = 0; x <= res; x++, v++)
            {
                vertices[v] = new Vector3(x * stepSize - 0.5f, 0f, z * stepSize - 0.5f);
                colors[v] = Color.black;
                normals[v] = Vector3.up;
                uv[v] = new Vector2(x * stepSize, z * stepSize);
            }
        }
        mesh.vertices = vertices;
        mesh.colors = colors;
        mesh.normals = normals;
        mesh.uv = uv;

        int[] triangles = new int[res * res * 6];
        for (int t = 0, v = 0, y = 0; y < res; y++, v++)
        {
            for (int x = 0; x < res; x++, v++, t += 6)
            {
                triangles[t] = v;
                triangles[t + 1] = v + res + 1;
                triangles[t + 2] = v + 1;
                triangles[t + 3] = v + 1;
                triangles[t + 4] = v + res + 1;
                triangles[t + 5] = v + res + 2;
            }
        }
        mesh.triangles = triangles;
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

public struct NoiseSample
{
    public float value;
    public Vector3 derivative;

    public static NoiseSample operator +(NoiseSample a, NoiseSample b)
    {
        a.value += b.value;
        a.derivative += b.derivative;
        return a;
    }

    public static NoiseSample operator +(float a, NoiseSample b)
    {
        b.value += a;
        return b;
    }

    public static NoiseSample operator +(NoiseSample a, float b)
    {
        a.value += b;
        return a;
    }

    public static NoiseSample operator -(NoiseSample a, NoiseSample b)
    {
        a.value -= b.value;
        a.derivative -= b.derivative;
        return a;
    }

    public static NoiseSample operator -(float a, NoiseSample b)
    {
        b.value = a - b.value;
        b.derivative = -b.derivative;
        return b;
    }

    public static NoiseSample operator -(NoiseSample a, float b)
    {
        a.value -= b;
        return a;
    }

    public static NoiseSample operator *(NoiseSample a, NoiseSample b)
    {
        a.derivative = a.derivative * b.value + b.derivative * a.value;
        a.value *= b.value;
        return a;
    }

    public static NoiseSample operator *(float a, NoiseSample b)
    {
        b.value *= a;
        b.derivative *= a;
        return b;
    }

    public static NoiseSample operator *(NoiseSample a, float b)
    {
        a.value *= b;
        a.derivative *= b;
        return a;
    }
}
