namespace HnSF.Sample.TDAction.State
{
    public class StateConditionMapper : StateConditionMapperBase
    {
        public StateConditionMapper()
        {
            functions.Add((int)ConditionFunctionEnum.NONE, BaseConditionFunctions.NoCondition);
            functions.Add((int)ConditionFunctionEnum.MOVEMENT_MAGNITUDE, BaseConditionFunctions.MovementSqrMagnitude);
            functions.Add((int)ConditionFunctionEnum.GROUND_STATE, BaseConditionFunctions.GroundedState);
            functions.Add((int)ConditionFunctionEnum.FALL_SPEED, BaseConditionFunctions.FallSpeed);
        }
    }
}