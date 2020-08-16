namespace CAF.Combat
{
    [System.Serializable]
    public class HitInfoBase
    {
        public HitboxForceType forceType = HitboxForceType.SET;
        public HitboxForceRelation forceRelation = HitboxForceRelation.ATTACKER;

        public virtual void DrawInspectorHitInfo()
        {

        }

        public virtual void DrawInspectorGrabInfo()
        {

        }
    }
}