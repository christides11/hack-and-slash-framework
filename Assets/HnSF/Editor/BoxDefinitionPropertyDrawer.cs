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
            var offsetLabelRect = new Rect(position.x, yPosition, 100, lineHeight);
            var offsetRect = new Rect(position.x+120, yPosition, position.width-120, lineHeight);
            yPosition += lineSpacing;
            var rotationLabelRect = new Rect(position.x, yPosition, 100, lineHeight);
            var rotationRect = new Rect(position.x+120, yPosition, position.width-120, lineHeight);
            yPosition += lineSpacing;
            var sizeRadiusLabelRect = new Rect(position.x, yPosition, 100, lineHeight);
            var sizeRect = new Rect(position.x+120, yPosition, position.width-120, lineHeight);
            var radiusRect = new Rect(position.x+120, yPosition, position.width-120, lineHeight);
            yPosition += lineSpacing;
            var heightLabelRect = new Rect(position.x, yPosition, 100, lineHeight);
            var heightRect = new Rect(position.x+120, yPosition, position.width-120, lineHeight);

            EditorGUI.PropertyField(shapeRect, property.FindPropertyRelative("shape"), new GUIContent("Shape"));
            EditorGUI.LabelField(offsetLabelRect, new GUIContent("Offset"));
            EditorGUI.PropertyField(offsetRect, property.FindPropertyRelative("offset"), GUIContent.none);
            EditorGUI.LabelField(rotationLabelRect, new GUIContent("Rotation"));
            EditorGUI.PropertyField(rotationRect, property.FindPropertyRelative("rotation"), GUIContent.none);
            
            
            int shapeEnumValue = property.FindPropertyRelative("shape").enumValueIndex;
            if(shapeEnumValue == (int)BoxShape.Rectangle)
            {
                EditorGUI.LabelField(sizeRadiusLabelRect, new GUIContent("Size"));
                EditorGUI.PropertyField(sizeRect, property.FindPropertyRelative("size"), GUIContent.none);
            }
            if (shapeEnumValue == (int)BoxShape.Circle || shapeEnumValue == (int)BoxShape.Capsule)
            {
                EditorGUI.LabelField(sizeRadiusLabelRect, new GUIContent("Radius"));
                EditorGUI.PropertyField(radiusRect, property.FindPropertyRelative("radius"), GUIContent.none);
            }
            if (shapeEnumValue == (int)BoxShape.Capsule)
            {
                EditorGUI.LabelField(heightLabelRect, new GUIContent("Height"));
                EditorGUI.PropertyField(heightRect, property.FindPropertyRelative("height"), GUIContent.none);
            }

            EditorGUI.EndProperty();
        }
    }
}