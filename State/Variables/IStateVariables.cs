using HnSF.Fighters;
using UnityEngine;

namespace HnSF
{
    public interface IStateVariables
    {
        public string Name { get; set; }
        public int ID { get; set; }
        public int FunctionMap { get; }

        public Vector2[] FrameRanges { get; set; }
        public IConditionVariables Condition { get; }
        public int Parent { get; }
        public int[] Children { get; }
    }
}