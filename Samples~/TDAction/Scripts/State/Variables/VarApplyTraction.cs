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
        public IConditionVariables Condition { get => condition; set => condition = value; }
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
        public Vector2Int[] FrameRanges
        {
            get => frameRanges;
            set => frameRanges = value;
        }
    
        [SerializeField, HideInInspector] private int id;

        [SerializeField] public Vector2Int[] frameRanges;
        [SubclassSelector] [SerializeField, SerializeReference] 
        public IConditionVariables condition;

        public bool RunDuringHitstop { get => runDuringHitstop; set => runDuringHitstop = value; }
        public bool runDuringHitstop;

        public bool useTractionStat;
        public bool aerialTraction;
        public float traction;
        
        [SerializeField, HideInInspector] private int parent;
        [SerializeField, HideInInspector] private int[] children;

        public void SetupDefaults()
        {
            traction = 5.0f;
        }

        public IStateVariables Copy()
        {
            return new VarApplyTraction()
            {
                useTractionStat = useTractionStat,
                aerialTraction = aerialTraction,
                traction = traction,
            };
        }
    }
}