namespace HnSF.Combat
{
    [System.Serializable]
    public class HitInfoBase
    {
        // General
        public HitboxType hitType;
        public bool hitKills = true;
        public bool continuousHit;
        public int spaceBetweenHits;

        // Forces
        public bool opponentResetXForce = true;
        public bool opponentResetYForce = true;
        public bool autoLink = false;
        public HitboxForceType forceType = HitboxForceType.SET;
        public HitboxForceRelation forceRelation = HitboxForceRelation.ATTACKER;

        // Stun
        public ushort attackerHitstop;
        public ushort hitstop;
        public ushort hitstun;

        public HitInfoBase()
        {

        }

        public HitInfoBase(HitInfoBase copy)
        {
            this.hitType = copy.hitType;
            this.hitKills = copy.hitKills;
            this.continuousHit = copy.continuousHit;
            this.spaceBetweenHits = copy.spaceBetweenHits;

            this.opponentResetXForce = copy.opponentResetXForce;
            this.opponentResetYForce = copy.opponentResetYForce;
            this.autoLink = copy.autoLink;
            this.forceType = copy.forceType;
            this.forceRelation = copy.forceRelation;

            this.attackerHitstop = copy.attackerHitstop;
            this.hitstop = copy.hitstop;
            this.hitstun = copy.hitstun;
        }

        public virtual void DrawInspectorHitInfo()
        {

        }

        public virtual void DrawInspectorGrabInfo()
        {

        }
    }
}