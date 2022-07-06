using UnityEngine;

namespace HnSF.Sample.TDAction.State
{
    public struct VarSetFallSpeed : IStateVariables
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
        public int FunctionMap => (int)StateFunctionEnum.SET_FALL_SPEED;
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

        public float value;
        
        public int parent;
        public int[] children;
    }
}