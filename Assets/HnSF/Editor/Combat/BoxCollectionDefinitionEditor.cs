using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace HnSF.Combat
{
    [CustomEditor(typeof(BoxCollectionDefinition), true)]
    public class BoxCollectionDefinitionEditor : Editor
    {
        protected Dictionary<string, Type> hurtboxGroupTypes = new Dictionary<string, Type>();

        public virtual void OnEnable()
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
            serializedObject.Update();
            CreateMenus();
            serializedObject.ApplyModifiedProperties();
        }

        protected bool hurtboxGroupsDropdown;
        public virtual void CreateMenus()
        {
            GenericMenu menu = new GenericMenu();
            foreach (string hType in hurtboxGroupTypes.Keys)
            {
                string destination = hType.Replace('.', '/');
                menu.AddItem(new GUIContent(destination), true, OnHurtboxGroupTypeSelected, hType);
            }
            DrawBoxSection(menu, serializedObject, "Add Hurtbox Group", "hurtboxGroups", ref hurtboxGroupsDropdown);
        }

        protected virtual void DrawBoxSection(GenericMenu addMenu, SerializedObject serializedObject, string addButtonName, string listPropertyName, ref bool foldoutValue)
        {
            if (GUILayout.Button(addButtonName))
            {
                addMenu.ShowAsContext();
            }

            foldoutValue = EditorGUILayout.Foldout(foldoutValue, "Boxes");

            if (foldoutValue)
            {
                EditorGUI.indentLevel++;
                SerializedProperty listProperty = serializedObject.FindProperty(listPropertyName);

                int lCount = listProperty.arraySize;
                for (int i = 0; i < lCount; i++)
                {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField($"Group {i}", GUILayout.MaxWidth(100));
                    if (GUILayout.Button("X", GUILayout.Width(30)))
                    {
                        listProperty.DeleteArrayElementAtIndex(i);
                        break;
                    }
                    if (GUILayout.Button("∨", GUILayout.Width(30)))
                    {
                        listProperty.MoveArrayElement(i, i + 1);
                    }
                    if (GUILayout.Button("∧", GUILayout.Width(30)))
                    {
                        listProperty.MoveArrayElement(i, i - 1);
                    }
                    EditorGUILayout.EndHorizontal();
                    EditorGUI.indentLevel++;
                    EditorGUILayout.PropertyField(listProperty.GetArrayElementAtIndex(i));
                    EditorGUI.indentLevel--;
                }
                EditorGUI.indentLevel--;
            }
        }

        protected virtual void OnHurtboxGroupTypeSelected(object type)
        {
            serializedObject.Update();
            SerializedProperty property = serializedObject.FindProperty("hurtboxGroups");
            property.InsertArrayElementAtIndex(property.arraySize);
            property.GetArrayElementAtIndex(property.arraySize - 1).managedReferenceValue = Activator.CreateInstance(hurtboxGroupTypes[(string)type]);
            serializedObject.ApplyModifiedProperties();
        }
    }
}
