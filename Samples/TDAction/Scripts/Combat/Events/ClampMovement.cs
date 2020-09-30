using UnityEngine;
using System.Collections.Generic;
using CAF.Combat;
using TDAction.Entities;
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

        public override bool Evaluate(uint frame, uint endFrame,
            CAF.Entities.EntityManager controller, AttackEventVariables variables)
        {
            EntityPhysicsManager physicsManager = (EntityPhysicsManager)controller.PhysicsManager;
            physicsManager.forceMovement.x = Mathf.Clamp(physicsManager.forceMovement.x,
                -variables.floatVars[0], variables.floatVars[0]);
            return false;
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