using System;
using System.Collections.Generic;
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
    }
}