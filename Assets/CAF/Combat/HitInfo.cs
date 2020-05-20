using UnityEngine;

namespace CAF.Combat
{
    [System.Serializable]
    public class HitInfo
    {
        public bool airOnly;
        public bool groundOnly;

        public bool continuousHit;
        public int spaceBetweenHits;
        public bool breakArmor;
        public bool unblockable;
        public bool knockdown;
        public bool groundBounces;
        public float groundBounceForce;
        public bool wallBounces;
        public float wallBounceForce;
        public float damageOnHit;
        public float damageOnBlock;
        public bool hitKills = true;
        public ushort attackerHitstop;
        public ushort hitstop;
        public ushort hitstun;

        public bool opponentResetXForce = true;
        public bool opponentResetYForce = true;
        public HitboxForceType forceType = HitboxForceType.SET;
        public HitboxForceRelation forceRelation = HitboxForceRelation.ATTACKER;
        public bool causesTumble;
        public float opponentForceMagnitude = 1;
        // Set ForceType.
        public Vector3 opponentForceDir = Vector3.forward;
        // Push/Pull ForceType.
        public bool forceIncludeYForce = false;
        public float opponentMaxMagnitude = 1;
        public float opponentMinMagnitude = 1;

        public HitInfo()
        {

        }

        public HitInfo(HitInfo other)
        {
            airOnly = other.airOnly;
            groundOnly = other.groundOnly;

            continuousHit = other.continuousHit;
            spaceBetweenHits = other.spaceBetweenHits;
            breakArmor = other.breakArmor;
            unblockable = other.unblockable;
            knockdown = other.knockdown;

            groundBounces = other.groundBounces;
            groundBounceForce = other.groundBounceForce;
            wallBounces = other.wallBounces;
            wallBounceForce = other.wallBounceForce;

            damageOnHit = other.damageOnHit;
            damageOnBlock = other.damageOnBlock;
            hitKills = other.hitKills;
            attackerHitstop = other.attackerHitstop;
            hitstop = other.hitstop;
            hitstun = other.hitstun;

            opponentResetXForce = other.opponentResetXForce;
            opponentResetYForce = other.opponentResetYForce;
            forceType = other.forceType;
            forceRelation = other.forceRelation;
            causesTumble = other.causesTumble;

            opponentForceDir = other.opponentForceDir;
            opponentForceMagnitude = other.opponentForceMagnitude;

            forceIncludeYForce = other.forceIncludeYForce;
            opponentMaxMagnitude = other.opponentMaxMagnitude;
        }
    }
}