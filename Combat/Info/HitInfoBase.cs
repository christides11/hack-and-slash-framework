namespace HnSF.Combat
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

        public HitInfoBase()
        {

        }

        public HitInfoBase(HitInfoBase copy)
        {
            this.airOnly = copy.airOnly;
            this.groundOnly = copy.groundOnly;
            this.hitKills = copy.hitKills;
            this.continuousHit = copy.continuousHit;
            this.spaceBetweenHits = copy.spaceBetweenHits;

            this.opponentResetXForce = copy.opponentResetXForce;
            this.opponentResetYForce = copy.opponentResetYForce;

            this.attackerHitstop = copy.attackerHitstop;
            this.hitstop = copy.hitstop;
            this.hitstun = copy.hitstun;

            this.forceType = copy.forceType;
            this.forceRelation = copy.forceRelation;
        }

        public virtual void DrawInspectorHitInfo()
        {

        }

        public virtual void DrawInspectorGrabInfo()
        {

        }
    }
}