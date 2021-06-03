using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace HnSF.Combat
{
    [CustomPropertyDrawer(typeof(HitInfoBase), true)]
    public class HitInfoBasePropertyDrawer : PropertyDrawer
    {
        protected float lineSpacing = 20;
        protected float lineHeight;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return calcHeight;
        }

        protected float calcHeight = 100;
        protected bool generalFoldoutGroup = false;
        protected bool forcesFoldoutGroup = false;
        protected bool stunFoldoutGroup = false;
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            float startHeight = position.y;
            lineHeight = EditorGUIUtility.singleLineHeight;
            float yPosition = position.y;
            DrawProperty(ref position, property, ref yPosition);
            calcHeight = yPosition - startHeight;
            EditorGUI.EndProperty();
        }

        protected virtual void DrawProperty(ref Rect position, SerializedProperty property, ref float yPosition)
        {
            // GENERAL //
            generalFoldoutGroup = EditorGUI.BeginFoldoutHeaderGroup(new Rect(position.x, yPosition, position.width, lineHeight),
                generalFoldoutGroup, new GUIContent("General"));
            yPosition += lineSpacing;
            if (generalFoldoutGroup)
            {
                EditorGUI.indentLevel++;
                yPosition = DrawGeneralGroup(ref position, property, yPosition);
                EditorGUI.indentLevel--;
            }
            EditorGUI.EndFoldoutHeaderGroup();

            // FORCES //
            forcesFoldoutGroup = EditorGUI.BeginFoldoutHeaderGroup(new Rect(position.x, yPosition, position.width, lineHeight),
                forcesFoldoutGroup, new GUIContent("Forces"));
            yPosition += lineSpacing;
            if (forcesFoldoutGroup)
            {
                EditorGUI.indentLevel++;
                yPosition = DrawForcesGroup(ref position, property, yPosition);
                EditorGUI.indentLevel--;
            }
            EditorGUI.EndFoldoutHeaderGroup();

            // STUN //
            stunFoldoutGroup = EditorGUI.BeginFoldoutHeaderGroup(new Rect(position.x, yPosition, position.width, lineHeight),
                stunFoldoutGroup, new GUIContent("Stun"));
            yPosition += lineSpacing;
            if (stunFoldoutGroup)
            {
                EditorGUI.indentLevel++;
                yPosition = DrawStunGroup(position, property, yPosition);
                EditorGUI.indentLevel--;
            }
            EditorGUI.EndFoldoutHeaderGroup();
        }

        protected virtual float DrawGeneralGroup(ref Rect position, SerializedProperty property, float yPosition)
        {
            EditorGUI.PropertyField(new Rect(position.x, yPosition, position.width, lineHeight), property.FindPropertyRelative("hitType"));
            yPosition += lineSpacing;
            EditorGUI.BeginDisabledGroup(property.FindPropertyRelative("groundOnly").boolValue);
            EditorGUI.PropertyField(new Rect(position.x, yPosition, position.width, lineHeight), property.FindPropertyRelative("airOnly"));
            EditorGUI.EndDisabledGroup();
            yPosition += lineSpacing;
            EditorGUI.BeginDisabledGroup(property.FindPropertyRelative("airOnly").boolValue);
            EditorGUI.PropertyField(new Rect(position.x, yPosition, position.width, lineHeight), property.FindPropertyRelative("groundOnly"));
            EditorGUI.EndDisabledGroup();
            yPosition += lineSpacing;
            EditorGUI.PropertyField(new Rect(position.x, yPosition, position.width, lineHeight), property.FindPropertyRelative("hitKills"));
            yPosition += lineSpacing;
            EditorGUI.PropertyField(new Rect(position.x, yPosition, position.width, lineHeight), property.FindPropertyRelative("continuousHit"));
            yPosition += lineSpacing;
            if (property.FindPropertyRelative("continuousHit").boolValue == true)
            {
                EditorGUI.PropertyField(new Rect(position.x, yPosition, position.width, lineHeight), property.FindPropertyRelative("spaceBetweenHits"));
                yPosition += lineSpacing;
            }
            return yPosition;
        }

        protected virtual float DrawForcesGroup(ref Rect position, SerializedProperty property, float yPosition)
        {
            EditorGUI.PropertyField(new Rect(position.x, yPosition, position.width, lineHeight), property.FindPropertyRelative("opponentResetXForce"));
            yPosition += lineSpacing;
            EditorGUI.PropertyField(new Rect(position.x, yPosition, position.width, lineHeight), property.FindPropertyRelative("opponentResetYForce"));
            yPosition += lineSpacing;
            EditorGUI.PropertyField(new Rect(position.x, yPosition, position.width, lineHeight), property.FindPropertyRelative("autoLink"));
            yPosition += lineSpacing;
            EditorGUI.PropertyField(new Rect(position.x, yPosition, position.width, lineHeight), property.FindPropertyRelative("forceType"));
            yPosition += lineSpacing;
            EditorGUI.PropertyField(new Rect(position.x, yPosition, position.width, lineHeight), property.FindPropertyRelative("forceRelation"));
            yPosition += lineSpacing;
            return yPosition;
        }

        protected virtual float DrawStunGroup(Rect position, SerializedProperty property, float yPosition)
        {
            EditorGUI.PropertyField(new Rect(position.x, yPosition, position.width, lineHeight), property.FindPropertyRelative("attackerHitstop"), new GUIContent("Hitstop (Attacker)"));
            yPosition += lineSpacing;
            EditorGUI.PropertyField(new Rect(position.x, yPosition, position.width, lineHeight), property.FindPropertyRelative("hitstop"), new GUIContent("Hitstop (Enemy)"));
            yPosition += lineSpacing;
            EditorGUI.PropertyField(new Rect(position.x, yPosition, position.width, lineHeight), property.FindPropertyRelative("hitstun"), new GUIContent("Hitstun (Enemy)"));
            yPosition += lineSpacing;
            return yPosition;
        }
    }
}