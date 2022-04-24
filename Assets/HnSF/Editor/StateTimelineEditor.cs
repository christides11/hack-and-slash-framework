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
            base.OnInspectorGUI();
        }
    }
}