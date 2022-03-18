namespace HnSF.Combat
{
    [System.Serializable]
    public class HitInfoBase
    {
        public int ID;

        public HitInfoBase()
        {

        }

        public HitInfoBase(HitInfoBase copy)
        {
            this.ID = copy.ID;
        }

        public virtual void DrawInspectorHitInfo()
        {

        }

        public virtual void DrawInspectorGrabInfo()
        {

        }
    }
}