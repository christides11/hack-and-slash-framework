using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace HnSF.Input
{
    [System.Serializable]
    public class InputDefinition
    {
        public InputDefinitionType inputType;

        public int buttonID;
        //Stick
        public Vector2 stickDirection;
        public float directionDeviation = 0.9f; // Directions are compared using dot product.

        public virtual void DrawInspector()
        {
#if UNITY_EDITOR
            inputType = (InputDefinitionType)EditorGUILayout.EnumPopup(inputType);
            switch (inputType)
            {
                case InputDefinitionType.Button:
                    buttonID = EditorGUILayout.IntField("ID", buttonID);
                    break;
                case InputDefinitionType.Stick:
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("X");
                    stickDirection.x = EditorGUILayout.FloatField(stickDirection.x);
                    EditorGUILayout.LabelField("Y");
                    stickDirection.y = EditorGUILayout.FloatField(stickDirection.y);
                    EditorGUILayout.EndHorizontal();
                    directionDeviation = EditorGUILayout.FloatField("Deviation", directionDeviation);
                    break;
            }
#endif
        }
    }
}