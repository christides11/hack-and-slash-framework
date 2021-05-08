using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

namespace HnSF.Combat
{
    public class CancelListDefinitionEditorWindow : EditorWindow
    {
        public AttackDefinition attack;
        public int index;

        public static void Init(AttackDefinition attack, int cancelListDefinitionIndex)
        {
            CancelListDefinitionEditorWindow window = (CancelListDefinitionEditorWindow)EditorWindow.GetWindow(typeof(CancelListDefinitionEditorWindow),
                true,
                $"Cancel List {attack.cancels[cancelListDefinitionIndex].startFrame}~{attack.cancels[cancelListDefinitionIndex].endFrame}");
            window.attack = attack;
            window.index = cancelListDefinitionIndex;
            window.Show();
        }

        Vector2 scrollPos;
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

            attackObject.ApplyModifiedProperties();
            EditorGUI.indentLevel--;
        }
    }
}