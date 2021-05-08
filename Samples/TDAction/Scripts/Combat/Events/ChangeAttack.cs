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
    public class ChangeAttack : AttackEvent
    {
        public bool resetFrameCounter = true;
        public bool lastFrameExecution = false;

        public int attackID = -1;

        public override string GetName()
        {
            return "Change Attack";
        }

        public override AttackEventReturnType Evaluate(int frame, int endFrame,
            HnSF.Fighters.FighterBase controller, AttackEventVariables variables)
        {
            if(lastFrameExecution && frame != endFrame)
            {
                return AttackEventReturnType.NONE;
            }
            FighterManager e = (FighterManager)controller;
            FighterCombatManager combatManager = (FighterCombatManager)controller.CombatManager;

            e.TryAttack(attackID, resetFrameCounter);
            return AttackEventReturnType.STALL;
        }
    }
}
