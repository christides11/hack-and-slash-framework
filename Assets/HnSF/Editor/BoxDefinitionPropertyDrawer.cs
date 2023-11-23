using UnityEditor;
using UnityEngine;

namespace HnSF.Combat
{
    [CustomPropertyDrawer(typeof(BoxDefinition), true)]
    public class BoxDefinitionPropertyDrawer : PropertyDrawer
    {
        protected float lineSpacing = 20;

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

            yPosition = DrawSharedProperties(position, property, lineHeight, yPosition);
            yPosition += lineSpacing;
            yPosition = DrawShapeProperties(position, property, lineHeight, yPosition);

            EditorGUI.EndProperty();
        }

        protected virtual float DrawSharedProperties(Rect position, SerializedProperty property, float lineHeight, float yPosition)
        {
            EditorGUI.PropertyField(new Rect(position.x, yPosition, position.width, lineHeight), property.FindPropertyRelative("shape"), new GUIContent("Shape"), true);
            yPosition += lineSpacing;
            EditorGUI.LabelField(new Rect(position.x, yPosition, 100, lineHeight), new GUIContent("Offset"));
            EditorGUI.PropertyField(new Rect(position.x + 120, yPosition, position.width - 120, lineHeight), property.FindPropertyRelative("offset"), GUIContent.none, true);
            yPosition += lineSpacing;
            EditorGUI.LabelField(new Rect(position.x, yPosition, 100, lineHeight), new GUIContent("Rotation"));
            EditorGUI.PropertyField(new Rect(position.x + 120, yPosition, position.width - 120, lineHeight), property.FindPropertyRelative("rotation"), GUIContent.none, true);
            return yPosition;
        }

        protected virtual float DrawShapeProperties(Rect position, SerializedProperty property, float lineHeight, float yPosition)
        {
            int shapeEnumValue = property.FindPropertyRelative("shape").enumValueIndex;
            if (shapeEnumValue == (int)BoxShape.Rectangle)
            {
                EditorGUI.LabelField(new Rect(position.x, yPosition, 100, lineHeight), new GUIContent("Size"));
                EditorGUI.PropertyField(new Rect(position.x + 120, yPosition, position.width - 120, lineHeight), property.FindPropertyRelative("size"), GUIContent.none, true);
            }
            if (shapeEnumValue == (int)BoxShape.Circle || shapeEnumValue == (int)BoxShape.Capsule)
            {
                EditorGUI.LabelField(new Rect(position.x, yPosition, 100, lineHeight), new GUIContent("Radius"));
                EditorGUI.PropertyField(new Rect(position.x + 120, yPosition, position.width - 120, lineHeight), property.FindPropertyRelative("radius"), GUIContent.none, true);
            }
            yPosition += lineSpacing;
            if (shapeEnumValue == (int)BoxShape.Capsule)
            {
                EditorGUI.LabelField(new Rect(position.x, yPosition, 100, lineHeight), new GUIContent("Height"));
                EditorGUI.PropertyField(new Rect(position.x + 120, yPosition, position.width - 120, lineHeight), property.FindPropertyRelative("height"), GUIContent.none, true);
            }
            return yPosition;
        }
    }
}