using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace HnSF
{
    [CustomEditor(typeof(StateTimeline), true)]
    public class StateTimelineEditor : Editor
    {
        [SerializeField] public StateTimelineEditorWindow currentTimeline;

        public override void OnInspectorGUI()
        {
            StateTimeline st = (StateTimeline)target;
            base.OnInspectorGUI();
            if (GUILayout.Button("Open Editor"))
            {
                st.BuildStateVariablesIDMap();
                currentTimeline = StateTimelineEditorWindow.OpenWindow(st);
            }

            if (GUI.changed)
            {
                if (currentTimeline)
                {
                    currentTimeline.RefreshFrameBars();
                }
            }
        }
    }
}