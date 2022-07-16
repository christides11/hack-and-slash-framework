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
        
        public virtual void BuildStateVariablesIDMap()
        {
            stateVariablesIDMap.Clear();
            for (int i = 0; i < data.Length; i++)
            {
                stateVariablesIDMap.Add(data[i].ID, i);
            }
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

        public void AddStateVariable(IStateVariables var, int parentID = -1)
        {
            Array.Resize(ref data, data.Length+1);
            data[^1] = var;
            data[^1].ID = data.Length == 1 ? 0 : data[^2].ID + 1;
            data[^1].Parent = parentID;
            data[^1].Children = Array.Empty<int>();
            data[^1].FrameRanges = Array.Empty<Vector2>();
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
            List<IStateVariables> tempData = data.ToList();
            
            List<int> stateVarsToRemove = new List<int>();

            Queue<int> stateVarsToCheck = new Queue<int>();
            stateVarsToCheck.Enqueue(data[index].ID);
            
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