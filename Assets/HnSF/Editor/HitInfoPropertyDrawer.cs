using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace HnSF.Combat
{
    [CustomPropertyDrawer(typeof(HitInfo), true)]
    public class HitInfoPropertyDrawer : HitInfoBasePropertyDrawer
    {

        bool damageFoldoutGroup;
        protected override void DrawProperty(ref Rect position, SerializedProperty property, ref float yPosition)
        {
            base.DrawProperty(ref position, property, ref yPosition);

            // DAMAGE //
            damageFoldoutGroup = EditorGUI.BeginFoldoutHeaderGroup(new Rect(position.x, yPosition, position.width, lineHeight),
                damageFoldoutGroup, new GUIContent("Damage"));
            yPosition += lineSpacing;
            if (damageFoldoutGroup)
            {
                EditorGUI.indentLevel++;
                yPosition = DrawDamageGroup(position, property, yPosition);
                EditorGUI.indentLevel--;
            }
            EditorGUI.EndFoldoutHeaderGroup();
        }

        protected virtual float DrawDamageGroup(Rect position, SerializedProperty property, float yPosition)
        {
            EditorGUI.PropertyField(new Rect(position.x, yPosition, position.width, lineHeight), property.FindPropertyRelative("damageOnHit"));

            return yPosition;
        }

        protected override float DrawGeneralGroup(ref Rect position, SerializedProperty property, float yPosition)
        {
            yPosition = base.DrawGeneralGroup(ref position, property, yPosition);

            EditorGUI.PropertyField(new Rect(position.x, yPosition, position.width, lineHeight), property.FindPropertyRelative("causesTumble"));
            yPosition += lineSpacing;
            EditorGUI.PropertyField(new Rect(position.x, yPosition, position.width, lineHeight), property.FindPropertyRelative("knockdown"));
            return yPosition;
        }

        protected override float DrawForcesGroup(ref Rect position, SerializedProperty property, float yPosition)
        {
            yPosition = base.DrawForcesGroup(ref position, property, yPosition);
            switch((HitboxForceType)property.FindPropertyRelative("forceType").enumValueIndex)
            {
                case HitboxForceType.SET:
                    EditorGUI.LabelField(new Rect(position.x, yPosition, 100, lineHeight), new GUIContent("Force"));
                    EditorGUI.PropertyField(new Rect(position.x+140, yPosition, position.width-140, lineHeight), property.FindPropertyRelative("opponentForce"),
                        GUIContent.none);
                    yPosition += lineSpacing;
                    break;
                default:
                    EditorGUI.PropertyField(new Rect(position.x, yPosition, position.width, lineHeight), property.FindPropertyRelative("forceIncludeYForce"));
                    yPosition += lineSpacing;
                    EditorGUI.PropertyField(new Rect(position.x, yPosition, position.width, lineHeight), property.FindPropertyRelative("opponentForceMultiplier"));
                    yPosition += lineSpacing;
                    EditorGUI.PropertyField(new Rect(position.x, yPosition, position.width, lineHeight), property.FindPropertyRelative("opponentMinMagnitude"));
                    yPosition += lineSpacing;
                    EditorGUI.PropertyField(new Rect(position.x, yPosition, position.width, lineHeight), property.FindPropertyRelative("opponentMaxMagnitude"));
                    yPosition += lineSpacing;
                    break;
            }
            EditorGUI.PropertyField(new Rect(position.x, yPosition, position.width, lineHeight), property.FindPropertyRelative("groundBounces"));
            yPosition += lineSpacing;
            if (property.FindPropertyRelative("groundBounces").boolValue)
            {
                EditorGUI.PropertyField(new Rect(position.x, yPosition, position.width, lineHeight), property.FindPropertyRelative("groundBounceForce"));
                yPosition += lineSpacing;
            }

            EditorGUI.PropertyField(new Rect(position.x, yPosition, position.width, lineHeight), property.FindPropertyRelative("wallBounces"));
            yPosition += lineSpacing;
            if (property.FindPropertyRelative("wallBounces").boolValue)
            {
                EditorGUI.PropertyField(new Rect(position.x, yPosition, position.width, lineHeight), property.FindPropertyRelative("wallBounceForce"));
                yPosition += lineSpacing;
            }
            return yPosition;
        }
    }
}