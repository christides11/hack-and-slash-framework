using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HnSF.Combat;
using TDAction.Fighter;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace TDAction.Combat.Events
{
    public class ForceAdd : AttackEvent
    {
        public bool addXForce;
        public bool addYForce;

        public float xForce;
        public float yForce;

        public override string GetName()
        {
            return "Add Forces";
        }

        public override AttackEventReturnType Evaluate(int frame, int endFrame,
            HnSF.Fighters.FighterBase controller, AttackEventVariables variables)
        {
            FighterManager e = (FighterManager)controller;
            FighterPhysicsManager physicsManager = (FighterPhysicsManager)controller.PhysicsManager;
            Vector2 f = Vector2.zero;
            if (addXForce)
            {
                f.x = xForce * e.FaceDirection;
            }
            if (addYForce)
            {
                f.y = yForce;
                physicsManager.forceGravity.y += f.y;
            }

            if (addXForce)
            {
                f.y = 0;
                physicsManager.forceMovement += f;
            }
            return AttackEventReturnType.NONE;
        }
    }
}
