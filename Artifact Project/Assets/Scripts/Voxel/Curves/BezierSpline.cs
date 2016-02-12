using System;
using UnityEngine;

public enum BezierControlPointMode { Free, Aligned, Mirrored }

public class BezierSpline : MonoBehaviour
{
    [SerializeField]
    Vector3[] points;

    [SerializeField]
    BezierControlPointMode[] modes;

    [SerializeField]
    bool loop;

    public int ControlPointCount
    {
        get { return points.Length; }
    }

    public int CurveCount
    {
        get { return (points.Length - 1) / 3; }
    }

    public BezierControlPointMode[] Modes
    {
        get{ return modes; }
        set{ modes = value; }
    }

    public bool Loop
    {
        get { return loop; }
        set
        {
            loop = value;
            if (value)
            {
                modes[modes.Length - 1] = modes[0];
                SetControlPoint(0, points[0]);
            }
        }
    }

    public Vector3 GetControlPoint (int i) { return points[i];}

    public void SetControlPoint (int i, Vector3 point)
    {
        if (i % 3 == 0)
        {
            Vector3 delta = point - points[i];
            if (loop)
            {
                if (i == 0)
                {
                    points[1] += delta;
                    points[points.Length - 2] += delta;
                    points[points.Length - 1] = delta;
                }
                else if (i == points.Length - 1)
                {
                    points[0] = point;
                    points[1] += delta;
                    points[i - 1] += delta;
                }
                else
                {
                    points[i - 1] += delta;
                    points[i + 1] += delta;
                }
            }
            else
            {
                if (i > 0)
                    points[i - 1] += delta;
                if (i + 1 < points.Length)
                    points[i + 1] += delta;
            }
        }
        points[i] = point;
        EnforceMode(i);
    }

    public BezierControlPointMode GetControlPointMode(int i) { return modes[(i + 1) / 3]; }

    public void SetControlPointMode(int i, BezierControlPointMode mode)
    {
        int modeIndex = (i + 1) / 3;
        modes[modeIndex] = mode;
        if (loop)
        {
            if (modeIndex == 0)
                modes[modes.Length - 1] = mode;
            else if (modeIndex == modes.Length - 1)
                modes[0] = mode;
        }
        EnforceMode(i);
    }

    public Vector3 GetPoint(float t)
    {
        int i;
        if(t >= 1f)
        {
            t = 1f;
            i = points.Length - 4;
        }
        else
        {
            t = Mathf.Clamp01(t) * CurveCount;
            i = (int)t;
            t -= i;
            i *= 3;
        }
        return transform.TransformPoint(Bezier.GetPoint(points[i], points[i + 1], points[i + 2], points[i + 3], t));
    }

    public Vector3 GetVelocity(float t, bool normalized = false)
    {
        int i;
        if (t >= 1f)
        {
            t = 1f;
            i = points.Length - 4;
        }
        else
        {
            t = Mathf.Clamp01(t) * CurveCount;
            i = (int)t;
            t -= i;
            i *= 3;
        }
        Vector3 velocity = transform.TransformPoint(Bezier.GetFirstDerivative(points[i], points[i + 1], points[i + 2], points[i + 3], t) - transform.position);
        if (normalized)
            velocity = velocity.normalized;
        return velocity;
    }

    public void AddCurve ()
    {
        Vector3 point = points[points.Length - 1];
        Array.Resize(ref points, points.Length + 3);
        point.x += 1f;
        points[points.Length - 3] = point;
        point.x += 1f;
        points[points.Length - 2] = point;
        point.x += 1f;
        points[points.Length - 1] = point;
        Array.Resize(ref modes, modes.Length + 1);
        modes[modes.Length - 1] = modes[modes.Length - 2];
        EnforceMode(points.Length - 4);
        if (loop)
        {
            points[points.Length - 1] = points[0];
            modes[modes.Length - 1] = modes[0];
            EnforceMode(0);
        }
    }

    void EnforceMode (int i)
    {
        int modeIndex = (i + 1) / 3;
        BezierControlPointMode mode = modes[modeIndex];
        if (mode == BezierControlPointMode.Free || !loop && (modeIndex == 0 || modeIndex == modes.Length - 1))
            return;
        int middleIndex = modeIndex * 3;
        int fixedIndex, enforcedIndex;
        if (i <= middleIndex)
        {
            fixedIndex = middleIndex - 1;
            if (fixedIndex < 0)
                fixedIndex = points.Length - 2;
            enforcedIndex = middleIndex + 1;
            if (enforcedIndex >= points.Length)
                enforcedIndex = 1;
        }
        else
        {
            fixedIndex = middleIndex + 1;
            if (fixedIndex >= points.Length)
                fixedIndex = 1;
            enforcedIndex = middleIndex - 1;
            if (enforcedIndex < 0)
                enforcedIndex = points.Length - 2;
        }
        Vector3 middle = points[middleIndex];
        Vector3 enforcedTangent = middle - points[fixedIndex];
        if (mode == BezierControlPointMode.Aligned)
        {
            enforcedTangent = enforcedTangent.normalized * Vector3.Distance(middle, points[enforcedIndex]);
        }
        points[enforcedIndex] = middle + enforcedTangent;
    }

    public void Reset()
    {
        points = new Vector3[] { new Vector3(1f, 0f, 0f), new Vector3(2f, 0f, 0f), new Vector3(3f, 0f, 0f), new Vector3(4f, 0f, 0f) };
        modes = new BezierControlPointMode[] { BezierControlPointMode.Mirrored, BezierControlPointMode.Mirrored };
    }
}
