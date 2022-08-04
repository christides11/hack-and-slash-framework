using System;
using System.Collections.Generic;
using System.Linq;
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
                UndoUtility.RecordObject(this, "Moved State Variable");
                data[parentIndex].Children.SwapValues(childIndex, childIndex-1);
            }
            else
            {
                if (currentIndex == 0) return;
                UndoUtility.RecordObject(this, "Moved State Variable");
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
                UndoUtility.RecordObject(this, "Moved State Variable");
                data[parentIndex].Children.SwapValues(childIndex, childIndex+1);
            }
            else
            {
                if (currentIndex == data.Length-1) return;
                UndoUtility.RecordObject(this, "Moved State Variable");
                data.SwapValues(currentIndex, currentIndex+1);
            }
        }

        public void AddStateVariable(IStateVariables var, int parentID = -1)
        {
            BuildStateVariablesIDMap();
            UndoUtility.RecordObject(this, "Added State Variable");
            Array.Resize(ref data, data.Length+1);
            data[^1] = var;
            data[^1].ID = data.Length == 1 ? 0 : GetHighestID() + 1;
            data[^1].Parent = parentID;
            data[^1].Children = Array.Empty<int>();
            data[^1].FrameRanges = Array.Empty<Vector2Int>();
            if (parentID != -1)
            {
                int parentIndex = stateVariablesIDMap[parentID];
                
                var tempChildren = data[parentIndex].Children;
                if (tempChildren == null) tempChildren = Array.Empty<int>();
                Array.Resize(ref tempChildren, tempChildren.Length+1);
                tempChildren[^1] = data[^1].ID;
                data[parentIndex].Children = tempChildren;
            }
        }

        public void RemoveStateVariable(int index)
        {
            BuildStateVariablesIDMap();
            UndoUtility.RecordObject(this, "Removed State Variable");
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
    }
}