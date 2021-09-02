using UnityEngine;
using System.Collections.Generic;
using HnSF.Combat;
using TDAction.Fighter;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace TDAction.Combat.Events
{
    public class ClampMovement : AttackEvent
    {
        public float max;

        public override string GetName()
        {
            return "Clamp Movement";
        }

        public override AttackEventReturnType Evaluate(int frame, int endFrame,
            HnSF.Fighters.IFighterBase controller)
        {
            FighterPhysicsManager physicsManager = (controller as FighterManager).PhysicsManager;
            physicsManager.forceMovement.x = Mathf.Clamp(physicsManager.forceMovement.x,
                -max, max);
            return AttackEventReturnType.NONE;
        }
    }
}