using UnityEngine;
using System.Collections.Generic;
using CAF.Combat;
using TDAction.Fighter;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace TDAction.Combat.Events
{
    public class ClampMovement : AttackEvent
    {
        public bool dmy;

        public override string GetName()
        {
            return "Clamp Movement";
        }

        public override AttackEventReturnType Evaluate(int frame, int endFrame,
            CAF.Fighters.FighterBase controller, AttackEventVariables variables)
        {
            FighterPhysicsManager physicsManager = (FighterPhysicsManager)controller.PhysicsManager;
            physicsManager.forceMovement.x = Mathf.Clamp(physicsManager.forceMovement.x,
                -variables.floatVars[0], variables.floatVars[0]);
            return AttackEventReturnType.NONE;
        }

#if UNITY_EDITOR
        public override void DrawEventVariables(CAF.Combat.AttackEventDefinition eventDefinition)
        {
            if (eventDefinition.variables.floatVars == null
                || eventDefinition.variables.floatVars.Count != 1)
            {
                eventDefinition.variables.floatVars = new List<float>(1);
                eventDefinition.variables.floatVars.Add(0);
            }

            eventDefinition.variables.floatVars[0] = EditorGUILayout.FloatField("Max", eventDefinition.variables.floatVars[0]);
        }
#endif
    }
}