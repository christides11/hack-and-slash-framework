namespace HnSF.Sample.TDAction.State
{
    public class StateConditionMapper : StateConditionMapperBase
    {
        public StateConditionMapper()
        {
            RegisterCondition(typeof(ConditionNone), BaseConditionFunctions.NoCondition);
            RegisterCondition(typeof(ConditionMovementMagnitude), BaseConditionFunctions.MovementSqrMagnitude);
            RegisterCondition(typeof(ConditionGroundState), BaseConditionFunctions.GroundedState);
            RegisterCondition(typeof(ConditionFallSpeed), BaseConditionFunctions.FallSpeed);
        }
    }
}