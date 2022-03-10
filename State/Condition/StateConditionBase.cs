using HnSF.Fighters;

namespace HnSF
{
    [System.Serializable]
    public class StateConditionBase
    {
        public bool inverse = false;

        public virtual bool IsTrue(IFighterBase fm)
        {
            return !inverse;
        }
    }
}