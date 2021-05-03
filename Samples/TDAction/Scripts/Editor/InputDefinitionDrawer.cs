using CAF.Input;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace TDAction
{
    [CustomPropertyDrawer(typeof(InputDefinition), true)]
    public class InputDefinitionDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            var typeRect = new Rect(position.x, position.y, position.width, 15);
            EditorGUI.PropertyField(typeRect, property.FindPropertyRelative("inputType"), GUIContent.none);

            InputDefinitionType enumType = (InputDefinitionType)property.FindPropertyRelative("inputType").enumValueIndex;

            switch (enumType)
            {
                case InputDefinitionType.Button:
                    var buttonRect = new Rect(position.x, position.y + 20, position.width, 20);
                    property.FindPropertyRelative("buttonID").intValue
                        = (int)(TDAction.Inputs.EntityInputs)EditorGUI.EnumPopup(buttonRect, (TDAction.Inputs.EntityInputs)property.FindPropertyRelative("buttonID").intValue);
                    break;
                case InputDefinitionType.Stick:
                    var stickRect = new Rect(position.x, position.y + 20, position.width, 20);
                    EditorGUI.PropertyField(stickRect, property.FindPropertyRelative("stickDirection"), GUIContent.none);
                    var dirRect = new Rect(position.x, position.y + 40, position.width, 20);
                    EditorGUI.PropertyField(dirRect, property.FindPropertyRelative("directionDeviation"), new GUIContent("Deviation"));
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