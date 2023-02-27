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

        public static IStateVariables stateVariableCopy;
        public static IConditionVariables conditionVariablesCopy;
        
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
            zoomIn.clicked += () =>
            {
                ChangeZoomLevel(2.0f);
            };
            zoomOut.clicked += () =>
            {
                ChangeZoomLevel(0.5f);
            };
            refresh.clicked += () =>
            {
                RefreshAll(true);
            };

            Undo.undoRedoPerformed += OnUndoRedoPerformed;
        }

        private void OnDestroy()
        {
            Undo.undoRedoPerformed -= OnUndoRedoPerformed;
        }

        public virtual void OnUndoRedoPerformed()
        {
            RefreshAll(true);
        }

        public virtual void ChangeZoomLevel(float zoomMultiplier)
        {
            this.zoomMultiplier *= zoomMultiplier;
            RefreshAll(true);
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

        private ContextualMenuManipulator sidebarPanelMenuManipulator = null;
        public virtual void RefreshSideBar()
        {
            VisualElement root = rootVisualElement;
            ScrollView sidebarPanel = root.Q<ScrollView>(name: "data-labels");
            
            var labelsToDelete = sidebarPanel.contentContainer.Query(name: "sidebar-data-label").Build();
            foreach (var container in labelsToDelete)
            {
                sidebarPanel.contentContainer.Remove(container);
            }

            if(sidebarPanelMenuManipulator != null) sidebarPanel.RemoveManipulator(sidebarPanelMenuManipulator);
            sidebarPanelMenuManipulator = new ContextualMenuManipulator((ContextualMenuPopulateEvent evt) =>
            {
                foreach (var c in stateVariableTypes)
                {
                    evt.menu.AppendAction("Add/" + c.Key, (x) =>
                    {
                        AddStateVariable(c);
                        RefreshAll(true);
                    });
                }
                if (stateVariableCopy != null)
                {
                    evt.menu.AppendAction("Paste", (x) =>
                    {
                        stateTimeline.AddStateVariable(stateVariableCopy);
                        RefreshAll(true);
                    });
                }
            });
            sidebarPanel.AddManipulator(sidebarPanelMenuManipulator);
            
            StateTimeline[] stateChain = GetStateTimelineParents(stateTimeline);
            for (int s = 0; s < stateChain.Length; s++)
            {
                stateChain[s].Initialize();
                for (int i = 0; i < stateChain[s].data.Length; i++)
                {
                    if (stateChain[s].data[i].Parent != -1) continue;
                    int dataID = stateChain[s].data[i].ID;
                    SidebarCreateLabel(sidebarPanel, stateChain[s], dataID);
                }
            }
        }

        public virtual void SidebarCreateLabel(ScrollView sidebarPanel, StateTimeline stateTimeline, int dataID)
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
            SidebarSetupLabel(stateTimeline, dataID, thisSideBar, index);

            if (stateTimeline.data[index].Children == null) return;
            for (int i = 0; i < stateTimeline.data[index].Children.Length; i++)
            {
                int childIndex = stateTimeline.stateVariablesIDMap[stateTimeline.data[index].Children[i]];
                SidebarCreateLabel(sidebarPanel, stateTimeline, stateTimeline.data[childIndex].ID);
            }
        }
        
        public virtual void SidebarSetupLabel(StateTimeline stateTimeline, int dataID, Button thisSideBar, int index)
        {
            thisSideBar.AddManipulator(new ContextualMenuManipulator((ContextualMenuPopulateEvent evt) =>
            {
                evt.menu.AppendAction("Edit", (x) =>
                {
                    var w = StateTimelineDataEditor.Init(stateTimeline, stateTimeline.data[index].ID);
                    w.onChanged += id => { UpdateData(stateTimeline, stateTimeline.data[index].ID); };
                });
                foreach (var c in stateVariableTypes)
                {
                    evt.menu.AppendAction("Add/" + c.Key, (x) =>
                    {
                        AddStateVariable(c, dataID);
                        RefreshAll(true);
                    });
                }

                evt.menu.AppendAction("Delete", (x) =>
                {
                    RemoveStateVariable(index);
                    RefreshAll(true);
                });
                evt.menu.AppendAction("Copy", (x) =>
                {
                    CopyStateVariable(stateTimeline, index);
                });
                if (stateVariableCopy != null)
                {
                    evt.menu.AppendAction("Paste in Place", (x) =>
                    {
                        PasteInPlaceStateVariable(index);
                        RefreshAll(true);
                    });
                    evt.menu.AppendAction("Paste as Child", (x) =>
                    {
                        PasteAsChildStateVariable(index);
                        RefreshAll(true);
                    });
                }
                evt.menu.AppendAction("Move Up", (x) =>
                {
                    MoveStateVarUp(stateTimeline, dataID);
                    RefreshAll(true);
                });
                evt.menu.AppendAction("Move Down", (x) =>
                {
                    MoveStateVarDown(stateTimeline, dataID);
                    RefreshAll(true);
                });
            }));
        }

        protected virtual void CopyStateVariable(StateTimeline stateTimeline, int index)
        {
            stateVariableCopy = stateTimeline.CopyStateVariable(index);
        }

        protected virtual void PasteInPlaceStateVariable(int index)
        {
            if (stateVariableCopy == null) return;
            stateTimeline.PasteInPlace(index, stateVariableCopy);
        }

        protected virtual void PasteAsChildStateVariable(int index)
        {
            if (stateVariableCopy == null) return;
            stateTimeline.PasteAsChild(index, stateVariableCopy);
        }

        protected virtual void UpdateData(StateTimeline stateTimeline1, int id)
        {
            RefreshAll(true);
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
                    MainWindowCreateFrameBarBG(mainPanel, stateChain[s], dataID);
                }
            }
        }

        public virtual void MainWindowCreateFrameBarBG(ScrollView mainPanel, StateTimeline stateTimeline, int dataID)
        {
            int index = stateTimeline.stateVariablesIDMap[dataID];
            int depth = stateTimeline.GetStateVariableDepth(dataID);
            
            mainFrameBarBG.CloneTree(mainPanel.contentContainer);
            var thisMainBar = mainPanel.contentContainer.Query(name: "main-framebar-bg").Build().Last();
            
            if (stateTimeline.data[index].Children == null) return;
            for (int i = 0; i < stateTimeline.data[index].Children.Length; i++)
            {
                int childIndex = stateTimeline.stateVariablesIDMap[stateTimeline.data[index].Children[i]];
                MainWindowCreateFrameBarBG(mainPanel, stateTimeline, stateTimeline.data[childIndex].ID);
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
                    DataBarsDrawParentAndChildren(dbs, dataID, ref incr, stateChain[s]);
                    incr++;
                }
            }
        }

        public virtual void DataBarsDrawParentAndChildren(List<VisualElement> dbs, int dataID, ref int incr, StateTimeline stateTimeline)
        {
            int index = stateTimeline.stateVariablesIDMap[dataID];

            if (stateTimeline.data[index].FrameRanges != null)
            {
                for (int j = 0; j < stateTimeline.data[index].FrameRanges.Length; j++)
                {
                    int frx = ConvertFrameNumber((int)stateTimeline.data[index].FrameRanges[j].x);
                    int fry = ConvertFrameNumber((int)stateTimeline.data[index].FrameRanges[j].y);
                    int framebarStart = frx;
                    int framebarWidth = fry - frx;
                    mainFrameBarLabel.CloneTree(dbs[incr]);
                    var thisMainFrameBarLabel = dbs[incr].Query(name: mainFrameBarLabel.name).Build().Last();
                    thisMainFrameBarLabel.style.left = GetFrameWidth() * framebarStart;
                    thisMainFrameBarLabel.style.width = new StyleLength(GetFrameWidth() * (framebarWidth + 1));
                    thisMainFrameBarLabel.Q<Label>().text = (fry + 1 - frx).ToString();
                }
            }

            if (stateTimeline.data[index].Children == null) return;
            for (int i = 0; i < stateTimeline.data[index].Children.Length; i++)
            {
                int childIndex = stateTimeline.stateVariablesIDMap[stateTimeline.data[index].Children[i]];
                incr++;
                DataBarsDrawParentAndChildren(dbs, stateTimeline.data[childIndex].ID, ref incr, stateTimeline);
            }
        }

        public virtual int ConvertFrameNumber(int number)
        {
            if (number == -1) return stateTimeline.totalFrames;
            if (number == -2) return stateTimeline.totalFrames+1;
            return number;
        }

        public virtual float GetFrameWidth()
        {
            return 20.0f * zoomMultiplier;
        }
    }
}