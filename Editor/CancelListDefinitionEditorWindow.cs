using UnityEngine;
using UnityEditor;
using System.Diagnostics.Eventing.Reader;
using System.Collections.Generic;
using System;

namespace HnSF.Combat
{
    public class CancelListDefinitionEditorWindow : EditorWindow
    {
        public AttackDefinition attack;
        public int index;

        protected Dictionary<string, Type> attackConditionTypes = new Dictionary<string, Type>();

        protected virtual void OnFocus()
        {
            attackConditionTypes.Clear();
            foreach (var a in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (var givenType in a.GetTypes())
                {
                    if (givenType.IsSubclassOf(typeof(AttackCondition)))
                    {
                        attackConditionTypes.Add(givenType.FullName, givenType);
                    }
                }
            }
        }

        public static void Init(AttackDefinition attack, int cancelListDefinitionIndex)
        {
            CancelListDefinitionEditorWindow window = (CancelListDefinitionEditorWindow)EditorWindow.GetWindow(typeof(CancelListDefinitionEditorWindow),
                true,
                $"Cancel List {attack.cancels[cancelListDefinitionIndex].startFrame}~{attack.cancels[cancelListDefinitionIndex].endFrame}");
            window.attack = attack;
            window.index = cancelListDefinitionIndex;
            window.Show();
        }

        protected Vector2 scrollPos;
        protected bool conditionsFoldout;
        protected virtual void OnGUI()
        {
            if(attack == null || attack.cancels.Count <= index)
            {
                Close();
                return;
            }
            scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
            DrawCancelListInfo();
            EditorGUILayout.EndScrollView();
        }

        protected virtual void DrawCancelListInfo()
        {
            EditorGUI.indentLevel++;
            SerializedObject attackObject = new SerializedObject(attack);
            attackObject.Update();

            SerializedProperty cancelListProperty = attackObject.FindProperty("cancels").GetArrayElementAtIndex(index);

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Frame Window");
            EditorGUILayout.PropertyField(cancelListProperty.FindPropertyRelative("startFrame"), GUIContent.none);
            EditorGUILayout.PropertyField(cancelListProperty.FindPropertyRelative("endFrame"), GUIContent.none);
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.PropertyField(cancelListProperty.FindPropertyRelative("cancelListID"), new GUIContent("Cancel List Identifier"));

            if (GUILayout.Button("Add Condition"))
            {
                GenericMenu menu = new GenericMenu();

                foreach (string t in attackConditionTypes.Keys)
                {
                    string destination = t.Replace('.', '/');
                    menu.AddItem(new GUIContent(destination), true, OnAttackConditionSelected, t);
                }
                menu.ShowAsContext();
            }

            conditionsFoldout = EditorGUILayout.Foldout(conditionsFoldout, "Conditions");
            if (conditionsFoldout)
            {
                SerializedProperty conditionsProperty = cancelListProperty.FindPropertyRelative("conditions");
                EditorGUI.indentLevel++;
                for (int i = 0; i < conditionsProperty.arraySize; i++)
                {
                    AttackCondition ac = attack.cancels[index].conditions[i];
                    EditorGUILayout.BeginHorizontal();
                    if (GUILayout.Button("-", GUILayout.Width(20)) || ac == null)
                    {
                        conditionsProperty.DeleteArrayElementAtIndex(i);
                        EditorGUILayout.EndHorizontal();
                        break;
                    }
                    EditorGUILayout.PropertyField(conditionsProperty.GetArrayElementAtIndex(i), new GUIContent(ac.GetName()), true);
                    EditorGUILayout.EndHorizontal();
                }
                EditorGUI.indentLevel--;
            }

            attackObject.ApplyModifiedProperties();
            EditorGUI.indentLevel--;
        }

        protected virtual void OnAttackConditionSelected(object t)
        {
            SerializedObject attackObject = new SerializedObject(attack);
            attackObject.Update();
            SerializedProperty eventProperty = attackObject.FindProperty("cancels").GetArrayElementAtIndex(index);
            eventProperty.FindPropertyRelative("conditions").InsertArrayElementAtIndex(eventProperty.FindPropertyRelative("conditions").arraySize);
            var conditionProperty = eventProperty.FindPropertyRelative("conditions").GetArrayElementAtIndex(eventProperty.FindPropertyRelative("conditions").arraySize - 1);
            conditionProperty.managedReferenceValue = Activator.CreateInstance(attackConditionTypes[(string)t]);
            attackObject.ApplyModifiedProperties();
        }
    }
}