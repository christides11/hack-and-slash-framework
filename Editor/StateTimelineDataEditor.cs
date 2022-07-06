using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace HnSF
{
    public class StateTimelineDataEditor : EditorWindow
    {
        [SerializeField] public StateTimelineEditorWindow currentTimeline;

        public int id;
        public StateTimeline state;

        public static void Init(StateTimeline stateTimeline, int id)
        {
            stateTimeline.BuildStateVariablesIDMap();
            StateTimelineDataEditor window =
                (StateTimelineDataEditor)EditorWindow.GetWindow(typeof(StateTimelineDataEditor));
            window.state = stateTimeline;
            window.id = id;
            window.Show();
        }

        private Vector2 _scrollPos = Vector2.zero;
        private void OnGUI()
        {
            SerializedObject so = new SerializedObject(state);
            SerializedProperty sp = so.FindProperty("data").GetArrayElementAtIndex(state.stateVariablesIDMap[id]);

            EditorGUILayout.PropertyField(sp, GUIContent.none);
            
            EditorGUILayout.LabelField("CHILDREN", EditorStyles.boldLabel);
            var d = state.data[state.stateVariablesIDMap[id]];
            for (int i = 0; i < d.Children.Length; i++)
            {
                EditorGUILayout.PropertyField(so.FindProperty("data").GetArrayElementAtIndex(state.stateVariablesIDMap[d.Children[i]]), GUIContent.none);
            }

            so.ApplyModifiedProperties();
        }
    }
}