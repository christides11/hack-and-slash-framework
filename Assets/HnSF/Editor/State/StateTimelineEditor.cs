using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace HnSF
{
    [CustomEditor(typeof(StateTimeline), true)]
    public class StateTimelineEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            StateTimeline st = (StateTimeline)target;

            st.totalFrames = (int)(st.duration / (1.0f / 60.0f));
            GUILayout.Label($"Total Frames: {st.totalFrames}", EditorStyles.boldLabel);
            //base.OnInspectorGUI();
        }
    }
}