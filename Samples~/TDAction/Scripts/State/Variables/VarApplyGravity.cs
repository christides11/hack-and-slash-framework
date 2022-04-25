using UnityEngine;

namespace HnSF.Sample.TDAction.State
{
    public struct VarApplyGravity : IStateVariables
    {
        public int FunctionMap => (int)StateFunctionEnum.APPLY_GRAVITY;
        public IConditionVariables Condition => condition;
        public Vector2[] FrameRanges
        {
            get => frameRanges;
            set => frameRanges = value;
        }
    
        [SerializeField] public Vector2[] frameRanges;
        [SelectImplementation(typeof(IConditionVariables))] [SerializeField, SerializeReference] 
        public IConditionVariables condition;

        public bool useMaxFallSpeedStat;
        public bool useGravityStat;
        public float maxFallSpeed;
        public float gravity;
    }
}