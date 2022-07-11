using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace HnSF.Combat
{
    [CustomPropertyDrawer(typeof(HitInfoBase), true)]
    public class HitInfoBasePropertyDrawer : PropertyDrawer
    {
        protected float lineHeight;
        protected int lines;
        protected Rect pos;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUIUtility.singleLineHeight * lines + EditorGUIUtility.standardVerticalSpacing * (lines - 1);
        }
        
        protected bool generalFoldoutGroup = false;
        protected bool forcesFoldoutGroup = false;
        protected bool stunFoldoutGroup = false;
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            lines = 0;
            pos = position;
            EditorGUI.BeginProperty(position, label, property);
            lineHeight = EditorGUIUtility.singleLineHeight;
            DrawProperty(ref position, property);
            EditorGUI.EndProperty();
        }

        protected virtual float GetLineY()
        {
            return pos.min.y + lines++ * lineHeight;
        }

        public virtual void DrawProperty(ref Rect position, SerializedProperty property)
        {
            // GENERAL //
            generalFoldoutGroup = EditorGUI.BeginFoldoutHeaderGroup(new Rect(position.x, GetLineY(), position.width, lineHeight),
                generalFoldoutGroup, new GUIContent("General"));
            if (generalFoldoutGroup)
            {
                EditorGUI.indentLevel++;
                DrawGeneralGroup(ref position, property);
                EditorGUI.indentLevel--;
            }
            EditorGUI.EndFoldoutHeaderGroup();
        }

        protected virtual void DrawGeneralGroup(ref Rect position, SerializedProperty property)
        {
            EditorGUI.PropertyField(new Rect(position.x, GetLineY(), position.width, lineHeight), property.FindPropertyRelative("ID"));
        }
    }
}