using System;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace HnSF.Input
{
    [System.Serializable]
    public class InputSequence
    {
        public uint executeWindow = 3;
        [SerializeField]public List<InputDefinition> executeInputs = new List<InputDefinition>();
        public uint sequenceWindow = 8;
        [SerializeField]public List<InputDefinition> sequenceInputs = new List<InputDefinition>();

#if UNITY_EDITOR
        [NonSerialized] private bool executeWindowFoldout = false;
        [NonSerialized] private bool sequenceWindowFoldout = false;
#endif

        public virtual void DrawInspector()
        {
#if UNITY_EDITOR
            executeWindow = (uint)EditorGUILayout.IntField("Execute Window", (int)executeWindow);
            DrawInputDefinitionList(ref executeWindowFoldout, "Execute Buttons", ref executeInputs);
            EditorGUILayout.Space();
            sequenceWindow = (uint)EditorGUILayout.IntField("Sequence Window", (int)sequenceWindow);
            DrawInputDefinitionList(ref sequenceWindowFoldout, "Sequence Buttons", ref sequenceInputs);
#endif
        }

#if UNITY_EDITOR
        public virtual void DrawInputDefinitionList(ref bool foldout, string foldoutName, ref List<InputDefinition> inputs)
        {
            foldout = EditorGUILayout.Foldout(foldout, foldoutName);
            if (foldout)
            {
                EditorGUI.indentLevel++;

                EditorGUILayout.BeginHorizontal();
                GUILayout.Space(EditorGUI.indentLevel * 15);
                if(GUILayout.Button("Add Input"))
                {
                    inputs.Add(new InputDefinition());
                }
                EditorGUILayout.EndHorizontal();

                for(int i = 0; i < inputs.Count; i++)
                {
                    EditorGUILayout.BeginHorizontal();
                    GUILayout.Space(EditorGUI.indentLevel * 15);
                    if (GUILayout.Button("X", GUILayout.Width(20)))
                    {
                        inputs.RemoveAt(i);
                        continue;
                    }
                    EditorGUILayout.EndHorizontal();
                    inputs[i].inputType = (InputDefinitionType)EditorGUILayout.EnumPopup("Input Type", inputs[i].inputType);
                    switch (inputs[i].inputType)
                    {
                        case InputDefinitionType.Button:
                            inputs[i].buttonID = EditorGUILayout.IntField("Button ID", inputs[i].buttonID);
                            break;
                        case InputDefinitionType.Stick:
                            break;
                    }
                }

                EditorGUI.indentLevel--;
            }
        }
#endif
    }
}