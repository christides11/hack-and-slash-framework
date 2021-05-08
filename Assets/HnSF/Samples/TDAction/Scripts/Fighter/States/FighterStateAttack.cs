using System;
using TDAction.Combat;
using TDAction.Inputs;
using UnityEngine;

namespace TDAction.Fighter
{
    public class FighterStateAttack : FighterState
    {
        public override string GetName()
        {
            return $"Attack ({GetEntityManager().CombatManager.CurrentAttack?.name})";
        }

        protected bool charging = true;

        public override void Initialize()
        {
            AttackDefinition currentAttack = 
                (TDAction.Combat.AttackDefinition)GetEntityManager().CombatManager.CurrentAttack.attackDefinition;
            if (currentAttack.useState)
            {
                GetEntityManager().StateManager.ChangeState(currentAttack.stateOverride);
                return;
            }
            charging = true;
            if (String.IsNullOrEmpty(currentAttack.animationName) == false)
            {
                (Manager as FighterManager).entityAnimator.SetAnimation(currentAttack.animationName);
            }
        }

        public override void OnUpdate()
        {
            FighterManager entityManager = GetEntityManager();
            AttackDefinition currentAttack =
                (TDAction.Combat.AttackDefinition)entityManager.CombatManager.CurrentAttack.attackDefinition;

            // Handle lifetime of box groups.
            for (int i = 0; i < currentAttack.hitboxGroups.Count; i++)
            {
                HandleHitboxGroup(i, currentAttack.hitboxGroups[i]);
            }

            if (TryCancelWindow(currentAttack))
            {
                return;
            }

            // Process events.
            bool eventCancel = false;
            bool interrupted = false;
            bool cleanup = false;
            for (int i = 0; i < currentAttack.events.Count; i++)
            {
                switch(HandleEvents(currentAttack, currentAttack.events[i]))
                {
                    case HnSF.Combat.AttackEventReturnType.STALL:
                        // Event wants us to stall on the current frame.
                        eventCancel = true;
                        break;
                    case HnSF.Combat.AttackEventReturnType.INTERRUPT:
                        interrupted = true;
                        cleanup = true;
                        break;
                    case HnSF.Combat.AttackEventReturnType.INTERRUPT_NO_CLEANUP:
                        interrupted = true;
                        break;
                }
            }

            if (interrupted)
            {
                if (cleanup)
                {
                    entityManager.CombatManager.Cleanup();
                }
                return;
            }

            if (CheckInterrupt())
            {
                return;
            }
            if (!eventCancel && !HandleChargeLevels(entityManager, currentAttack))
            {
                entityManager.StateManager.IncrementFrame();
            }
            (Manager as FighterManager).entityAnimator.SetFrame((int)entityManager.StateManager.CurrentStateFrame);
        }

        protected virtual bool TryCancelWindow(AttackDefinition currentAttack)
        {
            FighterManager e = GetEntityManager();
            for(int i = 0; i < currentAttack.cancels.Count; i++)
            {
                if (e.StateManager.CurrentStateFrame >= currentAttack.cancels[i].startFrame
                    && e.StateManager.CurrentStateFrame <= currentAttack.cancels[i].endFrame)
                {
                    int man = e.CombatManager.TryCancelList(currentAttack.cancels[i].cancelListID);
                    if(man != -1)
                    {
                        e.CombatManager.SetAttack(man);
                        e.StateManager.ChangeState((int)FighterStates.ATTACK);
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
        protected virtual HnSF.Combat.AttackEventReturnType HandleEvents(AttackDefinition currentAttack, HnSF.Combat.AttackEventDefinition currentEvent)
        {
            if (!currentEvent.active)
            {
                return HnSF.Combat.AttackEventReturnType.NONE;
            }
            FighterManager e = GetEntityManager();

            // Input Checking.
            if(e.StateManager.CurrentStateFrame >= currentEvent.inputCheckStartFrame
                && e.StateManager.CurrentStateFrame <= currentEvent.inputCheckEndFrame)
            {
                switch (currentEvent.inputCheckTiming)
                {
                    case HnSF.Combat.AttackEventInputCheckTiming.ONCE:
                        if (currentEvent.inputCheckProcessed)
                        {
                            break;
                        }
                        currentEvent.inputCheckProcessed = e.CombatManager.CheckForInputSequence(currentEvent.input);
                        Debug.Log($"?{currentEvent.inputCheckProcessed}, ({currentEvent.input.executeInputs.Count})");
                        break;
                    case HnSF.Combat.AttackEventInputCheckTiming.CONTINUOUS:
                        currentEvent.inputCheckProcessed = e.CombatManager.CheckForInputSequence(currentEvent.input, 0, true, true);
                        break;
                }
            }

            if(currentEvent.inputCheckTiming != HnSF.Combat.AttackEventInputCheckTiming.NONE
                && !currentEvent.inputCheckProcessed)
            {
                return HnSF.Combat.AttackEventReturnType.NONE;
            }

            if (e.StateManager.CurrentStateFrame >= currentEvent.startFrame
                && e.StateManager.CurrentStateFrame <= currentEvent.endFrame)
            {
                // Hit Check.
                if (currentEvent.onHitCheck != HnSF.Combat.OnHitType.NONE)
                {
                    if(currentEvent.onHitCheck == HnSF.Combat.OnHitType.ID_GROUP)
                    {
                        if (((FighterHitboxManager)e.CombatManager.hitboxManager).IDGroupHasHurt(currentEvent.onHitIDGroup) == false)
                        {
                            return HnSF.Combat.AttackEventReturnType.NONE;
                        }
                    }
                    else if(currentEvent.onHitCheck == HnSF.Combat.OnHitType.HITBOX_GROUP)
                    {
                        if (((FighterHitboxManager)e.CombatManager.hitboxManager).HitboxGroupHasHurt(currentAttack.hitboxGroups[currentEvent.onHitHitboxGroup].ID, 
                            currentEvent.onHitHitboxGroup) == false)
                        {
                            return HnSF.Combat.AttackEventReturnType.NONE;
                        }
                    }
                }
                return currentEvent.attackEvent.Evaluate((int)(e.StateManager.CurrentStateFrame - currentEvent.startFrame),
                    currentEvent.endFrame - currentEvent.startFrame,
                    e,
                    currentEvent.variables);
            }
            return HnSF.Combat.AttackEventReturnType.NONE;
        }

        /// <summary>
        /// Handles processing the charge levels of the current attack.
        /// </summary>
        /// <param name="entityManager">The entity itself.</param>
        /// <param name="currentAttack">The current attack the entity is doing.</param>
        /// <returns>If the frame should be held.</returns>
        private bool HandleChargeLevels(FighterManager entityManager, AttackDefinition currentAttack)
        {
            FighterCombatManager cManager = (FighterCombatManager)entityManager.CombatManager;
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
                if(entityManager.StateManager.CurrentStateFrame != currentAttack.chargeWindows[i].startFrame)
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
        protected virtual void HandleHitboxGroup(int groupIndex, HnSF.Combat.HitboxGroup boxGroup)
        {
            FighterManager entityManager = GetEntityManager();
            // Make sure we're in the frame window of the box.
            if (entityManager.StateManager.CurrentStateFrame < boxGroup.activeFramesStart
                || entityManager.StateManager.CurrentStateFrame > boxGroup.activeFramesEnd)
            {
                return;
            }

            // Check if the charge level requirement was met.
            if (boxGroup.chargeLevelNeeded >= 0)
            {
                int currentChargeLevel = entityManager.CombatManager.CurrentChargeLevel;
                if (currentChargeLevel < boxGroup.chargeLevelNeeded
                    || currentChargeLevel > boxGroup.chargeLevelMax)
                {
                    return;
                }
            }

            // Hit check.
            switch (boxGroup.hitGroupType)
            {
                case HnSF.Combat.HitboxType.HIT:
                    entityManager.CombatManager.hitboxManager.CheckForCollision(groupIndex, boxGroup);
                    break;
            }
        }
    }
}