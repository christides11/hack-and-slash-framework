using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace HnSF
{
    [CustomEditor(typeof(StateTimeline), true)]
    public class StateTimelineEditor : Editor
    {
        public VisualTreeAsset inspectorXML;

        [SerializeField] public StateTimelineEditorWindow currentTimeline;

        public override VisualElement CreateInspectorGUI()
        {
            VisualElement inspector = new VisualElement();

            inspectorXML.CloneTree(inspector);

            // Default Inspector
            VisualElement inspectorFoldout = inspector.Q("Default_Inspector");
            InspectorElement.FillDefaultInspector(inspectorFoldout, serializedObject, this);

            Button openEditor = inspector.Q<Button>(name: "OpenEditor");
            openEditor.clicked += () =>
            {
                StateTimeline st = (StateTimeline)target;
                st.BuildStateVariablesIDMap();
                currentTimeline = StateTimelineEditorWindow.OpenWindow(st);
            };

            return inspector;
        }
        /*public override void OnInspectorGUI()
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
        }*/
    }
}