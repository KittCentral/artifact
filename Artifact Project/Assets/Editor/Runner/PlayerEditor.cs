using UnityEngine;
using UnityEditor;

namespace Runner
{
    [CustomEditor(typeof(Runner.Player))]
    public class PlayerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            Player player = (Player)target;
            player.horiSpeed = EditorGUILayout.Slider("Horizontal Movement", player.horiSpeed, 5, 20);
            if (player.upMove)
                player.vertSpeed = EditorGUILayout.Slider("Vertical Movement", player.vertSpeed, 1, 20);
            player.upMove = EditorGUILayout.Toggle("Allow Vertical Movement", player.upMove);
            player.jump = EditorGUILayout.Toggle("Allow Jumping", player.jump);
            player.bullet = EditorGUILayout.ObjectField(player.bullet, typeof(GameObject), false) as GameObject;
            player.bulletSpeed = EditorGUILayout.FloatField(player.bulletSpeed);
        }
    }
}