using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace HnSF.Combat
{
    [CustomPropertyDrawer(typeof(BoxDefinition), true)]
    public class BoxDefinitionPropertyDrawer : PropertyDrawer
    {
        float lineSpacing = 20;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            float baseHeight = EditorGUIUtility.singleLineHeight * 4;
            int shapeEnumValue = property.FindPropertyRelative("shape").enumValueIndex;
            if(shapeEnumValue == (int)BoxShape.Capsule)
            {
                baseHeight += EditorGUIUtility.singleLineHeight;
            }

            return baseHeight;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            float lineHeight = EditorGUIUtility.singleLineHeight;

            float yPosition = position.y;
            var shapeRect = new Rect(position.x, yPosition, position.width, lineHeight);
            yPosition += lineSpacing;
            var offsetRect = new Rect(position.x, yPosition, position.width, lineHeight);
            yPosition += lineSpacing;
            var rotationRect = new Rect(position.x, yPosition, position.width, lineHeight);
            yPosition += lineSpacing;
            var sizeRect = new Rect(position.x, yPosition, position.width, lineHeight);
            var radiusRect = new Rect(position.x, yPosition, position.width, lineHeight);
            yPosition += lineSpacing;
            var heightRect = new Rect(position.x, yPosition, position.width, lineHeight);

            EditorGUI.PropertyField(shapeRect, property.FindPropertyRelative("shape"), new GUIContent("Shape"));
            EditorGUI.PropertyField(offsetRect, property.FindPropertyRelative("offset"), new GUIContent("Offset"));
            EditorGUI.PropertyField(rotationRect, property.FindPropertyRelative("rotation"), new GUIContent("Rotation"));
            int shapeEnumValue = property.FindPropertyRelative("shape").enumValueIndex;
            if(shapeEnumValue == (int)BoxShape.Rectangle)
            {
                EditorGUI.PropertyField(sizeRect, property.FindPropertyRelative("size"), new GUIContent("Size"));
            }
            if (shapeEnumValue == (int)BoxShape.Circle || shapeEnumValue == (int)BoxShape.Capsule)
            {
                EditorGUI.PropertyField(radiusRect, property.FindPropertyRelative("radius"), new GUIContent("Radius"));
            }
            if (shapeEnumValue == (int)BoxShape.Capsule)
            {
                EditorGUI.PropertyField(heightRect, property.FindPropertyRelative("height"), new GUIContent("Height"));
            }

            EditorGUI.EndProperty();
        }
    }
}