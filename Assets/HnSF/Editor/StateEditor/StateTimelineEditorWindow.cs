using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

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

        public Color[] depthColors = {
            Color.grey,
            new Color(0.4f, 0.4f, 0.4f)
        };

        public Color parentColor = new Color(0.05f, 0.05f, 0.05f);

        public static StateTimelineEditorWindow OpenWindow(StateTimeline stateTimeline)
        {
            StateTimelineEditorWindow wnd = CreateWindow<StateTimelineEditorWindow>();
            wnd.titleContent = new GUIContent(String.IsNullOrEmpty(stateTimeline.stateName) ? "State Timeline" : stateTimeline.stateName);
            wnd.minSize = new Vector2(400, 300);
            wnd.stateTimeline = stateTimeline;
            wnd.RefreshAll(true);
            return wnd;
        }

        public virtual void CreateGUI()
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
            Button refresh = root.Q<Button>(name: "refresh");
            zoomIn.clicked += () => { 
                zoomMultiplier *= 2.0f;
                RefreshAll(true);
            };
            zoomOut.clicked += () =>
            {
                zoomMultiplier *= 0.5f;
                RefreshAll(true);
            };
            refresh.clicked += () =>
            {
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
            stateTimeline.BuildStateVariablesIDMap();

            RefreshSideBar();
            RefreshMainWindow();
            RefreshFrameBars();
            RefreshTopBar();
        }

        public virtual void RefreshTopBar()
        {
            VisualElement root = rootVisualElement;
        }

        public virtual void RefreshSideBar()
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
                    evt.menu.AppendAction("Add/"+c.Key, (x) => { AddStateVariable(c); RefreshAll(true); });
                }
            }));
            
            StateTimeline[] stateChain = GetStateTimelineParents(stateTimeline);
            for (int s = 0; s < stateChain.Length; s++)
            {
                stateChain[s].Initialize();
                for (int i = 0; i < stateChain[s].data.Length; i++)
                {
                    if (stateChain[s].data[i].Parent != -1) continue;
                    int dataID = stateChain[s].data[i].ID;
                    SidebarDrawParentAndChildren(sidebarPanel, stateChain[s], dataID);
                }
            }
        }

        public virtual StateTimeline[] GetStateTimelineParents(StateTimeline startingTimeline)
        {
            List<StateTimeline> stateTimelineParents = new List<StateTimeline>();
            
            while (startingTimeline != null)
            {
                stateTimelineParents.Add(startingTimeline);
                startingTimeline = startingTimeline.useBaseState ? startingTimeline.baseState : null;
            }
            return stateTimelineParents.ToArray();
        }

        public virtual void SidebarDrawParentAndChildren(ScrollView sidebarPanel, StateTimeline stateTimeline, int dataID)
        {
            int index = stateTimeline.stateVariablesIDMap[dataID];
            int depth = stateTimeline.GetStateVariableDepth(dataID);
            
            dataBar.CloneTree(sidebarPanel.contentContainer);
            var thisSideBar = sidebarPanel.contentContainer.Query(name: "sidebar-data-label").Build().Last() as Button;
            
            thisSideBar.style.marginLeft = 10 * depth;
            thisSideBar.style.backgroundColor = this.stateTimeline != stateTimeline ? parentColor : depthColors[(depth+1) % Mathf.Abs(depthColors.Length)];
            thisSideBar.text =
                !String.IsNullOrEmpty(stateTimeline.data[index].Name) ? stateTimeline.data[index].Name
                    : stateTimeline.data[index].GetType().Name;
            thisSideBar.AddManipulator(new ContextualMenuManipulator((ContextualMenuPopulateEvent evt) =>
            {
                evt.menu.AppendAction("Edit", (x) =>
                {
                    var w = StateTimelineDataEditor.Init(stateTimeline, stateTimeline.data[index].ID);
                    w.onChanged += id => { RefreshAll(true); };
                });
                foreach (var c in stateVariableTypes)
                {
                    evt.menu.AppendAction("Add/"+c.Key, (x)=>{ AddStateVariable(c, dataID); RefreshAll(true); });
                }
                evt.menu.AppendAction("Delete", (x) => { RemoveStateVariable(index); RefreshAll(true); });
                evt.menu.AppendAction("Move Up", (x) => { MoveStateVarUp(stateTimeline, dataID); RefreshAll(true); });
                evt.menu.AppendAction("Move Down", (x) => { MoveStateVarDown(stateTimeline, dataID); RefreshAll(true); });
            }));

            if (stateTimeline.data[index].Children == null) return;
            for (int i = 0; i < stateTimeline.data[index].Children.Length; i++)
            {
                int childIndex = stateTimeline.stateVariablesIDMap[stateTimeline.data[index].Children[i]];
                SidebarDrawParentAndChildren(sidebarPanel, stateTimeline, stateTimeline.data[childIndex].ID);
            }
        }

        public virtual void MoveStateVarUp(StateTimeline stateTimeline, int id)
        {
            stateTimeline.MoveStateVariableUp(id);
        }
        
        public virtual void MoveStateVarDown(StateTimeline stateTimeline, int id)
        {
            stateTimeline.MoveStateVariableDown(id);
        }

        public virtual void RemoveStateVariable(int index)
        {
            stateTimeline.RemoveStateVariable(index);
        }

        public virtual void AddStateVariable(KeyValuePair<string, Type> keyValuePair, int parentID = -1)
        {
            stateTimeline.AddStateVariable((IStateVariables)Activator.CreateInstance(keyValuePair.Value), parentID);
        }

        public virtual void RefreshMainWindow()
        {
            VisualElement root = rootVisualElement;
            ScrollView mainPanel = root.Q<ScrollView>(name: "data-frames");
            
            var datasToDelete = mainPanel.contentContainer.Query(name: "main-framebar-bg").Build();
            foreach (var dataContainer in datasToDelete)
            {
                mainPanel.contentContainer.Remove(dataContainer);
            }
            
            StateTimeline[] stateChain = GetStateTimelineParents(stateTimeline);
            for (int s = 0; s < stateChain.Length; s++)
            {
                stateChain[s].Initialize();
                for (int i = 0; i < stateChain[s].data.Length; i++)
                {
                    if (stateChain[s].data[i].Parent != -1) continue;
                    int dataID = stateChain[s].data[i].ID;
                    MainWindowDrawParentAndChildren(mainPanel, stateChain[s], dataID);
                }
            }
        }

        public virtual void MainWindowDrawParentAndChildren(ScrollView mainPanel, StateTimeline stateTimeline, int dataID)
        {
            int index = stateTimeline.stateVariablesIDMap[dataID];
            int depth = stateTimeline.GetStateVariableDepth(dataID);
            
            mainFrameBarBG.CloneTree(mainPanel.contentContainer);
            var thisMainBar = mainPanel.contentContainer.Query(name: "main-framebar-bg").Build().Last();
            
            if (stateTimeline.data[index].Children == null) return;
            for (int i = 0; i < stateTimeline.data[index].Children.Length; i++)
            {
                int childIndex = stateTimeline.stateVariablesIDMap[stateTimeline.data[index].Children[i]];
                MainWindowDrawParentAndChildren(mainPanel, stateTimeline, stateTimeline.data[childIndex].ID);
            }
        }

        public virtual void RefreshFrameBars()
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
            var dbs = dataBars.ToList();
            dbs.RemoveAt(0);
            StateTimeline[] stateChain = GetStateTimelineParents(stateTimeline);
            int incr = 0;
            for (int s = 0; s < stateChain.Length; s++)
            {
                stateChain[s].Initialize();
                for (int i = 0; i < stateChain[s].data.Length; i++)
                {
                    if (stateChain[s].data[i].Parent != -1) continue;
                    int dataID = stateChain[s].data[i].ID;
                    DataBarsDrawParentAndChildren(dbs, dataID, incr, stateChain[s]);
                    //MainWindowDrawParentAndChildren(mainPanel, stateChain[s], dataID);
                }
            }
            /*for (int i = 0; i < stateTimeline.data.Length; i++)
            {
                if (stateTimeline.data[i].Parent != -1) continue;
                int dataID = stateTimeline.data[i].ID;
                DataBarsDrawParentAndChildren(dbs, dataID, 0);
            }*/
        }

        public virtual void DataBarsDrawParentAndChildren(List<VisualElement> dbs, int dataID, int incr, StateTimeline stateTimeline)
        {
            int index = stateTimeline.stateVariablesIDMap[dataID];

            if (stateTimeline.data[index].FrameRanges != null)
            {
                for (int j = 0; j < stateTimeline.data[index].FrameRanges.Length; j++)
                {
                    mainFrameBarLabel.CloneTree(dbs[incr]);
                    var thisMainFrameBarLabel = dbs[incr].Query(name: mainFrameBarLabel.name).Build().Last();
                    thisMainFrameBarLabel.style.left = GetFrameWidth() * stateTimeline.data[index].FrameRanges[j].x;
                    thisMainFrameBarLabel.style.width = new StyleLength(GetFrameWidth() *
                                                                        ((stateTimeline.data[index].FrameRanges[j].y -
                                                                          stateTimeline.data[index].FrameRanges[j].x) +
                                                                         1));
                    thisMainFrameBarLabel.Q<Label>().text = (stateTimeline.data[index].FrameRanges[j].y + 1 -
                                                             stateTimeline.data[index].FrameRanges[j].x).ToString();
                }
            }

            if (stateTimeline.data[index].Children == null) return;
            for (int i = 0; i < stateTimeline.data[index].Children.Length; i++)
            {
                int childIndex = stateTimeline.stateVariablesIDMap[stateTimeline.data[index].Children[i]];
                incr++;
                DataBarsDrawParentAndChildren(dbs, stateTimeline.data[childIndex].ID, incr, stateTimeline);
            }
        }

        public virtual float GetFrameWidth()
        {
            return 20.0f * zoomMultiplier;
        }
    }
}