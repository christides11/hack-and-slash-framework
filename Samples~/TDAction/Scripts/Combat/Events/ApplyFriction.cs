using System.Collections.Generic;
using UnityEngine;
using HnSF.Combat;
using TDAction.Fighter;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace TDAction.Combat.Events
{
    public class ApplyFriction : AttackEvent
    {
        public bool useXFriction;
        public bool useYFriction;

        public float xFriction;
        public float yFriction;

        public override string GetName()
        {
            return "Friction";
        }

        public override AttackEventReturnType Evaluate(int frame, int endFrame,
            HnSF.Fighters.IFighterBase controller)
        {
            FighterPhysicsManager physicsManager = (controller as FighterManager).PhysicsManager;
            if (useXFriction)
            {
                physicsManager.ApplyMovementFriction(xFriction);
            }
            if (useYFriction)
            {
                physicsManager.ApplyGravityFriction(yFriction);
            }
            return AttackEventReturnType.NONE;
        }
    }
}