namespace HnSF.Sample.TDAction.State
{
    public class StateConditionMapper : StateConditionMapperBase
    {
        public StateConditionMapper()
        {
            functions.Add((int)ConditionFunctionEnum.NONE, BaseConditionFunctions.NoCondition);
        }
    }
}