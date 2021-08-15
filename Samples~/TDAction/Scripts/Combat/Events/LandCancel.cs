using UnityEngine;
using System.Collections.Generic;
using HnSF.Combat;
using TDAction.Fighter;
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
        public FighterStates state;
        public int movesetIdentifier = -1;
        public int attackIdentifier;

        public override string GetName()
        {
            return "Land Cancel";
        }

        public override AttackEventReturnType Evaluate(int frame, int endFrame,
            HnSF.Fighters.IFighterBase controller, AttackEventVariables variables)
        {
            FighterManager manager = controller as FighterManager;

            if ((controller as TDAction.Fighter.FighterManager).TryLandCancel(cancelType == CancelType.DEFAULT ? true : false))
            {
                if(cancelType == CancelType.STATE)
                {
                    manager.StateManager.ChangeState((ushort)state);
                    return AttackEventReturnType.INTERRUPT;
                }else if(cancelType == CancelType.ATTACK)
                {
                    manager.CombatManager.Cleanup();
                    (controller as TDAction.Fighter.FighterManager).TryAttack(attackIdentifier, movesetIdentifier);
                    return AttackEventReturnType.INTERRUPT_NO_CLEANUP;
                }
                return AttackEventReturnType.INTERRUPT;
            }
            return AttackEventReturnType.NONE;
        }

#if UNITY_EDITOR
        public override void DrawEventVariables(HnSF.Combat.AttackEventDefinition eventDefinition)
        {
            cancelType = (CancelType)EditorGUILayout.EnumPopup("Cancel Type", (CancelType)cancelType);
            switch (cancelType)
            {
                case CancelType.ATTACK:
                    attackIdentifier = EditorGUILayout.IntField("Attack Identifier", attackIdentifier);
                    break;
                case CancelType.STATE:
                    state = (FighterStates)EditorGUILayout.EnumPopup("State", (FighterStates)state);
                    break;
            }
        }
#endif
    }
}