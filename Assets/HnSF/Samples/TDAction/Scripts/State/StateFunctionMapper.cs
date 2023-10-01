using HnSF.Sample.TDAction.State;

namespace HnSF.Sample.TDAction
{
    public class StateFunctionMapper : StateFunctionMapperBase
    {
        public StateFunctionMapper()
        {
            functions.Add(typeof(ChangeState), BaseStateFunctions.ChangeState);
            functions.Add(typeof(VarApplyGravity), BaseStateFunctions.ApplyGravity);
            functions.Add(typeof(VarApplyTraction), BaseStateFunctions.ApplyTraction);
            functions.Add(typeof(VarSetFallSpeed), BaseStateFunctions.SetFallSpeed);
        }
    }
}