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
    public class ForceSet : AttackEvent
    {
        public bool applyXForce;
        public bool applyYForce;

        public float xForce;
        public float yForce;

        public override string GetName()
        {
            return "Set Forces";
        }

        public override AttackEventReturnType Evaluate(int frame, int endFrame,
            HnSF.Fighters.IFighterBase controller, AttackEventVariables variables)
        {
            FighterManager e = (FighterManager)controller;
            FighterPhysicsManager physicsManager = (controller as FighterManager).PhysicsManager;
            Vector3 f = Vector3.zero;
            if (applyXForce)
            {
                f.x = xForce * e.FaceDirection;
            }
            if (applyYForce)
            {
                f.y = yForce;
            }

            if (applyYForce)
            {
                physicsManager.forceGravity.y = f.y;
            }
            if (applyXForce)
            {
                f.y = 0;
                physicsManager.forceMovement = f;
            }
            return AttackEventReturnType.NONE;
        }
    }
}
