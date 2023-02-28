namespace HnSF
{
    [System.Serializable]
    public class FighterStateReferenceBase
    {
        public virtual int GetState()
        {
            return 0;
        }

        public virtual FighterStateReferenceBase Copy()
        {
            return new FighterStateReferenceBase();
        }
    }
}