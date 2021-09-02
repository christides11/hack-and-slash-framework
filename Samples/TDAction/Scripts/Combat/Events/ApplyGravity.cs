using UnityEngine;
using System.Collections.Generic;
using HnSF.Combat;
using TDAction.Fighter;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace TDAction.Combat.Events
{
    public class ApplyGravity : AttackEvent
    {
        public bool useEntityMaxFallSpeed;
        public bool useEntityGravity;
        public bool useEntityGravityScale;

        public float gravityCurveMultiplier = 1;
        public AnimationCurve gravityCurve = new AnimationCurve();

        public float gravityScaleCurveMultiplier = 1;
        public AnimationCurve gravityScaleCurve = new AnimationCurve();

        public float maxFallSpeedCurveMultiplier = 1;
        public AnimationCurve maxFallSpeedCurve = new AnimationCurve();

        public override string GetName()
        {
            return "Apply Gravity";
        }

        public override AttackEventReturnType Evaluate(int frame, int endFrame,
            HnSF.Fighters.IFighterBase controller)
        {
            FighterStatsManager statsManager = (controller as FighterManager).statManager;
            FighterPhysicsManager physicsManager = (controller as FighterManager).PhysicsManager;
            if (physicsManager.IsGrounded)
            {
                physicsManager.forceGravity = Vector3.zero;
                return AttackEventReturnType.NONE;
            }
            float percent = (float)frame / (float)endFrame;

            float gravity = statsManager.CurrentStats.gravity.GetCurrentValue();
            if (!useEntityGravity)
            {
                gravity = gravityCurve.Evaluate(percent)
                    * gravityCurveMultiplier;
            }

            float gravityScale = physicsManager.GravityScale;
            if (!useEntityGravityScale)
            {
                gravityScale = gravityScaleCurve.Evaluate(percent)
                    * gravityScaleCurveMultiplier;
            }

            float maxFallSpeed = statsManager.CurrentStats.maxFallSpeed.GetCurrentValue();
            if (!useEntityMaxFallSpeed)
            {
                maxFallSpeed = maxFallSpeedCurve.Evaluate(percent)
                    * maxFallSpeedCurveMultiplier;
            }

            physicsManager.HandleGravity(maxFallSpeed, gravity, gravityScale);
            return AttackEventReturnType.NONE;
        }
    }
}