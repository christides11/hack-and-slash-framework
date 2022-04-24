using HnSF.Sample.TDAction.State;

namespace HnSF.Sample.TDAction
{
    public class StateFunctionMapper : StateFunctionMapperBase
    {
        public StateFunctionMapper()
        {
            functions.Add((int)StateFunctionEnum.CHANGE_STATE, BaseStateFunctions.ChangeState);
        }
    }
}