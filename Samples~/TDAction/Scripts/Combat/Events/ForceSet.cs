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
    public class ForceSet : AttackEvent
    {
        public bool xForce;
        public bool yForce;

        public override string GetName()
        {
            return "Set Forces";
        }

        public override bool Evaluate(uint frame, uint endFrame,
            CAF.Fighters.FighterBase controller, AttackEventVariables variables)
        {
            FighterManager e = (FighterManager)controller;
            EntityPhysicsManager physicsManager = (EntityPhysicsManager)controller.PhysicsManager;
            Vector3 f = Vector3.zero;
            if (xForce)
            {
                f.x = variables.floatVars[0] * e.FaceDirection;
            }
            if (yForce)
            {
                f.y = variables.floatVars[1];
            }

            if (yForce)
            {
                physicsManager.forceGravity.y = f.y;
            }
            if (xForce)
            {
                f.y = 0;
                physicsManager.forceMovement = f;
            }
            return false;
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

            xForce = EditorGUILayout.Toggle("X Force", xForce);
            yForce = EditorGUILayout.Toggle("Y Force", yForce);

            if (xForce)
            {
                eventDefinition.variables.floatVars[0] = EditorGUILayout.FloatField("X Force",
                    eventDefinition.variables.floatVars[0]);
            }
            if (yForce)
            {
                eventDefinition.variables.floatVars[1] = EditorGUILayout.FloatField("Y Force",
                    eventDefinition.variables.floatVars[1]);
            }
        }
#endif
    }
}
