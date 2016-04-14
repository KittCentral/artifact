using UnityEngine;
using UnityEditor;

namespace Runner
{
    [CustomEditor(typeof(Runner.Player))]
    [CanEditMultipleObjects]
    public class PlayerEditor : Editor
    {
        SerializedProperty horiSpeed;
        SerializedProperty vertSpeed;
        SerializedProperty upMove;
        SerializedProperty bulletSpeed;
        SerializedProperty bullet;

        void OnEnable()
        {
            horiSpeed = serializedObject.FindProperty("horiSpeed");
            vertSpeed = serializedObject.FindProperty("vertSpeed");
            upMove = serializedObject.FindProperty("upMove");
            bulletSpeed = serializedObject.FindProperty("bulletSpeed");
            bullet = serializedObject.FindProperty("bullet");
        }

        public override void OnInspectorGUI()
        {

        }
    }
}