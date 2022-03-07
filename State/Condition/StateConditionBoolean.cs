using HnSF.Fighters;

namespace HnSF
{
    [System.Serializable]
    public class StateConditionBoolean : StateConditionBase
    {
        public bool value = true;

        public override bool IsTrue(IFighterBase fm)
        {
            return value;
        }
    }
}