﻿using UnityEngine;
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
            HnSF.Fighters.FighterBase controller, AttackEventVariables variables)
        {
            FighterPhysicsManager physicsManager = (FighterPhysicsManager)controller.PhysicsManager;
            physicsManager.forceMovement.x = Mathf.Clamp(physicsManager.forceMovement.x,
                -max, max);
            return AttackEventReturnType.NONE;
        }
    }
}