using UnityEditor;
using UnityEngine;

namespace HnSF.Input
{
    //[CustomPropertyDrawer(typeof(InputDefinition))]
    public class InputDefinitionDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            var typeRect = new Rect(position.x, position.y, position.width, 15);
            EditorGUI.PropertyField(typeRect, property.FindPropertyRelative("inputType"), GUIContent.none, true);

            InputDefinitionType enumType = (InputDefinitionType)property.FindPropertyRelative("inputType").enumValueIndex;

            switch (enumType)
            {
                case InputDefinitionType.Button:
                    var buttonRect = new Rect(position.x, position.y+20, position.width, 20);
                    EditorGUI.PropertyField(buttonRect, property.FindPropertyRelative("buttonID"), new GUIContent("ID"), true);
                    break;
                case InputDefinitionType.Stick:
                    var stickRect = new Rect(position.x, position.y + 20, position.width, 20);
                    EditorGUI.PropertyField(stickRect, property.FindPropertyRelative("stickDirection"), GUIContent.none, true);
                    var dirRect = new Rect(position.x, position.y + 40, position.width, 20);
                    EditorGUI.PropertyField(dirRect, property.FindPropertyRelative("directionDeviation"), new GUIContent("Deviation"), true);
                    break;
            }

            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            InputDefinitionType enumType = (InputDefinitionType)property.FindPropertyRelative("inputType").enumValueIndex;

            return enumType == InputDefinitionType.Button ? 45 : 70;
        }
    }
}