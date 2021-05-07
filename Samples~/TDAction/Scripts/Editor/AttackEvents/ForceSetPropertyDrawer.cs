using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace TDAction.Combat.Events { 
    [CustomPropertyDrawer(typeof(ForceSet))]
    public class ForceSetPropertyDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            SerializedProperty applyXForceProperty = property.FindPropertyRelative("applyXForce");
            SerializedProperty applyYForceProperty = property.FindPropertyRelative("applyYForce");

            return EditorGUIUtility.singleLineHeight * 2
                + (applyXForceProperty.boolValue ? EditorGUIUtility.singleLineHeight : 0)
                + (applyYForceProperty.boolValue ? EditorGUIUtility.singleLineHeight : 0)
                + 18;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            float lineHeight = EditorGUIUtility.singleLineHeight;
            float lineSpacing = 20;

            float yPosition = position.y;
            var applyXForceRect = new Rect(position.x, yPosition, position.width, lineHeight);
            yPosition += lineSpacing;
            var applyYForceRect = new Rect(position.x, yPosition, position.width, lineHeight);
            yPosition += lineSpacing;
            var oneForceRect = new Rect(position.x, yPosition, position.width, lineHeight);
            yPosition += lineSpacing;
            var twoForceRect = new Rect(position.x, yPosition, position.width, lineHeight);

            SerializedProperty applyXForceProperty = property.FindPropertyRelative("applyXForce");
            SerializedProperty applyYForceProperty = property.FindPropertyRelative("applyYForce");

            EditorGUI.PropertyField(applyXForceRect, applyXForceProperty, new GUIContent("Apply X Force"));
            EditorGUI.PropertyField(applyYForceRect, applyYForceProperty, new GUIContent("Apply Y Force"));

            if (applyXForceProperty.boolValue == true)
            {
                EditorGUI.PropertyField(oneForceRect, property.FindPropertyRelative("xForce"));
            }
            if (applyYForceProperty.boolValue == true)
            {
                EditorGUI.PropertyField(applyXForceProperty.boolValue == true ? twoForceRect : oneForceRect, property.FindPropertyRelative("yForce"));
            }

            EditorGUI.EndProperty();
        }
    } 
}

