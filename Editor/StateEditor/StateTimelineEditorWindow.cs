using System;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using UnityEngine.Serialization;

namespace HnSF
{
    public class StateTimelineEditorWindow : EditorWindow
    {
        public VisualTreeAsset tree;
        public VisualTreeAsset dataBar;
        public VisualTreeAsset mainFrameBarBG;
        public VisualTreeAsset topbarFrameLabel;
        public VisualTreeAsset mainFrameBarLabel;

        [SerializeField] public float zoomMultiplier = 1.0f;
        [SerializeField] public StateTimeline stateTimeline;

        public Color frameZeroColor = new Color(0.1141082f, 0.5f, 0);
        public Color frameInterruptColor = new Color(0.5019608f, 0, 0.009442259f);

        public static StateTimelineEditorWindow OpenWindow(StateTimeline stateTimeline)
        {
            StateTimelineEditorWindow wnd = CreateWindow<StateTimelineEditorWindow>();
            wnd.titleContent = new GUIContent("AttackTimeline");
            wnd.minSize = new Vector2(400, 300);
            wnd.stateTimeline = stateTimeline;
            wnd.RefreshAll(true);
            return wnd;
        }

        public void CreateGUI()
        {
            VisualElement root = rootVisualElement;
            tree.CloneTree(root);
            ScrollView labelPanel = root.Q<ScrollView>(name: "data-labels");
            ScrollView dataPanel = root.Q<ScrollView>(name: "data-frames");

            VisualElement ve = root.Q<VisualElement>(name: "frame-padding");


            // SCROLLING
            labelPanel.verticalScroller.valueChanged += (v) => { dataPanel.verticalScroller.value = v; };

            labelPanel.contentContainer.RegisterCallback<WheelEvent>(@event =>
            {
                labelPanel.verticalScroller.value += @event.delta.y * labelPanel.verticalPageSize;
                @event.StopPropagation();
            });

            dataPanel.contentContainer.RegisterCallback<WheelEvent>(@event =>
            {
                dataPanel.horizontalScroller.value += @event.delta.y * dataPanel.horizontalPageSize;
                @event.StopPropagation();
            });
        }

        public void RefreshAll(bool refreshData = false)
        {
            VisualElement root = rootVisualElement;
            ScrollView labelPanel = root.Q<ScrollView>(name: "data-labels");
            ScrollView dataPanel = root.Q<ScrollView>(name: "data-frames");

            if (stateTimeline == null) return;
            if (refreshData)
            {
                var labelsToDelete = labelPanel.contentContainer.Query(name: "data-bar").Build();
                var datasToDelete = dataPanel.contentContainer.Query(name: "frame-bar").Build();
                foreach (var container in labelsToDelete)
                {
                    labelPanel.contentContainer.Remove(container);
                }

                foreach (var dataContainer in datasToDelete)
                {
                    dataPanel.contentContainer.Remove(dataContainer);
                }

                for (int i = 0; i < stateTimeline.data.Length; i++)
                {
                    dataBar.CloneTree(labelPanel.contentContainer);
                    mainFrameBarBG.CloneTree(dataPanel.contentContainer);

                    var thisSideBar = labelPanel.contentContainer.Query(name: "sidebar-data-label").Build().Last();
                    var thisMainBar = dataPanel.contentContainer.Query(name: "main-framebar-bg").Build().Last();

                    thisSideBar.Q<UnityEngine.UIElements.Label>(name: "label-text").text =
                        !String.IsNullOrEmpty(stateTimeline.data[i].Name) ? stateTimeline.data[i].Name
                        : stateTimeline.data[i].GetType().Name;
                }
            }

            RefreshFrameBars();
        }

        public void RefreshFrameBars()
        {
            VisualElement root = rootVisualElement;
            ScrollView labelPanel = root.Q<ScrollView>(name: "data-labels");
            ScrollView dataPanel = root.Q<ScrollView>(name: "data-frames");

            // Frame bar lengths
            var dataBars = dataPanel.contentContainer.Query(className: "frame-bar").Build();

            foreach (var c in dataBars)
            {
                c.style.width = new StyleLength((stateTimeline.totalFrames + 2) * 20.0f);
            }

            // TOPBAR //
            // Cleanup
            var frameLabels = dataBars.First().Query(name: topbarFrameLabel.name).Build();
            foreach (var fl in frameLabels)
            {
                dataBars.First().Remove(fl);
            }

            // Create frame numbers
            for (int i = 0; i < stateTimeline.totalFrames + 2; i++)
            {
                topbarFrameLabel.CloneTree(dataBars.First());
                var thisFrameLabelNumber = dataBars.First().Query(name: topbarFrameLabel.name).Build().Last();
                Label l = thisFrameLabelNumber.Children().First() as Label;
                l.text = $"{i}";
                if (i == 0) l.style.backgroundColor = frameZeroColor;
                if (i == stateTimeline.totalFrames + 1) l.style.backgroundColor = frameInterruptColor;
            }
            
            // MAIN WINDOW //
            // Cleanup
            var dbs = dataBars.ToList();
            for (int i = 1; i < dbs.Count; i++)
            {
                for (int j = 0; j < stateTimeline.data[i - 1].FrameRanges.Length; j++)
                {
                    mainFrameBarLabel.CloneTree(dbs[i]);
                    var thisMainFrameBarLabel = dbs[i].Query(name: mainFrameBarLabel.name).Build().Last();
                    thisMainFrameBarLabel.style.left = 20.0f * stateTimeline.data[i - 1].FrameRanges[j].x;
                    thisMainFrameBarLabel.style.width = new StyleLength(20.0f * ((stateTimeline.data[i - 1].FrameRanges[j].y - stateTimeline.data[i - 1].FrameRanges[j].x) + 1) );
                }
            }
        }
    }
}