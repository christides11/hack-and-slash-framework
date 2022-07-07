using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
            GetStateVariablesFromAssemblies();
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
            
            Button zoomIn = root.Q<Button>(name: "zoom-in");
            Button zoomOut = root.Q<Button>(name: "zoom-out");
            zoomIn.clicked += () => { 
                zoomMultiplier *= 1.5f;
                RefreshAll(true);
            };
            zoomOut.clicked += () =>
            {
                zoomMultiplier *= 0.5f;
                RefreshAll(true);
            };
        }

        public Dictionary<string, Type> stateVariableTypes = new Dictionary<string, Type>();
        protected virtual void GetStateVariablesFromAssemblies()
        {
            stateVariableTypes.Clear();
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();

            foreach (Assembly assembly in assemblies)
            {
                Type[] types = assembly.GetTypes();

                foreach (Type type in types)
                {
                    if (!Attribute.IsDefined(type, typeof(StateVariableAttribute))) continue;

                    StateVariableAttribute sva = type.GetCustomAttribute<StateVariableAttribute>();
                    stateVariableTypes.Add(sva.menuName, type);
                }
            }
        }

        public virtual void RefreshAll(bool refreshData = false)
        {
            if (stateTimeline == null)
            {
                Close();
                return;
            }

            RefreshSideBar();
            RefreshMainWindow();
            RefreshFrameBars();
            RefreshTopBar();
        }

        private void RefreshTopBar()
        {
            VisualElement root = rootVisualElement;
        }

        public void RefreshSideBar()
        {
            VisualElement root = rootVisualElement;
            ScrollView sidebarPanel = root.Q<ScrollView>(name: "data-labels");
            
            var labelsToDelete = sidebarPanel.contentContainer.Query(name: "sidebar-data-label").Build();
            foreach (var container in labelsToDelete)
            {
                sidebarPanel.contentContainer.Remove(container);
            }
            
            sidebarPanel.AddManipulator(new ContextualMenuManipulator((ContextualMenuPopulateEvent evt) =>
            {
                foreach (var c in stateVariableTypes)
                {
                    evt.menu.AppendAction("Add/"+c.Key, (x) => { AddStateVariable(c); });
                }
            }));
            
            for (int i = 0; i < stateTimeline.data.Length; i++)
            {
                int index = i;
                int dataID = stateTimeline.data[i].ID;
                int depth = stateTimeline.GetStateVariableDepth(dataID);
                dataBar.CloneTree(sidebarPanel.contentContainer);
                var thisSideBar = sidebarPanel.contentContainer.Query(name: "sidebar-data-label").Build().Last() as Button;

                thisSideBar.style.marginLeft = 10 * depth;
                thisSideBar.text =
                    !String.IsNullOrEmpty(stateTimeline.data[i].Name) ? stateTimeline.data[i].Name
                        : stateTimeline.data[i].GetType().Name;
                thisSideBar.AddManipulator(new ContextualMenuManipulator((ContextualMenuPopulateEvent evt) =>
                {
                    evt.menu.AppendAction("Edit", (x)=>{ StateTimelineDataEditor.Init(stateTimeline, stateTimeline.data[index].ID); });
                    foreach (var c in stateVariableTypes)
                    {
                        evt.menu.AppendAction("Add/"+c.Key, (x)=>{ AddStateVariable(c, dataID); });
                    }
                    evt.menu.AppendAction("Delete", (x) => { RemoveStateVariable(index); });
                }));
            }
        }

        private void RemoveStateVariable(int index)
        {
            stateTimeline.RemoveStateVariable(index);
        }

        private void AddStateVariable(KeyValuePair<string, Type> keyValuePair, int parentID = -1)
        {
            stateTimeline.AddStateVariable((IStateVariables)Activator.CreateInstance(keyValuePair.Value), parentID);
        }

        public void RefreshMainWindow()
        {
            VisualElement root = rootVisualElement;
            ScrollView mainPanel = root.Q<ScrollView>(name: "data-frames");
            
            var datasToDelete = mainPanel.contentContainer.Query(name: "main-framebar-bg").Build();
            foreach (var dataContainer in datasToDelete)
            {
                mainPanel.contentContainer.Remove(dataContainer);
            }
            
            for (int i = 0; i < stateTimeline.data.Length; i++)
            {
                mainFrameBarBG.CloneTree(mainPanel.contentContainer);

                var thisMainBar = mainPanel.contentContainer.Query(name: "main-framebar-bg").Build().Last();
            }
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
                c.style.width = new StyleLength((stateTimeline.totalFrames + 2) * GetFrameWidth());
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
                thisFrameLabelNumber.style.width = new StyleLength(GetFrameWidth());
            }
            
            // MAIN WINDOW //
            // 
            var dbs = dataBars.ToList();
            for (int i = 1; i < dbs.Count; i++)
            {
                for (int j = 0; j < stateTimeline.data[i - 1].FrameRanges.Length; j++)
                {
                    mainFrameBarLabel.CloneTree(dbs[i]);
                    var thisMainFrameBarLabel = dbs[i].Query(name: mainFrameBarLabel.name).Build().Last();
                    thisMainFrameBarLabel.style.left = GetFrameWidth() * stateTimeline.data[i - 1].FrameRanges[j].x;
                    thisMainFrameBarLabel.style.width = new StyleLength(GetFrameWidth() * ((stateTimeline.data[i - 1].FrameRanges[j].y - stateTimeline.data[i - 1].FrameRanges[j].x) + 1) );
                }
            }
        }

        public float GetFrameWidth()
        {
            return 20.0f * zoomMultiplier;
        }
    }
}