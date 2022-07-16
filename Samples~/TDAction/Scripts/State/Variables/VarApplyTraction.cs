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
        public int Parent
        {
            get => parent;
            set => parent = value;
        }

        public int[] Children
        {
            get => children;
            set => children = value;
        }
        public Vector2[] FrameRanges
        {
            get => frameRanges;
            set => frameRanges = value;
        }
    
        [SerializeField, HideInInspector] private int id;
        [SerializeField] public Vector2[] frameRanges;
        [SelectImplementation(typeof(IConditionVariables))] [SerializeField, SerializeReference] 
        public IConditionVariables condition;

        public bool useTractionStat;
        public bool aerialTraction;
        public float traction;
        
        [SerializeField, HideInInspector] private int parent;
        [SerializeField, HideInInspector] private int[] children;
    }
}