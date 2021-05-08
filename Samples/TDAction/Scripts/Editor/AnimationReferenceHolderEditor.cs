using UnityEngine;
using UnityEditor;

namespace TDAction.Combat
{
    [CustomEditor(typeof(AnimationReferenceHolder), true)]
    public class AnimationReferenceHolderEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            var animationsProperty = serializedObject.FindProperty("animations");

            for (int i = 0; i < animationsProperty.arraySize; i++)
            {
                var entry = animationsProperty.GetArrayElementAtIndex(i);

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.PropertyField(entry.FindPropertyRelative("animationName"), GUIContent.none);
                EditorGUILayout.PropertyField(entry.FindPropertyRelative("animation"), GUIContent.none);
                if(GUILayout.Button("X", GUILayout.Width(30)))
                {
                    animationsProperty.DeleteArrayElementAtIndex(i);
                    EditorGUILayout.EndHorizontal();
                    break;
                }
                EditorGUILayout.EndHorizontal();
                GUILayout.Space(5);
            }

            if(GUILayout.Button("Add Animation"))
            {
                animationsProperty.InsertArrayElementAtIndex(animationsProperty.arraySize);
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}