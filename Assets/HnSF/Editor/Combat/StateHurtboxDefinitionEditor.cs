using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace HnSF.Combat
{
    [CustomEditor(typeof(StateHurtboxDefinition), true)]
    public class StateHurtboxDefinitionEditor : Editor
    {
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
            this.serializedObject.Update();

            if(GUILayout.Button("Add Hurtbox Group"))
            {
                GenericMenu menu = new GenericMenu();

                foreach (string hType in hurtboxGroupTypes.Keys)
                {
                    string destination = hType.Replace('.', '/');
                    menu.AddItem(new GUIContent(destination), true, OnHurtboxGroupTypeSelected, hType);
                }
                menu.ShowAsContext();
            }

            SerializedProperty hurtboxGroupProperty = serializedObject.FindProperty("hurtboxGroups");

            int lCount = hurtboxGroupProperty.arraySize;
            for(int i = 0; i < lCount; i++)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField($"Hurtbox Group {i}", GUILayout.MaxWidth(100));
                if(GUILayout.Button("X", GUILayout.Width(30)))
                {
                    hurtboxGroupProperty.DeleteArrayElementAtIndex(i);
                    break;
                }
                EditorGUILayout.EndHorizontal();
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(hurtboxGroupProperty.GetArrayElementAtIndex(i));
                EditorGUI.indentLevel--;
            }
            serializedObject.ApplyModifiedProperties();

            //this.DrawDefaultInspector();
            /*
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
            }*/
        }

        private void OnHurtboxGroupTypeSelected(object type)
        {
            serializedObject.Update();
            SerializedProperty property = serializedObject.FindProperty("hurtboxGroups");
            property.InsertArrayElementAtIndex(property.arraySize);
            property.GetArrayElementAtIndex(property.arraySize - 1).managedReferenceValue = Activator.CreateInstance(hurtboxGroupTypes[(string)type]);
            serializedObject.ApplyModifiedProperties();
        }
    }
}
