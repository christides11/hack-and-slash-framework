using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CAF.Combat;
using TDAction.Entities;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace TDAction.Combat.Events
{
    public class ChangeAttack : AttackEvent
    {
        public bool resetCurrentFrame = true;
        public bool lastFrameExecution = false;

        public override string GetName()
        {
            return "Change Attack";
        }

        public override AttackEventReturnType Evaluate(int frame, int endFrame,
            CAF.Fighters.FighterBase controller, AttackEventVariables variables)
        {
            if(lastFrameExecution && frame != endFrame)
            {
                return AttackEventReturnType.NONE;
            }
            FighterManager e = (FighterManager)controller;
            EntityCombatManager combatManager = (EntityCombatManager)controller.CombatManager;

            e.TryAttack(variables.intVars[0]);
            return AttackEventReturnType.STALL;
        }

#if UNITY_EDITOR
        public override void DrawEventVariables(CAF.Combat.AttackEventDefinition eventDefinition)
        {
            if (eventDefinition.variables.intVars== null
                || eventDefinition.variables.intVars.Count != 1)
            {
                eventDefinition.variables.intVars = new List<int>(1);
                eventDefinition.variables.intVars.Add(-1);
            }

            resetCurrentFrame = EditorGUILayout.Toggle("Reset Current Frame Counter", resetCurrentFrame);

            eventDefinition.variables.intVars[0] = EditorGUILayout.IntField("Attack", eventDefinition.variables.intVars[0]);

            lastFrameExecution = EditorGUILayout.Toggle("Execute on Last Frame", lastFrameExecution);
        }
#endif
    }
}
