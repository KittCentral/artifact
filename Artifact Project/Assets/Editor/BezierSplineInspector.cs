using UnityEngine;
using UnityEditor;

[CustomEditor (typeof(BezierSpline))]
public class BezierSplineInspector : Editor
{
    BezierSpline spline;
    Transform handleTransform;
    Quaternion handleRotation;

    const int lineSteps = 10;
    const float directionScale = 1f;
    const int stepsPerCurve = 10;
    const float handleSize = 0.04f;
    const float pickSize = 0.06f;

    int selectedIndex = -1;

    void OnSceneGUI()
    {
        spline = target as BezierSpline;
        handleTransform = spline.transform;
        handleRotation = Tools.pivotRotation == PivotRotation.Local ? handleTransform.rotation : Quaternion.identity;

        Vector3 p0 = ShowPoint(0);
        for (int i = 1; i < spline.points.Length; i += 3)
        {
            Vector3 p1 = ShowPoint(i);
            Vector3 p2 = ShowPoint(i + 1);
            Vector3 p3 = ShowPoint(i + 2);

            Handles.color = Color.gray;
            Handles.DrawLine(p0, p1);
            Handles.DrawLine(p2, p3);

            Handles.DrawBezier(p0, p3, p1, p2, Color.white, null, 2f);
            p0 = p3;
        }
        ShowDirections();
    }

    public override void OnInspectorGUI ()
    {
        DrawDefaultInspector();
        spline = target as BezierSpline;
        if(GUILayout.Button("Add Curve"))
        {
            Undo.RecordObject(spline, "Add Curve");
            EditorUtility.SetDirty(spline);
            spline.AddCurve();
        }
    }

    void ShowDirections()
    {
        Handles.color = Color.green;
        Vector3 point = spline.GetPoint(0f);
        Handles.DrawLine(point, point + spline.GetVelocity(0f, true) * directionScale);
        int steps = stepsPerCurve * spline.CurveCount;
        for (int i = 1; i <= steps; i++)
        {
            point = spline.GetPoint(i / (float)steps);
            Handles.DrawLine(point, point + spline.GetVelocity(i / (float)steps, true) * directionScale);
        }
    }

    Vector3 ShowPoint(int i)
    {
        Vector3 point = handleTransform.TransformPoint(spline.points[i]);
        Handles.color = Color.white;
        if (Handles.Button(point, handleRotation, handleSize, pickSize, Handles.DotCap))
        {
            selectedIndex = i;
        }
        if (selectedIndex == i)
        {
            EditorGUI.BeginChangeCheck();
            point = Handles.DoPositionHandle(point, handleRotation);
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(spline, "Move Point");
                EditorUtility.SetDirty(spline);
                spline.points[i] = handleTransform.InverseTransformPoint(point);
            }
        }
        return point;
    }
}
