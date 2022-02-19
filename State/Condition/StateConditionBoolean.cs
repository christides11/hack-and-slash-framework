using HnSF.Fighters;
using Juce.ImplementationSelector;

namespace HnSF
{
    [System.Serializable]
    [SelectImplementationCustomDisplayName("Menu/boolean")]
    public class StateConditionBoolean : StateConditionBase
    {
        public bool value = true;

        public override bool IsTrue(IFighterBase fm)
        {
            return value;
        }
    }
}