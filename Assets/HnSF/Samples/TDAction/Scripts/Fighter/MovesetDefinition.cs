using System;
using System.Collections;
using System.Collections.Generic;
using HnSF.Combat;
using UnityEngine;
using UnityEngine.Serialization;

namespace HnSF.Sample.TDAction
{
    [CreateAssetMenu(menuName = "2dAction/Moveset")]
    public class MovesetDefinition : ScriptableObject, IMovesetDefinition
    {
        [NonSerialized] protected Dictionary<int, HnSF.StateTimeline> stateMap;
        [FormerlySerializedAs("states")]
        [Header("States")] 
        [SerializeField] protected IntStateMap[] groundActions = Array.Empty<IntStateMap>();
        [SerializeField] protected IntStateMap[] airActions = Array.Empty<IntStateMap>();
        [SerializeField] protected IntStateMap[] groundAttacks = Array.Empty<IntStateMap>();
        [SerializeField] protected IntStateMap[] airAttacks = Array.Empty<IntStateMap>();
        
        public virtual void Initialize()
        {
            if (stateMap == null) stateMap = new Dictionary<int, HnSF.StateTimeline>();
            if (stateMap.Count ==
                groundActions.Length + airActions.Length + groundAttacks.Length + airAttacks.Length) return;
            stateMap.Clear();

            foreach (var action in groundActions)
            {
                stateMap.Add(action.state.GetState(), action.stateTimeline);
            }
            
            foreach (var action in airActions)
            {
                stateMap.Add(action.state.GetState(), action.stateTimeline);
            }
            
            foreach (var action in groundAttacks)
            {
                stateMap.Add(action.state.GetState(), action.stateTimeline);
            }
            
            foreach (var action in airAttacks)
            {
                stateMap.Add(action.state.GetState(), action.stateTimeline);
            }
        }

        public HnSF.StateTimeline GetState(int stateID)
        {
            return stateMap[stateID];
        }
    }
}