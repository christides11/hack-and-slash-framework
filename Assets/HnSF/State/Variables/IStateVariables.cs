using HnSF.Fighters;
using UnityEngine;

namespace HnSF
{
    public interface IStateVariables
    {
        public int FunctionMap { get; }

        public Vector2[] FrameRanges { get; set; }
        public IConditionVariables Condition { get; }
        public IStateVariables[] Children { get; }
    }
}