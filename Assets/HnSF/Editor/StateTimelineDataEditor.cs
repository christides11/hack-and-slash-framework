using UnityEditor;
using UnityEngine;

namespace HnSF
{
    public class StateTimelineDataEditor : EditorWindow
    {
        public delegate void EmptyAction(int ID);
        public EmptyAction onChanged;
        
        [SerializeField] public StateTimelineEditorWindow currentTimeline;

        public int id;
        public StateTimeline state;

        public static StateTimelineDataEditor Init(StateTimeline stateTimeline, int id)
        {
            stateTimeline.BuildStateVariablesIDMap();
            StateTimelineDataEditor window =
                (StateTimelineDataEditor)EditorWindow.GetWindow(typeof(StateTimelineDataEditor));
            window.state = stateTimeline;
            window.id = id;
            window.Show();
            return window;
        }

        protected Vector2 _scrollPos = Vector2.zero;
        protected virtual void OnGUI()
        {
            var so = new SerializedObject(state);
            SerializedProperty sp = so.FindProperty("data").GetArrayElementAtIndex(state.stateVariablesIDMap[id]);

            _scrollPos = EditorGUILayout.BeginScrollView(_scrollPos);
            EditorGUILayout.PropertyField(sp, GUIContent.none, includeChildren: true);
            
            EditorGUILayout.LabelField("CHILDREN", EditorStyles.boldLabel);
            var d = state.data[state.stateVariablesIDMap[id]];
            if (d.Children == null) d.Children = new int[0];
            for (int i = 0; i < d.Children.Length; i++)
            {
                EditorGUILayout.PropertyField(so.FindProperty("data").GetArrayElementAtIndex(state.stateVariablesIDMap[d.Children[i]]), GUIContent.none, includeChildren: true);
            }
            EditorGUILayout.EndScrollView();
            
            so.ApplyModifiedProperties();
            
            if (GUI.changed)
            {
                onChanged?.Invoke(id);
            }
        }
    }
}