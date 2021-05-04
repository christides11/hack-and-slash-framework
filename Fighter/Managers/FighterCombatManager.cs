using CAF.Combat;
using CAF.Input;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CAF.Fighters
{
    public class FighterCombatManager : MonoBehaviour, IHurtable
    {
        public delegate void EmptyAction(FighterBase self);
        public delegate void HealthChangedAction(FighterBase initializer, FighterBase self, HitInfoBase hitInfo);
        public delegate void MovesetChangedAction(FighterBase self, MovesetDefinition lastMoveset);
        public delegate void ChargeLevelChangedAction(FighterBase self, int lastChargeLevel);
        public delegate void ChargeLevelChargeChangedAction(FighterBase self, int lastChargeLevelCharge);
        public event HealthChangedAction OnHit;
        public event HealthChangedAction OnHealed;
        public event EmptyAction OnEnterHitStop;
        public event EmptyAction OnEnterHitStun;
        public event EmptyAction OnHitStopAdded;
        public event EmptyAction OnHitStunAdded;
        public event EmptyAction OnExitHitStop;
        public event EmptyAction OnExitHitStun;
        public event MovesetChangedAction OnMovesetChanged;
        public event ChargeLevelChangedAction OnChargeLevelChanged;
        public event ChargeLevelChargeChangedAction OnChargeLevelChargeChanged;
        public event ChargeLevelChargeChangedAction OnChargeLevelChargeMaxReached;

        public int HitStun { get { return hitstun; } }
        public int HitStop { get { return hitstop; } }
        public int CurrentChargeLevel { get; protected set; } = 0;
        public int CurrentChargeLevelCharge { get; protected set; } = 0;
        public MovesetAttackNode CurrentAttack { get; protected set; } = null;
        public MovesetDefinition CurrentMoveset { get; protected set; } = null;
        public HitInfoBase LastHitBy { get; protected set; }

        protected int hitstun;
        protected int hitstop; 

        public FighterBase manager;
        public FighterHitboxManager hitboxManager;

        public LayerMask hitboxLayerMask;

        protected virtual void Awake()
        {
            hitboxManager = new FighterHitboxManager(this, manager);
        }

        public virtual void CLateUpdate()
        {
            if (hitstop > 0)
            {
                hitstop--;
                if(hitstop == 0)
                {
                    OnExitHitStop?.Invoke(manager);
                }
            }
        }

        public virtual void Cleanup()
        {
            if (CurrentAttack == null)
            {
                return;
            }
            CurrentChargeLevel = 0;
            CurrentChargeLevelCharge = 0;
            CurrentAttack = null;
            hitboxManager.Reset();
        }

        public virtual void SetAttack(MovesetAttackNode attackNode)
        {
            Cleanup();
            CurrentAttack = attackNode;
        }

        public virtual MovesetAttackNode TryAttack()
        {
            if(CurrentMoveset == null)
            {
                return null;
            }
            if(CurrentAttack == null)
            {
                return CheckStartingNodes();
            }
            return CheckCurrentAttackCancelWindows();
        }

        public virtual MovesetAttackNode TryCommandAttack()
        {
            switch (manager.IsGrounded)
            {
                case true:
                    MovesetAttackNode groundCommandNormal = CheckAttackNodes(ref CurrentMoveset.groundAttackCommandNormals);
                    if (groundCommandNormal != null)
                    {
                        return groundCommandNormal;
                    }
                    break;
                case false:
                    MovesetAttackNode airCommandNormal = CheckAttackNodes(ref CurrentMoveset.airAttackCommandNormals);
                    if (airCommandNormal != null)
                    {
                        return airCommandNormal;
                    }
                    break;
            }
            return null;
        }

        protected virtual MovesetAttackNode CheckStartingNodes()
        {
            switch (manager.IsGrounded)
            {
                case true:
                    MovesetAttackNode groundCommandNormal = CheckAttackNodes(ref CurrentMoveset.groundAttackCommandNormals);
                    if (groundCommandNormal != null)
                    {
                        return groundCommandNormal;
                    }
                    MovesetAttackNode groundNormal = CheckAttackNodes(ref CurrentMoveset.groundAttackStartNodes);
                    if (groundNormal != null)
                    {
                        return groundNormal;
                    }
                    break;
                case false:
                    MovesetAttackNode airCommandNormal = CheckAttackNodes(ref CurrentMoveset.airAttackCommandNormals);
                    if (airCommandNormal != null)
                    {
                        return airCommandNormal;
                    }
                    MovesetAttackNode airNormal = CheckAttackNodes(ref CurrentMoveset.airAttackStartNodes);
                    if (airNormal != null)
                    {
                        return airNormal;
                    }
                    break;
            }
            return null;
        }

        protected virtual MovesetAttackNode CheckCurrentAttackCancelWindows()
        {
            for (int i = 0; i < CurrentAttack.nextNode.Count; i++)
            {
                if (manager.StateManager.CurrentStateFrame >= CurrentAttack.nextNode[i].cancelWindow.x &&
                    manager.StateManager.CurrentStateFrame <= CurrentAttack.nextNode[i].cancelWindow.y)
                {
                    MovesetAttackNode man = CheckAttackNode(CurrentAttack.nextNode[i].node);
                    if(man != null)
                    {
                        return man;
                    }
                }
            }
            return null;
        }

        protected virtual MovesetAttackNode CheckAttackNodes<T>(ref List<T> nodes) where T : MovesetAttackNode
        {
            for (int i = 0; i < nodes.Count; i++)
            {
                MovesetAttackNode man = CheckAttackNode(nodes[i]);
                if(man != null)
                {
                    return man;
                }
            }
            return null;
        }

        protected virtual MovesetAttackNode CheckAttackNode(MovesetAttackNode node)
        {
            if (node.attackDefinition == null)
            {
                return null;
            }

            if (CheckForInputSequence(node.inputSequence))
            {
                return node;
            }
            return null;
        }

        /// <summary>
        /// Checks to see if a given input sequence was inputted.
        /// </summary>
        /// <param name="sequence">The sequence we're looking for.</param>
        /// <param name="baseOffset">How far back we want to start the sequence check. 0 = current frame, 1 = 1 frame back, etc.</param>
        /// <param name="processSequenceButtons">If the sequence buttons should be checked, even if the execute buttons were not pressed.</param>
        /// <param name="holdInput">If the sequence check should check for the buttons being held down instead of their first prcess.</param>
        /// <returns>True if the input sequence was inputted.</returns>
        public virtual bool CheckForInputSequence(InputSequence sequence, uint baseOffset = 0, bool processSequenceButtons = false, bool holdInput = false)
        {
            uint currentOffset = 0;
            // Check execute button(s)
            bool pressedExecuteInputs = true;
            for (int e = 0; e < sequence.executeInputs.Count; e++)
            {
                switch (sequence.executeInputs[e].inputType)
                {
                    case Input.InputDefinitionType.Stick:
                        if (!CheckStickDirection(sequence.executeInputs[e], baseOffset))
                        {
                            pressedExecuteInputs = false;
                            break;
                        }
                        break;
                    case Input.InputDefinitionType.Button:
                        if (!manager.InputManager.GetButton(sequence.executeInputs[e].buttonID, out uint gotOffset, baseOffset, 
                            true, sequence.executeWindow).firstPress)
                        {
                            pressedExecuteInputs = false;
                            break;
                        }
                        if (gotOffset >= currentOffset)
                        {
                            currentOffset = gotOffset;
                        }
                        break;
                }
            }

            if (sequence.executeInputs.Count <= 0)
            {
                currentOffset++;
                if (!processSequenceButtons)
                {
                    pressedExecuteInputs = false;
                }
            }
            // We did not press the buttons required for this move.
            if (!pressedExecuteInputs)
            {
                return false;
            }
            
            // Execute inputs where used, make them unusable to stop them from being read twice.
            for(int a = 0; a < sequence.executeInputs.Count; a++)
            {
                manager.InputManager.ClearBuffer(sequence.executeInputs[a].buttonID);
            }
            // Check sequence button(s).
            bool pressedSequenceButtons = true;
            for (int s = 0; s < sequence.sequenceInputs.Count; s++)
            {
                switch (sequence.sequenceInputs[s].inputType)
                {
                    case Input.InputDefinitionType.Stick:
                        bool foundDir = false;
                        for (uint f = currentOffset; f < currentOffset + sequence.sequenceWindow; f++)
                        {
                            if (CheckStickDirection(sequence.sequenceInputs[s], f))
                            {
                                foundDir = true;
                                currentOffset = f;
                                break;
                            }
                        }
                        if (!foundDir)
                        {
                            pressedSequenceButtons = false;
                            break;
                        }
                        break;
                    case Input.InputDefinitionType.Button:
                        bool foundButton = false;
                        for (uint f = currentOffset; f < currentOffset + sequence.sequenceWindow; f++)
                        {
                            if ( (!holdInput && manager.InputManager.GetButton(sequence.sequenceInputs[s].buttonID, out uint gotOffset, f, false).firstPress)
                                || (holdInput && manager.InputManager.GetButton(sequence.sequenceInputs[s].buttonID, out uint gotOffsetTwo, f, false).isDown) )
                            {
                                foundButton = true;
                                currentOffset = f;
                                break;
                            }
                        }
                        if (!foundButton)
                        {
                            pressedSequenceButtons = false;
                            break;
                        }
                        break;
                }
                if (!pressedSequenceButtons)
                {
                    break;
                }
            }

            if (!pressedSequenceButtons)
            {
                return false;
            }
            return true;
        }

        protected virtual bool CheckStickDirection(InputDefinition sequenceInput, uint framesBack)
        {
            Debug.LogError("CheckStickDirection has to be overrided for command inputs to work.");
            return false;
        }

        public virtual void SetHitStop(int value)
        {
            hitstop = value;
            if(hitstop == 0)
            {
                OnExitHitStop?.Invoke(manager);
                return;
            }
            OnEnterHitStop?.Invoke(manager);
        }

        public virtual void AddHitStop(int value)
        {
            hitstop += value;
            OnHitStopAdded?.Invoke(manager);
        }

        public virtual void SetHitStun(int value)
        {
            hitstun = value;
            if(hitstun == 0)
            {
                OnExitHitStun?.Invoke(manager);
                return;
            }
            OnEnterHitStun?.Invoke(manager);
         }

        public virtual void AddHitStun(int value)
        {
            hitstun += value;
            OnHitStunAdded?.Invoke(manager);
        }

        public virtual void SetMoveset(MovesetDefinition moveset)
        {
            MovesetDefinition oldMoveset = CurrentMoveset;
            CurrentMoveset = moveset;
            OnMovesetChanged?.Invoke(manager, oldMoveset);
        }

        public virtual void SetChargeLevel(int value)
        {
            int lastChargeLevel = value;
            CurrentChargeLevel = value;
            OnChargeLevelChanged?.Invoke(manager, lastChargeLevel);
        }
        
        public virtual void SetChargeLevelCharge(int value)
        {
            int oldValue = CurrentChargeLevelCharge;
            CurrentChargeLevelCharge = value;
            OnChargeLevelChargeChanged?.Invoke(manager, oldValue);
        }

        public virtual void IncrementChargeLevelCharge(int maxCharge)
        {
            CurrentChargeLevelCharge++;
            OnChargeLevelChargeChanged?.Invoke(manager, CurrentChargeLevelCharge-1);
            if(CurrentChargeLevelCharge == maxCharge)
            {
                OnChargeLevelChargeMaxReached?.Invoke(manager, CurrentChargeLevelCharge - 1);
            }
        }

        public virtual HitReaction Hurt(HurtInfoBase hurtInfoBase)
        {
            HitReaction hr = new HitReaction();
            hr.reactionType = HitReactionType.Hit;
            OnHit?.Invoke(null, manager, hurtInfoBase.hitInfo);
            return hr;
        }

        public virtual void Heal(HealInfoBase healInfo)
        {
            OnHealed?.Invoke(null, manager, null);
        }

        public virtual int GetTeam()
        {
            return 0;
        }
    }
}