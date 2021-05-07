using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace CAF.Combat
{
    [CustomPropertyDrawer(typeof(AttackEvent))]
    public class AttackEventPropertyDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return base.GetPropertyHeight(property, label);
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.PropertyField(position, property);
        }
    }
}