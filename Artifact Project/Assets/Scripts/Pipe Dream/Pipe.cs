using UnityEngine;

namespace PipeDream
{
    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
    public class Pipe : MonoBehaviour
    {
        public float pipeRadius;
        public int pipeSegmentCount;
        public float minCurveRadius, maxCurveRadius;
        public int minCurveSegmentCount, maxCurveSegmentCount;
        public float ringDistance;
        public PipeItemGenerator[] generators;

        float curveRadius;
        int curveSegmentCount;
        
        float curveAngle;

        float relativeRotation;

        Mesh mesh;
        Vector3[] verts;
        int[] tris;

        public float CurveRadius
        {
            get{ return curveRadius; }
            set{ curveRadius = value; }
        }

        public float CurveAngle
        {
            get{ return curveAngle; }
            set{ curveAngle = value; }
        }

        public float RelativeRotation
        {
            get{ return relativeRotation; }
            set{ relativeRotation = value; }
        }

        public int CurveSegmentCount
        {
            get{ return curveSegmentCount; }
            set{ curveSegmentCount = value; }
        }

        void Awake()
        {
            GetComponent<MeshFilter>().mesh = mesh = new Mesh();
            mesh.name = "Pipe";
        }

        public void Generate (bool withItems = true)
        {
            CurveRadius = Random.Range(minCurveRadius, maxCurveRadius);
            CurveSegmentCount = Random.Range(minCurveSegmentCount, maxCurveSegmentCount);
            if (transform.GetComponentInParent<PipeSystem>().player.distanceTravelled < 120)
            {
                for (int i = 0; i < transform.childCount; i++)
                {
                    Destroy(transform.GetChild(i).gameObject);
                }
            }
            if(withItems)
                generators[Random.Range(0, generators.Length)].GenerateItems(this);
            mesh.Clear();
            SetVertices();
            SetTriangles();
            mesh.RecalculateNormals();
        }
        /*
        void OnDrawGizmos()
        {
            float uStep = (2f * Mathf.PI) / curveSegmentCount;
            float vStep = (2f * Mathf.PI) / pipeSegmentCount;
            for (int u = 0; u < curveSegmentCount; u++)
            {
                for (int v = 0; v < pipeSegmentCount; v++)
                {
                    Vector3 point = GetPointOnTorus(u * uStep, v * vStep);
                    Gizmos.color = new Color(1f, (float)v / pipeSegmentCount, (float)u / curveSegmentCount);
                    Gizmos.DrawSphere(point, 0.1f);
                }
            }
        }
        */
        void SetVertices ()
        {
            verts = new Vector3[pipeSegmentCount * CurveSegmentCount * 4];
            float uStep = ringDistance / CurveRadius;
            CurveAngle = uStep * CurveSegmentCount * (360f / (2f * Mathf.PI));
            CreateFirstQuadRing(uStep);
            int iDelta = pipeSegmentCount * 4;
            for (int u = 2, i = iDelta; u <= CurveSegmentCount; u++, i += iDelta)
                CreateQuadRing(u * uStep, i);
            mesh.vertices = verts;
        }

        void SetTriangles ()
        {
            tris = new int[pipeSegmentCount * CurveSegmentCount * 6];
            for (int t = 0, i = 0; t < tris.Length; t += 6, i += 4)
            {
                tris[t] = i;
                tris[t + 2] = tris[t + 3] = i + 1;
                tris[t + 1] = tris[t + 4] = i + 2;
                tris[t + 5] = i + 3;
            }
            mesh.triangles = tris;
        }

        void CreateFirstQuadRing (float u)
        {
            float vStep = (2f * Mathf.PI) / pipeSegmentCount;
            Vector3 vertexA = GetPointOnTorus(0f, 0f);
            Vector3 vertexB = GetPointOnTorus(u, 0f);
            for (int v = 1, i = 0; v <= pipeSegmentCount; v++, i += 4)
            {
                verts[i] = vertexA;
                verts[i + 1] = vertexA = GetPointOnTorus(0f, v * vStep);
                verts[i + 2] = vertexB;
                verts[i + 3] = vertexB = GetPointOnTorus(u, v * vStep);
            }
        }

        void CreateQuadRing(float u, int i)
        {
            float vStep = (2f * Mathf.PI) / pipeSegmentCount;
            int ringOffset = pipeSegmentCount * 4;
            Vector3 vertex = GetPointOnTorus(u, 0f);
            for (int v = 1; v <= pipeSegmentCount; v++, i += 4)
            {
                verts[i] = verts[i - ringOffset + 2];
                verts[i + 1] = verts[i - ringOffset + 3];
                verts[i + 2] = vertex;
                verts[i + 3] = vertex = GetPointOnTorus(u, v * vStep);
            }
        }

        public void AlignWith (Pipe pipe)
        {
            RelativeRotation = Random.Range(0, CurveSegmentCount) * 360f / pipeSegmentCount;

            transform.SetParent(pipe.transform, false);
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.Euler(0f, 0f, -pipe.CurveAngle);
            transform.localScale = Vector3.one;
            transform.Translate(0f, pipe.CurveRadius, 0f);
            transform.Rotate(relativeRotation, 0f, 0f);
            transform.Translate(0f, -CurveRadius, 0f);
            transform.SetParent(pipe.transform.parent);
        }

        Vector3 GetPointOnTorus (float u, float v)
        {
            float r = CurveRadius + pipeRadius * Mathf.Cos(v);
            Vector3 point = new Vector3(r * Mathf.Sin(u), r * Mathf.Cos(u), pipeRadius * Mathf.Sin(v));
            return point;
        }
    }
}
