using HnSF.Combat;
using HnSF.Input;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HnSF.Fighters
{
    public class FighterCombatManager : MonoBehaviour, IHurtable
    {
        public delegate void EmptyAction(FighterBase self);
        public delegate void HealthChangedAction(FighterBase initializer, FighterBase self, HitInfoBase hitInfo);
        public delegate void MovesetChangedAction(FighterBase self, int lastMoveset);
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
        /// <summary>
        /// The current moveset we are assigned.
        /// </summary>
        public virtual MovesetDefinition CurrentMoveset { get; }
        /// <summary>
        /// The identifier of the current moveset.
        /// </summary>
        public virtual int CurrentMovesetIdentifier { get { return currentMoveset; } }
        /// <summary>
        /// The moveset that the current attack belongs to. Not the same as our current moveset.
        /// </summary>
        public virtual MovesetDefinition CurrentAttackMoveset { get { return GetMoveset(currentAttackMoveset); } }
        /// <summary>
        /// The identifier of the moveset that the current attack belongs to. Not the same as our current moveset.
        /// </summary>
        public virtual int CurrentAttackMovesetIdentifier { get { return currentAttackMoveset; } }
        /// <summary>
        /// The attack node of the current attack.
        /// </summary>
        public virtual MovesetAttackNode CurrentAttackNode { get { if (currentAttackNode < 0) { return null; } 
                return (MovesetAttackNode)GetMoveset(currentAttackMoveset).GetAttackNode(currentAttackNode); } }
        public virtual int CurrentAttackNodeIdentifier { get { return currentAttackNode; } }

        protected int currentMoveset = 0;
        protected int currentAttackMoveset = -1;
        protected int currentAttackNode = -1;

        protected int hitstun;
        protected int hitstop; 

        public FighterBase manager;
        public HitboxManager hitboxManager;

        public LayerMask hitboxLayerMask;

        protected virtual void Awake()
        {
            hitboxManager.OnHitHurtbox += OnHitEnemy;
        }

        protected virtual void OnHitEnemy(HitboxGroup hitboxGroup, int hitboxIndex, Hurtbox hurtbox)
        {
            SetHitStop(hitboxGroup.hitboxHitInfo.attackerHitstop);
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

        public virtual int GetMovesetCount()
        {
            throw new NotImplementedException("GetMovesetCount must be overriden.");
        }

        public virtual MovesetDefinition GetMoveset(int index)
        {
            throw new NotImplementedException("GetMoveset must be overriden.");
        }

        public virtual void Cleanup()
        {
            if (CurrentAttackNode == null)
            {
                return;
            }
            CurrentChargeLevel = 0;
            CurrentChargeLevelCharge = 0;
            currentAttackMoveset = -1;
            currentAttackNode = -1;
            hitboxManager.Reset();
        }

        /// <summary>
        /// Resets anything having to do with the current attack and sets a new one. This method assumes that the attack node is in the current moveset.
        /// </summary>
        /// <param name="attackNodeIdentifier">The identifier of the attack node.</param>
        public virtual void SetAttack(int attackNodeIdentifier)
        {
            Cleanup();
            currentAttackMoveset = currentMoveset;
            currentAttackNode = attackNodeIdentifier;
        }

        /// <summary>
        /// Resets anything having to do with the current attack and sets a new one. 
        /// </summary>
        /// <param name="attackNodeIdentifier"></param>
        /// <param name="attackMovesetIdentifier"></param>
        public virtual void SetAttack(int attackNodeIdentifier, int attackMovesetIdentifier)
        {
            Cleanup();
            currentAttackMoveset = attackMovesetIdentifier;
            currentAttackNode = attackNodeIdentifier;
        }

        /// <summary>
        /// Checks if we can attack based on our current attack state.
        /// </summary>
        /// <returns>The identifier of the attack node that can be done. If none, returns -1.</returns>
        public virtual int TryAttack()
        {
            if(CurrentMoveset == null)
            {
                return -1;
            }
            // We are currently not doing an attack, so check the starting nodes instead.
            if(CurrentAttackNode == null)
            {
                return CheckStartingNodes();
            }
            // We are doing an attack, check it's cancel windows.
            return CheckCurrentAttackCancelWindows();
        }

        /// <summary>
        /// Check a cancel list in the current moveset to see if an attack can be canceled into.
        /// </summary>
        /// <param name="cancelListID"></param>
        /// <returns>The identifier of the attack node if found. Otherwise -1.</returns>
        public virtual int TryCancelList(int cancelListID)
        {
            CancelList cl = CurrentMoveset.GetCancelList(cancelListID);
            if (cl == null)
            {
                return -1;
            }
            int attack = CheckAttackNodes(ref cl.nodes);
            if(attack != -1)
            {
                return attack;
            }
            return -1;
        }

        protected virtual int CheckStartingNodes()
        {
            MovesetDefinition moveset = CurrentMoveset;
            switch (manager.PhysicsManager.IsGrounded)
            {
                case true:
                    if(moveset.groundIdleCancelListID != -1)
                    {
                        int cancel = TryCancelList(moveset.groundIdleCancelListID);
                        if(cancel != -1)
                        {
                            return cancel;
                        }
                    }
                    int groundNormal = CheckAttackNodes(ref moveset.groundAttackStartNodes);
                    if (groundNormal != -1)
                    {
                        return groundNormal;
                    }
                    break;
                case false:
                    if (moveset.airIdleCancelListID != -1)
                    {
                        int cancel = TryCancelList(moveset.airIdleCancelListID);
                        if (cancel != -1)
                        {
                            return cancel;
                        }
                    }
                    int airNormal = CheckAttackNodes(ref moveset.airAttackStartNodes);
                    if (airNormal != -1)
                    {
                        return airNormal;
                    }
                    break;
            }
            return -1;
        }

        /// <summary>
        /// Checks the cancel windows of the current attack to see if we should transition to the next attack.
        /// </summary>
        /// <returns>The identifier of the attack if the transition conditions are met. Otherwise -1.</returns>
        protected virtual int CheckCurrentAttackCancelWindows()
        {
            // Our current moveset is not the same as our current attack's, ignore it's cancel windows.
            if(currentMoveset != currentAttackMoveset)
            {
                return -1;
            }
            for (int i = 0; i < CurrentAttackNode.nextNode.Count; i++)
            {
                if (manager.StateManager.CurrentStateFrame >= CurrentAttackNode.nextNode[i].cancelWindow.x &&
                    manager.StateManager.CurrentStateFrame <= CurrentAttackNode.nextNode[i].cancelWindow.y)
                {
                    MovesetAttackNode currentNode = CurrentAttackNode;
                    MovesetAttackNode man = CheckAttackNode((MovesetAttackNode)GetMoveset(currentAttackMoveset).GetAttackNode(currentNode.nextNode[i].nodeIdentifier));
                    if(man != null)
                    {
                        return currentNode.nextNode[i].nodeIdentifier;
                    }
                }
            }
            return -1;
        }

        protected virtual int CheckAttackNodes<T>(ref List<T> nodes) where T : MovesetAttackNode
        {
            for (int i = 0; i < nodes.Count; i++)
            {
                MovesetAttackNode man = CheckAttackNode(nodes[i]);
                if(man != null)
                {
                    return nodes[i].Identifier;
                }
            }
            return -1;
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
                if (processSequenceButtons == false)
                {
                    pressedExecuteInputs = false;
                }
            }
            // We did not press the buttons required for this move.
            if (!pressedExecuteInputs)
            {
                return false;
            }
            
            manager.InputManager.ClearBuffer();
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
            throw new NotImplementedException("CheckStickDirection has to be overrided for command inputs to work.");
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

        public virtual void SetMoveset(int movesetIndex)
        {
            int oldMoveset = currentMoveset;
            currentMoveset = movesetIndex;
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