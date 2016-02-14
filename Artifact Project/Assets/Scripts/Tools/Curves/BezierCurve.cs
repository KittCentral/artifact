using UnityEngine;

public class BezierCurve : MonoBehaviour
{
    public Vector3[] points;

    public void Reset()
    {
        points = new Vector3[] { new Vector3(1f, 0f, 0f), new Vector3(2f, 0f, 0f), new Vector3(3f, 0f, 0f), new Vector3(4f, 0f, 0f) };
    }

    public Vector3 GetPoint (float t)
    {
        return transform.TransformPoint(Bezier.GetPoint(points[0], points[1], points[2], points[3], t));
    }

    public Vector3 GetVelocity (float t, bool normalized = false)
    {
        Vector3 velocity = transform.TransformPoint(Bezier.GetFirstDerivative(points[0], points[1], points[2], points[3], t) - transform.position);
        if (normalized)
            velocity = velocity.normalized;
        return velocity;
    }
}
