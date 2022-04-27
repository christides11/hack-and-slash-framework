using System;
using System.Collections.Generic;
using UnityEngine;

namespace HnSF.Combat
{
    public class MovesetDefinition : ScriptableObject
    {
        [NonSerialized] public Dictionary<int, StateTimeline> stateMap;
        [Header("States")] [SerializeField] protected IntStateMap[] states = Array.Empty<IntStateMap>();
        
        public virtual void Initialize()
        {
            if (stateMap == null)
            {
                stateMap = new Dictionary<int, HnSF.StateTimeline>();
            }

            if (stateMap.Count == states.Length) return;
            stateMap.Clear();
            foreach (var intStateMap in states)
            {
                stateMap.Add(intStateMap.state.GetState(), intStateMap.stateTimeline);
            }
        }
    }
}