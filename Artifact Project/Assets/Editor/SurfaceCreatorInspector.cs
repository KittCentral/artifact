using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SurfaceCreator))]
public class SurfaceCreatorInspector : Editor
{
    SurfaceCreator creator;

    void OnEnable()
    {
        creator = target as SurfaceCreator;
        Undo.undoRedoPerformed += RefreshCreator;
    }

    void OnDisable()
    {
        Undo.undoRedoPerformed -= RefreshCreator;
    }

    void RefreshCreator()
    {
        if (Application.isPlaying)
            creator.Refresh();
    }

    public override void OnInspectorGUI()
    {
        EditorGUI.BeginChangeCheck();
        DrawDefaultInspector();
        if (EditorGUI.EndChangeCheck())
            RefreshCreator();
    }
}
