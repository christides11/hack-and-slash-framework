using HnSF.Sample.TDAction.State;

namespace HnSF.Sample.TDAction
{
    public class StateFunctionMapper : StateFunctionMapperBase
    {
        public StateFunctionMapper()
        {
            functions.Add(typeof(State.ChangeState), BaseStateFunctions.ChangeState);
            functions.Add(typeof(State.VarApplyGravity), BaseStateFunctions.ApplyGravity);
            functions.Add(typeof(State.VarApplyTraction), BaseStateFunctions.ApplyTraction);
            functions.Add(typeof(State.VarSetFallSpeed), BaseStateFunctions.SetFallSpeed);
        }
    }
}