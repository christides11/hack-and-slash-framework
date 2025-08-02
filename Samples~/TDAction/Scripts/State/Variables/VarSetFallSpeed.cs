using UnityEngine;

namespace HnSF.Sample.TDAction.State
{
    [StateVariable("Gravity/Set Fall Speed")]
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

        public float value;
        
        [SerializeField, HideInInspector] private int parent;
        [SerializeField, HideInInspector] private int[] children;

        public void SetupDefaults()
        {
            
        }

        public IStateVariables Copy()
        {
            return new VarSetFallSpeed()
            {
                value = value
            };
        }
    }
}