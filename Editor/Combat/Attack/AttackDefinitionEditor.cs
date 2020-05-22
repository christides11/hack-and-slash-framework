using UnityEditor;
using UnityEngine;

namespace CAF.Combat
{
    [CustomEditor(typeof(AttackDefinition), true)]
    public class AttackDefinitionEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            if (GUILayout.Button("Open Editor", GUILayout.Width(Screen.width), GUILayout.Height(45)))
            {
                AttackDefinitionEditorWindow.Init(target as AttackDefinition);
            }

            DrawDefaultInspector();
        }
    }
}