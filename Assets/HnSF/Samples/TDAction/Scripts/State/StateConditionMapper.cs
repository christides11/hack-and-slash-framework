namespace HnSF.Sample.TDAction.State
{
    public class StateConditionMapper : StateConditionMapperBase
    {
        public StateConditionMapper()
        {
            functions.Add((int)ConditionFunctionEnum.NONE, BaseConditionFunctions.NoCondition);
            functions.Add((int)ConditionFunctionEnum.MOVEMENT_MAGNITUDE, BaseConditionFunctions.MovementSqrMagnitude);
        }
    }
}