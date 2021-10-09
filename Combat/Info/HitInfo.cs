#if UNITY_EDITOR
using UnityEditor;
#endif
using System;
using UnityEngine;

namespace HnSF.Combat
{
    [System.Serializable]
    public class HitInfo : HitInfoBase
    {
        public bool airOnly;
        public bool groundOnly;
        public bool causesTumble;
        public bool knockdown;
        public bool groundBounces;
        public float groundBounceForce;
        public bool wallBounces;
        public float wallBounceForce;

        public float damageOnHit;

        // Set ForceType.
        public Vector3 opponentForce = Vector3.up;
        // Push/Pull ForceType.
        public bool forceIncludeYForce = false;
        public float opponentForceMultiplier = 1;
        public float opponentMaxMagnitude = 1;
        public float opponentMinMagnitude = 1;
        public Vector3 pushPullCenterOffset;

        public AttackDefinition throwConfirm;

        public HitInfo()
        {

        }

        public HitInfo(HitInfoBase other): base(other)
        {
            if(other.GetType() != typeof(HitInfo)
                && other.GetType().IsSubclassOf(typeof(HitInfo)) == false)
            {
                return;
            }
            HitInfo otherHitInfo = (HitInfo)other;
            knockdown = otherHitInfo.knockdown;

            groundBounces = otherHitInfo.groundBounces;
            groundBounceForce = otherHitInfo.groundBounceForce;
            wallBounces = otherHitInfo.wallBounces;
            wallBounceForce = otherHitInfo.wallBounceForce;

            damageOnHit = otherHitInfo.damageOnHit;
            causesTumble = otherHitInfo.causesTumble;

            opponentForce = otherHitInfo.opponentForce;

            forceIncludeYForce = otherHitInfo.forceIncludeYForce;
            opponentForceMultiplier = otherHitInfo.opponentForceMultiplier;
            opponentMinMagnitude = otherHitInfo.opponentMinMagnitude;
            opponentMaxMagnitude = otherHitInfo.opponentMaxMagnitude;
            pushPullCenterOffset = otherHitInfo.pushPullCenterOffset;
        }
    }
}