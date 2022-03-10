using System;
using System.Collections.Generic;
using UnityEngine;

namespace HnSF.Combat
{
    public class MovesetDefinition : ScriptableObject
    {
        [NonSerialized] public Dictionary<int, StateTimeline> stateMap;
        [Header("States")] [SerializeField] protected IntStateMap[] states = Array.Empty<IntStateMap>();
    }
}