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
            return $"Attack ({GetEntityManager().CombatManager.CurrentAttack?.name})";
        }

        protected bool charging = true;

        public override void Initialize()
        {
            base.Initialize();
            AttackDefinition currentAttack = 
                (TDAction.Combat.AttackDefinition)GetEntityManager().CombatManager.CurrentAttack.attackDefinition;
            if (currentAttack.stateOverride > -1)
            {
                GetEntityManager().StateManager.ChangeState(currentAttack.stateOverride);
                return;
            }
            charging = true;
        }

        public override void OnUpdate()
        {
            EntityManager entityManager = GetEntityManager();
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

            if (TryCommandAttackCancel(currentAttack))
            {
                return;
            }

            // Process events.
            bool eventCancel = false;
            for (int i = 0; i < currentAttack.events.Count; i++)
            {
                if (HandleEvents(currentAttack.events[i]))
                {
                    // Event wants us to stall on the current frame.
                    eventCancel = true;
                    return;
                }
            }

            if (CheckInterrupt())
            {
                return;
            }
            if (!eventCancel && !HandleChargeLevels(entityManager, currentAttack))
            {
                entityManager.StateManager.IncrementFrame();
            }
        }

        /// <summary>
        /// Tries to cancel into a command attack.
        /// </summary>
        /// <param name="currentAttack">The current attack's information.</param>
        /// <returns>True if we attack canceled.</returns>
        protected virtual bool TryCommandAttackCancel(AttackDefinition currentAttack)
        {
            EntityManager e = GetEntityManager();
            for (int i = 0; i < currentAttack.commandAttackCancelWindows.Count; i++)
            {
                if (e.StateManager.CurrentStateFrame >= currentAttack.commandAttackCancelWindows[i].x
                    && e.StateManager.CurrentStateFrame <= currentAttack.commandAttackCancelWindows[i].y)
                {
                    CAF.Combat.MovesetAttackNode man = (CAF.Combat.MovesetAttackNode)e.CombatManager.TryCommandAttack();
                    if (man != null)
                    {
                        e.CombatManager.SetAttack(man);
                        e.StateManager.ChangeState((int)EntityStates.ATTACK);
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Handles the lifetime of events.
        /// </summary>
        /// <param name="currentEvent">The event being processed.</param>
        /// <returns>True if the current attack state was canceled by the event.</returns>
        protected virtual bool HandleEvents(CAF.Combat.AttackEventDefinition currentEvent)
        {
            if (!currentEvent.active)
            {
                return false;
            }
            EntityManager e = GetEntityManager();

            if(e.StateManager.CurrentStateFrame >= currentEvent.inputCheckStartFrame
                && e.StateManager.CurrentStateFrame <= currentEvent.inputCheckEndFrame)
            {
                switch (currentEvent.inputCheckTiming)
                {
                    case CAF.Combat.AttackEventInputCheckTiming.ONCE:
                        if (currentEvent.inputCheckProcessed)
                        {
                            break;
                        }
                        currentEvent.inputCheckProcessed = e.CombatManager.CheckForInputSequence(currentEvent.input);
                        Debug.Log($"?{currentEvent.inputCheckProcessed}, ({currentEvent.input.executeInputs.Count})");
                        break;
                    case CAF.Combat.AttackEventInputCheckTiming.CONTINUOUS:
                        currentEvent.inputCheckProcessed = e.CombatManager.CheckForInputSequence(currentEvent.input, 0, true, true);
                        break;
                }
            }

            if(currentEvent.inputCheckTiming != CAF.Combat.AttackEventInputCheckTiming.NONE
                && !currentEvent.inputCheckProcessed)
            {
                return false;
            }

            if (e.StateManager.CurrentStateFrame >= currentEvent.startFrame
                && e.StateManager.CurrentStateFrame <= currentEvent.endFrame)
            {
                if (currentEvent.onHit)
                {
                    List<CAF.Combat.IHurtable> ihList = 
                        ((EntityHitboxManager)e.CombatManager.hitboxManager).GetHitList(currentEvent.onHitHitboxGroup);
                    if (ihList == null)
                    {
                        return false;
                    }
                    if (ihList.Count <= 1)
                    {
                        return false;
                    }
                }
                return currentEvent.attackEvent.Evaluate(e.StateManager.CurrentStateFrame - currentEvent.startFrame,
                    currentEvent.endFrame - currentEvent.startFrame,
                    e,
                    currentEvent.variables);
            }
            return false;
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
                    cManager.IncrementChargeLevelCharge(currentAttack.chargeWindows[i].chargeLevels[cManager.CurrentChargeLevel].maxChargeFrames);
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
            EntityManager entityManager = GetEntityManager();
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
            if (CheckEnemyStepWindows(currentAttack)
                || CheckJumpCancelWindows(currentAttack)
                || CheckLandCancelWindows(currentAttack))
            {
                return true;
            }
            return false;
        }

        private bool CheckLandCancelWindows(AttackDefinition currentAttack)
        {
            EntityManager entityManager = GetEntityManager();
            for (int i = 0; i < currentAttack.landCancelWindows.Count; i++)
            {
                if (entityManager.StateManager.CurrentStateFrame >= currentAttack.landCancelWindows[i].x
                    || entityManager.StateManager.CurrentStateFrame <= currentAttack.landCancelWindows[i].y)
                {
                    if (entityManager.TryLandCancel())
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private bool CheckJumpCancelWindows(AttackDefinition currentAttack)
        {
            EntityManager entityManager = GetEntityManager();
            for (int i = 0; i < currentAttack.jumpCancelWindows.Count; i++)
            {
                if (entityManager.StateManager.CurrentStateFrame >= currentAttack.jumpCancelWindows[i].x
                    && entityManager.StateManager.CurrentStateFrame <= currentAttack.jumpCancelWindows[i].y)
                {
                    if (entityManager.TryJump())
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private bool CheckEnemyStepWindows(AttackDefinition currentAttack)
        {
            EntityManager e = GetEntityManager();
            for(int i = 0; i < currentAttack.enemyStepWindows.Count; i++)
            {
                if(e.StateManager.CurrentStateFrame >= currentAttack.enemyStepWindows[i].x
                    && e.StateManager.CurrentStateFrame <= currentAttack.enemyStepWindows[i].y)
                {
                    if (e.TryEnemyStep())
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}