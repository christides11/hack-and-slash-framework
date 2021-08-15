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
            HnSF.Fighters.IFighterBase controller, AttackEventVariables variables)
        {
            if(lastFrameExecution && frame != endFrame)
            {
                return AttackEventReturnType.NONE;
            }
            FighterManager e = (FighterManager)controller;
            FighterCombatManager combatManager = (controller as FighterManager).CombatManager;

            e.TryAttack(attackID, -1, resetFrameCounter);
            return AttackEventReturnType.STALL;
        }
    }
}
