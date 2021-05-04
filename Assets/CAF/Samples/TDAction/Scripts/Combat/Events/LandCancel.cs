using UnityEngine;
using System.Collections.Generic;
using CAF.Combat;
using TDAction.Entities;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace TDAction.Combat.Events
{
    public class LandCancel : AttackEvent
    {
        public enum CancelType
        {
            DEFAULT = 0,
            STATE = 1,
            ATTACK = 2
        }

        public CancelType cancelType;
        public EntityStates state;

        public override string GetName()
        {
            return "Land Cancel";
        }

        public override AttackEventReturnType Evaluate(int frame, int endFrame,
            CAF.Fighters.FighterBase controller, AttackEventVariables variables)
        {
            if ((controller as TDAction.Entities.FighterManager).TryLandCancel())
            {
                return AttackEventReturnType.INTERRUPT;
            }
            return AttackEventReturnType.NONE;
        }

#if UNITY_EDITOR
        public override void DrawEventVariables(CAF.Combat.AttackEventDefinition eventDefinition)
        {
            cancelType = (CancelType)EditorGUILayout.EnumPopup("Cancel Type", (CancelType)cancelType);
            switch (cancelType)
            {
                case CancelType.ATTACK:

                    break;
                case CancelType.STATE:
                    state = (EntityStates)EditorGUILayout.EnumPopup("State", (EntityStates)state);
                    break;
            }
            /*
            if (eventDefinition.variables.floatVars == null
                || eventDefinition.variables.floatVars.Count != 1)
            {
                eventDefinition.variables.floatVars = new List<float>(1);
                eventDefinition.variables.floatVars.Add(0);
            }

            eventDefinition.variables.floatVars[0] = EditorGUILayout.FloatField("Max", eventDefinition.variables.floatVars[0]);*/
        }
#endif
    }
}