using HnSF.Fighters;
using UnityEngine;

namespace HnSF
{
    public interface IStateVariables
    {
        public string Name { get; set; }
        public int ID { get; set; }

        public Vector2Int[] FrameRanges { get; set; }
        public IConditionVariables Condition { get; }
        public int Parent { get; set; }
        public int[] Children { get; set; }

        IStateVariables Copy();
    }
}