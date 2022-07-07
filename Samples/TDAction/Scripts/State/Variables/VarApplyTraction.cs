using System;
using HnSF.Fighters;
using UnityEngine;

namespace HnSF.Sample.TDAction.State
{
    [StateVariable("Traction/Apply Traction")]
    public struct VarApplyTraction : IStateVariables
    {
        public string name;
        public string Name
        {
            get => name;
            set => name = value;
        }
        public int ID
        {
            get => id;
            set => id = value;
        }
        public int FunctionMap => (int)StateFunctionEnum.APPLY_TRACTION;
        public IConditionVariables Condition => condition;
        public int Parent => Parent;
        public int[] Children => children;

        public Vector2[] FrameRanges
        {
            get => frameRanges;
            set => frameRanges = value;
        }
    
        public int id;
        [SerializeField] public Vector2[] frameRanges;
        [SelectImplementation(typeof(IConditionVariables))] [SerializeField, SerializeReference] 
        public IConditionVariables condition;

        public bool useTractionStat;
        public bool aerialTraction;
        public float traction;
        
        public int parent;
        public int[] children;
    }
}