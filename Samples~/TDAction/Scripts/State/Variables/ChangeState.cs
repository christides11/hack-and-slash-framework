using UnityEngine;

namespace HnSF.Sample.TDAction.State
{
    public struct ChangeState : IStateVariables
    {
        public int FunctionMap => (int)StateFunctionEnum.CHANGE_STATE;
        public IConditionVariables Condition => condition;
        public Vector2[] FrameRanges
        {
            get => frameRanges;
            set => frameRanges = value;
        }
    
        [SerializeField] public Vector2[] frameRanges;
        [SelectImplementation(typeof(IConditionVariables))] [SerializeField, SerializeReference] 
        public IConditionVariables condition;

        public int stateMovesetID;
        public int stateID;
    }
}