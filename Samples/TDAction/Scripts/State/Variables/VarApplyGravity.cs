using System;
using HnSF.Fighters;
using UnityEngine;
using UnityEngine.Rendering;

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
        [SelectImplementation(typeof(IConditionVariables))] [SerializeField, SerializeReference] 
        public IConditionVariables condition;

        public bool RunDuringHitstop { get => runDuringHitstop; set => runDuringHitstop = value; }
        public bool runDuringHitstop;

        public bool useMaxFallSpeedStat;
        public bool useGravityStat;
        public float maxFallSpeed;
        public float gravity;
        
        [SerializeField, HideInInspector] private int parent;
        [SerializeField, HideInInspector] private int[] children;

        public void SetupDefaults()
        {
            
        }

        public IStateVariables Copy()
        {
            var copy = new VarApplyGravity();
            copy.frameRanges = frameRanges;
            copy.condition = condition?.Copy();
            copy.useMaxFallSpeedStat = useMaxFallSpeedStat;
            copy.useGravityStat = useGravityStat;
            copy.gravity = gravity;
            copy.maxFallSpeed = maxFallSpeed;
            return copy;
        }
    }
}