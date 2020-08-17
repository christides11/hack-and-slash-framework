using System;
using System.Collections;
using System.Collections.Generic;
using TDAction.Combat;
using TDAction.Inputs;
using UnityEngine;

namespace TDAction.Entities.States
{
    public class EntityStateAttack : EntityState
    {
        public override string GetName()
        {
            return $"Attack ({GetEntityController().CombatManager.CurrentAttack?.name}).";
        }

        protected bool charging = true;

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
            charging = true;
        }

        public override void OnUpdate()
        {
            EntityManager entityManager = GetEntityController();
            AttackDefinition currentAttack =
                (TDAction.Combat.AttackDefinition)entityManager.CombatManager.CurrentAttack.attackDefinition;

            // Handle lifetime of box groups.
            for (int i = 0; i < currentAttack.boxGroups.Count; i++)
            {
                HandleBoxGroup(i, currentAttack.boxGroups[i]);
            }

            // Check if we should cancel the attack with something else.
            if (CheckCancelWindows(currentAttack))
            {
                entityManager.CombatManager.Cleanup();
                return;
            }

            if (CheckInterrupt())
            {
                return;
            }
            if (!HandleChargeLevels(entityManager, currentAttack))
            {
                entityManager.StateManager.IncrementFrame();
            }
        }

        /// <summary>
        /// Handles processing the charge levels of the current attack.
        /// </summary>
        /// <param name="entityManager">The entity itself.</param>
        /// <param name="currentAttack">The current attack the entity is doing.</param>
        /// <returns>If the frame should be held.</returns>
        private bool HandleChargeLevels(EntityManager entityManager, AttackDefinition currentAttack)
        {
            EntityCombatManager cManager = (EntityCombatManager)entityManager.CombatManager;
            if (!charging)
            {
                return false;
            }

            if (!entityManager.InputManager.GetButton((int)EntityInputs.ATTACK).isDown)
            {
                charging = false;
                return false;
            }

            bool result = false;
            for(int i = 0; i < currentAttack.chargeWindows.Count; i++)
            {
                // Not on the correct frame.
                if(entityManager.StateManager.CurrentStateFrame != currentAttack.chargeWindows[i].frame)
                {
                    continue;
                }

                // Still have charge levels to go through.
                if(entityManager.CombatManager.CurrentChargeLevel < currentAttack.chargeWindows[i].chargeLevels.Count)
                {
                    cManager.IncrementChargeLevelCharge();
                    // Charge completed, move on to the next level.
                    if(cManager.CurrentChargeLevelCharge == currentAttack.chargeWindows[i].chargeLevels[cManager.CurrentChargeLevel].maxChargeFrames)
                    {
                        cManager.SetChargeLevel(cManager.CurrentChargeLevel+1);
                        cManager.SetChargeLevelCharge(0);
                    }
                } else if (currentAttack.chargeWindows[i].releaseOnCompletion)
                {
                    charging = false;
                }
                result = true;
                // Only one charge level can be handled per frame, ignore everything else.
                break;
            }
            return result;
        }

        /// <summary>
        /// Handles the lifetime process of box groups.
        /// </summary>
        /// <param name="groupIndex">The group number being processed.</param>
        /// <param name="boxGroup">The group being processed.</param>
        protected virtual void HandleBoxGroup(int groupIndex, CAF.Combat.BoxGroup boxGroup)
        {
            EntityManager entityManager = GetEntityController();
            // Cleanup the box if it's active frames are over.
            if(entityManager.StateManager.CurrentStateFrame == boxGroup.activeFramesEnd + 1)
            {
                entityManager.CombatManager.hitboxManager.DeactivateHitboxGroup(groupIndex);
            }

            // Make sure we're in the frame window of the box.
            if(entityManager.StateManager.CurrentStateFrame < boxGroup.activeFramesStart
                || entityManager.StateManager.CurrentStateFrame > boxGroup.activeFramesEnd)
            {
                return;
            }

            // Check if the charge level requirement was met.
            if(boxGroup.chargeLevelNeeded >= 0)
            {
                int currentChargeLevel = entityManager.CombatManager.CurrentChargeLevel;
                if (currentChargeLevel <= boxGroup.chargeLevelNeeded
                    || currentChargeLevel > boxGroup.chargeLevelMax)
                {
                    return;
                }
            }

            // Create the box.
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