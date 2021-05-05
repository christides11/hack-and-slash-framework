using UnityEngine;
using System.Collections.Generic;
using CAF.Combat;
using TDAction.Fighter;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace TDAction.Combat.Events
{
    public class ApplyGravity : AttackEvent
    {
        public bool useEntityMaxFallSpeed;
        public bool useEntityGravity;
        public bool useEntityGravityScale;

        public override string GetName()
        {
            return "Apply Gravity";
        }

        public override AttackEventReturnType Evaluate(int frame, int endFrame,
            CAF.Fighters.FighterBase controller, AttackEventVariables variables)
        {
            FighterStatsManager statsManager = (controller as FighterManager).statManager;
            FighterPhysicsManager physicsManager = (FighterPhysicsManager)controller.PhysicsManager;
            if (controller.IsGrounded)
            {
                physicsManager.forceGravity = Vector3.zero;
                return AttackEventReturnType.NONE;
            }
            float percent = (float)frame / (float)endFrame;

            float gravity = statsManager.CurrentStats.gravity.GetCurrentValue();
            if (!useEntityGravity)
            {
                gravity = variables.curveVars[0].Evaluate(percent)
                    * variables.floatVars[0];
            }

            float gravityScale = physicsManager.GravityScale;
            if (!useEntityGravityScale)
            {
                gravityScale = variables.curveVars[1].Evaluate(percent)
                    * variables.floatVars[1];
            }

            float maxFallSpeed = statsManager.CurrentStats.maxFallSpeed.GetCurrentValue();
            if (!useEntityMaxFallSpeed)
            {
                maxFallSpeed = variables.curveVars[2].Evaluate(percent)
                    * variables.floatVars[2];
            }

            physicsManager.HandleGravity(maxFallSpeed, gravity, gravityScale);
            return AttackEventReturnType.NONE;
        }

#if UNITY_EDITOR
        public override void DrawEventVariables(CAF.Combat.AttackEventDefinition eventDefinition)
        {
            if (eventDefinition.variables.floatVars == null
                || eventDefinition.variables.floatVars.Count != 3)
            {
                eventDefinition.variables.floatVars = new List<float>(3);
                eventDefinition.variables.floatVars.Add(0);
                eventDefinition.variables.floatVars.Add(0);
                eventDefinition.variables.floatVars.Add(0);
            }
            if (eventDefinition.variables.curveVars == null
                || eventDefinition.variables.curveVars.Count != 3)
            {
                eventDefinition.variables.curveVars = new List<AnimationCurve>(3);
                eventDefinition.variables.curveVars.Add(new AnimationCurve());
                eventDefinition.variables.curveVars.Add(new AnimationCurve());
                eventDefinition.variables.curveVars.Add(new AnimationCurve());
            }

            useEntityGravity = EditorGUILayout.Toggle("Use Entity Gravity?", useEntityGravity);
            useEntityGravityScale = EditorGUILayout.Toggle("Use Entity Gravity Scale?", useEntityGravityScale);
            useEntityMaxFallSpeed = EditorGUILayout.Toggle("Use Entity Max Fall Speed?", useEntityMaxFallSpeed);

            EditorGUILayout.Space();

            if (!useEntityGravity)
            {
                eventDefinition.variables.floatVars[0] = EditorGUILayout.FloatField("Gravity Curve Multiplier",
                    eventDefinition.variables.floatVars[0]);
                eventDefinition.variables.curveVars[0] = EditorGUILayout.CurveField("Gravity Curve",
                    eventDefinition.variables.curveVars[0]);
            }
            if (!useEntityGravityScale)
            {
                eventDefinition.variables.floatVars[1] = EditorGUILayout.FloatField("Gravity Scale Curve Multiplier",
                    eventDefinition.variables.floatVars[1]);
                eventDefinition.variables.curveVars[1] = EditorGUILayout.CurveField("Gravity Scale Curve",
                    eventDefinition.variables.curveVars[1]);
            }
            if (!useEntityMaxFallSpeed)
            {
                eventDefinition.variables.floatVars[2] = EditorGUILayout.FloatField("Max Fall Speed Multiplier",
                    eventDefinition.variables.floatVars[2]);
                eventDefinition.variables.curveVars[2] = EditorGUILayout.CurveField("Max Fall Speed Curve",
                    eventDefinition.variables.curveVars[2]);
            }
        }
#endif
    }
}