using HnSF.Sample.TDAction.State;

namespace HnSF.Sample.TDAction
{
    public class StateFunctionMapper : StateFunctionMapperBase
    {
        public StateFunctionMapper()
        {
            functions.Add((int)StateFunctionEnum.CHANGE_STATE, BaseStateFunctions.ChangeState);
            functions.Add((int)StateFunctionEnum.APPLY_GRAVITY, BaseStateFunctions.ApplyGravity);
            functions.Add((int)StateFunctionEnum.APPLY_TRACTION, BaseStateFunctions.ApplyTraction);
            functions.Add((int)StateFunctionEnum.SET_FALL_SPEED, BaseStateFunctions.SetFallSpeed);
        }
    }
}