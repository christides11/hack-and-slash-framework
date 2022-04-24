using UnityEngine;

public interface IStateVariables
{
    public int FunctionMap { get; }
    public Vector2[] FrameRanges { get; set; }
    //public IConditionVar Condition { get; }
}