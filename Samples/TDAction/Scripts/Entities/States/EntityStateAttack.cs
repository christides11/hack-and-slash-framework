using System;
using System.Collections;
using System.Collections.Generic;
using TDAction.Combat;
using UnityEngine;

namespace TDAction.Entities.States
{
    public class EntityStateAttack : EntityState
    {
        public override string GetName()
        {
            return $"Attack ({GetEntityController().CombatManager.CurrentAttack?.name}).";
        }

        public override void Initialize()
        {
            base.Initialize();
            AttackDefinition currentAttack = 
                (TDAction.Combat.AttackDefinition)GetEntityController().CombatManager.CurrentAttack.attackDefinition;
            if (currentAttack.stateOverride > -1)
            {
                GetEntityController().StateManager.ChangeState(currentAttack.stateOverride);
                return;
            }
        }

        public override void OnUpdate()
        {
            EntityManager entityManager = GetEntityController();

            AttackDefinition currentAttack =
                (TDAction.Combat.AttackDefinition)entityManager.CombatManager.CurrentAttack.attackDefinition;

            for (int i = 0; i < currentAttack.boxGroups.Count; i++)
            {
                HandleBoxGroup(i, currentAttack.boxGroups[i]);
            }

            if (CheckCancelWindows(currentAttack))
            {
                entityManager.CombatManager.Cleanup();
                return;
            }

            CheckInterrupt();
        }

        /// <summary>
        /// Handles the lifetime process of hitboxes and detectboxes.
        /// </summary>
        /// <param name="groupIndex">The group number being processed.</param>
        /// <param name="boxGroup">The group being processed.</param>
        protected virtual void HandleBoxGroup(int groupIndex, CAF.Combat.BoxGroup boxGroup)
        {
            EntityManager entityManager = GetEntityController();
            if(entityManager.StateManager.CurrentStateFrame == boxGroup.activeFramesEnd + 1)
            {
                entityManager.CombatManager.hitboxManager.DeactivateHitboxGroup(groupIndex);
            }

            if(entityManager.StateManager.CurrentStateFrame < boxGroup.activeFramesStart
                || entityManager.StateManager.CurrentStateFrame > boxGroup.activeFramesEnd)
            {
                return;
            }

            switch (boxGroup.hitGroupType)
            {
                case CAF.Combat.BoxGroupType.HIT:
                    entityManager.CombatManager.hitboxManager.CreateHitboxGroup(groupIndex);
                    break;
            }
        }

        protected virtual bool CheckCancelWindows(AttackDefinition currentAttack)
        {
            return false;
        }
    }
}