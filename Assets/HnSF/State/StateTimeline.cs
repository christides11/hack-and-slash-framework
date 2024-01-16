using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace HnSF
{
    public class StateTimeline : ScriptableObject
    {
        
        public string stateName;
        public bool useBaseState = false;
        public StateTimeline baseState;
        public int totalFrames = 10;
        public bool autoIncrement = true;
        public bool autoLoop = true;
        public int autoLoopFrame = 1;

        // ID : Index
        public Dictionary<int, int> stateVariablesIDMap = new Dictionary<int, int>();
        [SelectImplementation(typeof(IStateVariables))] [SerializeField, SerializeReference]
        public IStateVariables[] data = Array.Empty<IStateVariables>();

        public virtual void Initialize()
        {
            BuildStateVariablesIDMap();
        }

        public int GetHighestID()
        {
            if (data.Length == 0) return -1;
            int highestID = data[0].ID;
            for (int i = 1; i < data.Length; i++)
            {
                if (data[i].ID > highestID) highestID = data[i].ID;
            }
            return highestID;
        }
        
        public virtual void BuildStateVariablesIDMap()
        {
            if (stateVariablesIDMap == null) stateVariablesIDMap = new Dictionary<int, int>();
            stateVariablesIDMap.Clear();
            for (int i = 0; i < data.Length; i++)
            {
                stateVariablesIDMap.Add(data[i].ID, i);
            }
            if(useBaseState) baseState.BuildStateVariablesIDMap();
        }

        public int GetStateVariableDepth(int id)
        {
            int depth = 0;

            int idx = stateVariablesIDMap[id];
            while (data[idx].Parent != -1)
            {
                depth++;
                idx = stateVariablesIDMap[data[idx].Parent];
            }
            
            return depth;
        }

        public void MoveStateVariableUp(int id)
        {
            int currentIndex = stateVariablesIDMap[id];

            if (data[currentIndex].Parent != -1)
            {
                int parentIndex = stateVariablesIDMap[data[currentIndex].Parent];
                if (data[parentIndex].Children.Length <= 1) return;
                int childIndex = Array.IndexOf(data[parentIndex].Children, id);
                if (childIndex == 0) return;
                #if UNITY_EDITOR
                UndoUtility.RecordObject(this, "Moved State Variable");
                #endif
                data[parentIndex].Children.SwapValues(childIndex, childIndex-1);
            }
            else
            {
                if (currentIndex == 0) return;
                #if UNITY_EDITOR
                UndoUtility.RecordObject(this, "Moved State Variable");
                #endif
                data.SwapValues(currentIndex, currentIndex-1);
            }
        }

        public void MoveStateVariableDown(int id)
        {
            int currentIndex = stateVariablesIDMap[id];

            if (data[currentIndex].Parent != -1)
            {
                int parentIndex = stateVariablesIDMap[data[currentIndex].Parent];
                if (data[parentIndex].Children.Length <= 1) return;
                int childIndex = Array.IndexOf(data[parentIndex].Children, id);
                if (childIndex == data[parentIndex].Children.Length-1) return;
                #if UNITY_EDITOR
                UndoUtility.RecordObject(this, "Moved State Variable");
                #endif
                data[parentIndex].Children.SwapValues(childIndex, childIndex+1);
            }
            else
            {
                if (currentIndex == data.Length-1) return;
                #if UNITY_EDITOR
                UndoUtility.RecordObject(this, "Moved State Variable");
                #endif
                data.SwapValues(currentIndex, currentIndex+1);
            }
        }

        public int AddStateVariable(IStateVariables var, int parentID = -1)
        {
            BuildStateVariablesIDMap();
            #if UNITY_EDITOR
            UndoUtility.RecordObject(this, "Added State Variable");
            #endif
            Array.Resize(ref data, data.Length+1);
            data[^1] = var;
            data[^1].ID = data.Length == 1 ? 0 : GetHighestID() + 1;
            data[^1].Parent = parentID;
            data[^1].Children = Array.Empty<int>();
            data[^1].FrameRanges = Array.Empty<Vector2Int>();
            int dataIndex = data.Length - 1;
            if (parentID != -1)
            {
                int parentIndex = stateVariablesIDMap[parentID];
                
                var tempChildren = data[parentIndex].Children;
                if (tempChildren == null) tempChildren = Array.Empty<int>();
                Array.Resize(ref tempChildren, tempChildren.Length+1);
                tempChildren[^1] = data[^1].ID;
                data[parentIndex].Children = tempChildren;
            }
            var.SetupDefaults();
            return dataIndex;
        }

        public void RemoveStateVariable(int index)
        {
            BuildStateVariablesIDMap();
            #if UNITY_EDITOR
            UndoUtility.RecordObject(this, "Removed State Variable");
            #endif
            List<IStateVariables> tempData = data.ToList();
            
            List<int> stateVarsToRemove = new List<int>();

            Queue<int> stateVarsToCheck = new Queue<int>();
            
            stateVarsToCheck.Enqueue(data[index].ID);

            if (data[index].Parent != -1)
            {
                List<int> temp = new List<int>(tempData[stateVariablesIDMap[data[index].Parent]].Children);
                temp.Remove(data[index].ID);
                tempData[stateVariablesIDMap[data[index].Parent]].Children = temp.ToArray();
            }

            while (stateVarsToCheck.Count > 0)
            {
                int idx = stateVariablesIDMap[stateVarsToCheck.Dequeue()];
                stateVarsToRemove.Add(idx);

                for (int i = 0; i < data[idx].Children.Length; i++)
                {
                    stateVarsToCheck.Enqueue(data[idx].Children[i]);
                }
            }
            
            stateVarsToRemove.Sort();

            for (int i = stateVarsToRemove.Count - 1; i >= 0; i--)
            {
                tempData.RemoveAt(stateVarsToRemove[i]);
            }

            data = tempData.ToArray();
        }

        public IStateVariables CopyStateVariable(int index)
        {
            var copyData = data[index].Copy();
            copyData.Name = data[index].Name;
            copyData.FrameRanges = new Vector2Int[0];
            if (data[index].FrameRanges != null)
            {
                copyData.FrameRanges = new Vector2Int[data[index].FrameRanges.Length];
                for (int i = 0; i < data[index].FrameRanges.Length; i++)
                    copyData.FrameRanges[i] = data[index].FrameRanges[i];
            }
            copyData.Condition = data[index].Condition?.Copy();
            copyData.RunDuringHitstop = data[index].RunDuringHitstop;
            return copyData;
        }

        public IStateVariables[] CopyStateVariableTree(int index)
        {
            return null;
        }

        public void PasteInPlace(int index, IStateVariables newData)
        {
#if UNITY_EDITOR
            UndoUtility.RecordObject(this, "Pasted State Variable");
#endif
            var oldData = data[index];

            var nameCopy = oldData.Name;
            var idCopy = oldData.ID;
            var parentCopy = oldData.Parent;
            int[] childrenCopy = oldData.Children;
            Vector2Int[] frameRangeCopy = new Vector2Int[0];
            if (oldData.FrameRanges != null)
            {
                frameRangeCopy = new Vector2Int[oldData.FrameRanges.Length];
                for (int i = 0; i < oldData.FrameRanges.Length; i++)
                    frameRangeCopy[i] = oldData.FrameRanges[i];
            }
            var conditionCopy = newData.Condition?.Copy();
            var runDuringHitstopCopy = newData.RunDuringHitstop;

            data[index] = newData.Copy();
            data[index].Name = nameCopy;
            data[index].ID = idCopy;
            data[index].Parent = parentCopy;
            data[index].Children = childrenCopy;
            data[index].FrameRanges = frameRangeCopy;
            data[index].Condition = conditionCopy;
            data[index].RunDuringHitstop = runDuringHitstopCopy;
        }

        public void PasteAsChild(int parentIndex, IStateVariables wantedChildData)
        {
#if UNITY_EDITOR
            UndoUtility.RecordObject(this, "Pasted State Variable as Child");
#endif
            var nameCopy = wantedChildData.Name;
            var frameRangeCopy = new Vector2Int[0];
            if (wantedChildData.FrameRanges != null)
            {
                frameRangeCopy = new Vector2Int[wantedChildData.FrameRanges.Length];
                for (int i = 0; i < wantedChildData.FrameRanges.Length; i++)
                    frameRangeCopy[i] = wantedChildData.FrameRanges[i];
            }
            var conditionCopy = wantedChildData.Condition?.Copy();
            var hitstopCopy = wantedChildData.RunDuringHitstop;
            var dataIndex = AddStateVariable(wantedChildData.Copy(), parentIndex);
            data[dataIndex].Name = nameCopy;
            data[dataIndex].FrameRanges = frameRangeCopy;
            data[dataIndex].Condition = conditionCopy;
            data[dataIndex].RunDuringHitstop = hitstopCopy;
        }
    }
}