using UnityEngine;
using System.Collections.Generic;
using HnSF.Combat;
using TDAction.Fighter;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace TDAction.Combat.Events
{
    public class ClampGravityEvent : AttackEvent
    {
        public float minClamp = 0;
        public float maxClamp = 0;

        public override string GetName()
        {
            return "Clamp Gravity";
        }

        public override AttackEventReturnType Evaluate(int frame, int endFrame,
            HnSF.Fighters.IFighterBase controller, AttackEventVariables variables)
        {
            FighterPhysicsManager physicsManager = (controller as FighterManager).PhysicsManager;
            physicsManager.forceGravity.y = Mathf.Clamp(physicsManager.forceGravity.y,
                minClamp, maxClamp);
            return AttackEventReturnType.NONE;
        }
    }
}