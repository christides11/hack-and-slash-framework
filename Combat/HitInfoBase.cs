namespace CAF.Combat
{
    [System.Serializable]
    public class HitInfoBase
    {
        // General
        public bool airOnly;
        public bool groundOnly;
        public bool hitKills = true;
        public bool continuousHit;
        public int spaceBetweenHits;

        // Forces
        public bool opponentResetXForce = true;
        public bool opponentResetYForce = true;

        public ushort attackerHitstop;
        public ushort hitstop;
        public ushort hitstun;

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