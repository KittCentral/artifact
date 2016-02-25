using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TextureCreator))]
public class TextureCreatorInspector : Editor
{
    TextureCreator creator;

    void OnEnable()
    {
        creator = target as TextureCreator;
        Undo.undoRedoPerformed += RefreshCreator;
    }

    void OnDisable()
    {
        Undo.undoRedoPerformed -= RefreshCreator;
    }

    void RefreshCreator()
    {
        if (Application.isPlaying)
            creator.FillTexture();
    }

    public override void OnInspectorGUI ()
    {
        EditorGUI.BeginChangeCheck();
        DrawDefaultInspector();
        if (EditorGUI.EndChangeCheck())
            RefreshCreator();
    }
}
