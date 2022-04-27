using System;
using HnSF.Fighters;
using UnityEngine;

namespace HnSF.Sample.TDAction.State
{
    public struct VarApplyTraction : IStateVariables
    {
        public int FunctionMap => (int)StateFunctionEnum.APPLY_TRACTION;
        public IConditionVariables Condition => condition;
        public IStateVariables[] Children => children;

        public Vector2[] FrameRanges
        {
            get => frameRanges;
            set => frameRanges = value;
        }
    
        [SerializeField] public Vector2[] frameRanges;
        [SelectImplementation(typeof(IConditionVariables))] [SerializeField, SerializeReference] 
        public IConditionVariables condition;

        public bool useTractionStat;
        public bool aerialTraction;
        public float traction;
        
        [SelectImplementation(typeof(IConditionVariables))] [SerializeField, SerializeReference] 
        private IStateVariables[] children;
    }
}