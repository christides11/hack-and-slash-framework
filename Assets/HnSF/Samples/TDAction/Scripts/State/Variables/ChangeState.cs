using UnityEngine;

namespace HnSF.Sample.TDAction.State
{
    [StateVariable("State/Change State")]
    public struct ChangeState : IStateVariables
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
        public int FunctionMap => (int)StateFunctionEnum.CHANGE_STATE;
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

        public int stateMovesetID;
        public int stateID;
        
        public int parent;
        public int[] children;
    }
}