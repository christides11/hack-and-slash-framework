using UnityEngine;
using UnityEditor;

namespace TDAction.Combat
{
    [CustomEditor(typeof(AttackDefinition), true)]
    public class AttackDefinitionEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            if (GUILayout.Button("Open Editor", GUILayout.Width(Screen.width), GUILayout.Height(45)))
            {
                TDAction.Combat.AttackDefinitionEditorWindow.Init(target as AttackDefinition);
            }

            DrawDefaultInspector();
        }
    }
}