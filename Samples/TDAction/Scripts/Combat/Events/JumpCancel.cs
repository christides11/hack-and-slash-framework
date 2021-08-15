using UnityEngine;
using System.Collections.Generic;
using HnSF.Combat;
using TDAction.Fighter;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace TDAction.Combat.Events
{
    public class JumpCancel : AttackEvent
    {
        public override string GetName()
        {
            return "Jump Cancel";
        }

        public override AttackEventReturnType Evaluate(int frame, int endFrame,
            HnSF.Fighters.IFighterBase controller, AttackEventVariables variables)
        {
            if((controller as TDAction.Fighter.FighterManager).TryJump())
            {
                return AttackEventReturnType.INTERRUPT;
            }
            return AttackEventReturnType.NONE;
        }
    }
}
