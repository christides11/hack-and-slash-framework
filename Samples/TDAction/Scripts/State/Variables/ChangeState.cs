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

        public int stateMovesetID;
        public int stateID;
        
        [SerializeField, HideInInspector] private int parent;
        [SerializeField, HideInInspector] private int[] children;

        public IStateVariables Copy()
        {
            return new ChangeState()
            {
                stateMovesetID = stateMovesetID,
                stateID = stateID
            };
        }
    }
}