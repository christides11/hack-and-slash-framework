using UnityEngine;

namespace HnSF.Sample.TDAction.State
{
    public class VarSetFallSpeed : IStateVariables
    {
        public int FunctionMap => (int)StateFunctionEnum.SET_FALL_SPEED;
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

        public float value;
        
        [SelectImplementation(typeof(IConditionVariables))] [SerializeField, SerializeReference] 
        private IStateVariables[] children;
    }
}