using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace CAF.Combat
{
    [CustomEditor(typeof(StateHurtboxDefinition), true)]
    public class StateHurtboxDefinitionEditor : Editor
    {
        StateHurtboxDefinition t;

        protected Dictionary<string, Type> hurtboxGroupTypes = new Dictionary<string, Type>();

        private void OnEnable()
        {
            hurtboxGroupTypes.Clear();
            foreach (var a in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (var givenType in a.GetTypes())
                {
                    if (givenType.IsSubclassOf(typeof(HurtboxGroup)) || givenType == typeof(HurtboxGroup))
                    {
                        hurtboxGroupTypes.Add(givenType.FullName, givenType);
                    }
                }
            }
        }

        public override void OnInspectorGUI()
        {
            t = target as StateHurtboxDefinition;

            if (GUILayout.Button("Add Hurtbox Group"))
            {
                GenericMenu menu = new GenericMenu();

                foreach (string hType in hurtboxGroupTypes.Keys)
                {
                    string destination = hType.Replace('.', '/');
                    menu.AddItem(new GUIContent(destination), true, OnHurtboxGroupTypeSelected, hType);
                }
                menu.ShowAsContext();
            }

            for(int i = 0; i < t.hurtboxGroups.Count; i++)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField($"Hurtbox Group {i}", GUILayout.MaxWidth(100));
                if (GUILayout.Button("X", GUILayout.Width(30)))
                {
                    t.hurtboxGroups.RemoveAt(i);
                    break;
                }
                EditorGUILayout.EndHorizontal();
                EditorGUI.indentLevel++;
                t.hurtboxGroups[i].DrawInspector(15);
                EditorGUI.indentLevel--;
            }

            if (GUI.changed)
            {
                EditorUtility.SetDirty(target);
                GUI.changed = false;
            }
        }

        private void OnHurtboxGroupTypeSelected(object type)
        {
            t.hurtboxGroups.Add((HurtboxGroup)Activator.CreateInstance(hurtboxGroupTypes[(string)type]));
        }
    }
}
