using System;
using HnSF.Fighters;
using UnityEngine;

namespace HnSF.Sample.TDAction.State
{
    [StateVariable("Gravity/Apply Gravity")]
    public struct VarApplyGravity : IStateVariables
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
        public int FunctionMap => (int)StateFunctionEnum.APPLY_GRAVITY;
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

        public bool useMaxFallSpeedStat;
        public bool useGravityStat;
        public float maxFallSpeed;
        public float gravity;
        
        public int parent;
        public int[] children;
    }
}