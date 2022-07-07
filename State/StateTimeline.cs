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