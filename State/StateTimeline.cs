using System;
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
        
        [SelectImplementation(typeof(IStateVariables))] [SerializeField, SerializeReference]
        public IStateVariables[] data = Array.Empty<IStateVariables>();
    }
}