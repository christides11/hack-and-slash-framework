namespace HnSF.Sample.TDAction.State
{
    public class StateConditionMapper : StateConditionMapperBase
    {
        public StateConditionMapper()
        {
            functions.Add(typeof(ConditionNone), BaseConditionFunctions.NoCondition);
            functions.Add(typeof(ConditionMovementMagnitude), BaseConditionFunctions.MovementSqrMagnitude);
            functions.Add(typeof(ConditionGroundState), BaseConditionFunctions.GroundedState);
            functions.Add(typeof(ConditionFallSpeed), BaseConditionFunctions.FallSpeed);
        }
    }
}