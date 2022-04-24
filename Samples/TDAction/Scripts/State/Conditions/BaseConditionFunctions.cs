using HnSF.Fighters;

namespace HnSF.Sample.TDAction
{
    public class BaseConditionFunctions
    {
        public static bool NoCondition(IFighterBase fighter, IConditionVariables variables)
        {
            return true;
        }
    }
}