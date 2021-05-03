using UnityEngine;
using System.Collections.Generic;
using CAF.Combat;
using TDAction.Entities;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace TDAction.Combat.Events
{
    public class ClampGravityEvent : AttackEvent
    {
        public bool dmy;

        public override string GetName()
        {
            return "Clamp Gravity";
        }

        public override AttackEventReturnType Evaluate(int frame, int endFrame,
            CAF.Fighters.FighterBase controller, AttackEventVariables variables)
        {
            EntityPhysicsManager physicsManager = (EntityPhysicsManager)controller.PhysicsManager;
            physicsManager.forceGravity.y = Mathf.Clamp(physicsManager.forceGravity.y,
                variables.floatVars[0], variables.floatVars[1]);
            return AttackEventReturnType.NONE;
        }

#if UNITY_EDITOR
        public override void DrawEventVariables(CAF.Combat.AttackEventDefinition eventDefinition)
        {
            if (eventDefinition.variables.floatVars == null
                || eventDefinition.variables.floatVars.Count != 2)
            {
                eventDefinition.variables.floatVars = new List<float>(2);
                eventDefinition.variables.floatVars.Add(0);
                eventDefinition.variables.floatVars.Add(0);
            }

            eventDefinition.variables.floatVars[0] = EditorGUILayout.FloatField("Min", eventDefinition.variables.floatVars[0]);
            eventDefinition.variables.floatVars[1] = EditorGUILayout.FloatField("Max", eventDefinition.variables.floatVars[1]);
        }
#endif
    }
}