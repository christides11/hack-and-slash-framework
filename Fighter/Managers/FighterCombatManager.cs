using HnSF.Combat;
using HnSF.Input;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HnSF.Fighters
{
    public class FighterCombatManager : MonoBehaviour, IHurtable, IHealable, IFighterCombatManager
    {
        public delegate void EmptyAction(IFighterBase self);
        public delegate void HealthChangedAction(IFighterBase initializer, IFighterBase self, HitInfoBase hitInfo);
        public delegate void MovesetChangedAction(IFighterBase self, int lastMoveset);
        public delegate void ChargeLevelChangedAction(IFighterBase self, int lastChargeLevel);
        public delegate void ChargeLevelChargeChangedAction(IFighterBase self, int lastChargeLevelCharge);
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

        public virtual IFighterBase Manager { get; }
        public virtual IFighterPhysicsManager PhysicsManager { get; }
        public virtual IFighterStateManager StateManager { get; }

        protected int currentMoveset = 0;
        protected int currentAttackMoveset = -1;
        protected int currentAttackNode = -1;

        protected int hitstun;
        protected int hitstop; 

        public HitboxManager hitboxManager;

        public LayerMask hitboxLayerMask;

        protected virtual void Awake()
        {
            hitboxManager.OnHitHurtbox += OnHitEnemy;
        }

        protected virtual void OnHitEnemy(HitboxGroup hitboxGroup, int hitboxIndex, Hurtbox hurtbox, HitReactionBase hitReaction)
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
                    OnExitHitStop?.Invoke(Manager);
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
            switch (PhysicsManager.IsGrounded)
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
                if (StateManager.CurrentStateFrame >= CurrentAttackNode.nextNode[i].cancelWindow.x &&
                    StateManager.CurrentStateFrame <= CurrentAttackNode.nextNode[i].cancelWindow.y)
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

            if (CheckAttackNodeConditions(node) == false)
            {
                return null;
            }

            if (CheckForInputSequence(node.inputSequence))
            {
                return node;
            }
            return null;
        }

        protected virtual bool CheckAttackNodeConditions(MovesetAttackNode node)
        {
            return true;
        }

        /// <summary>
        /// Checks to see if a given input sequence was inputted.
        /// </summary>
        /// <param name="sequence">The sequence we're looking for.</param>
        /// <param name="baseOffset">How far back we want to start the sequence check. 0 = current frame, 1 = 1 frame back, etc.</param>
        /// <param name="processSequenceButtons">If the sequence buttons should be checked, even if the execute buttons were not pressed or don't exist.</param>
        /// <param name="holdInput">If the sequence check should check for the buttons being held down instead of their first prcess.</param>
        /// <returns>True if the input sequence was inputted.</returns>
        public virtual bool CheckForInputSequence(InputSequence sequence, uint baseOffset = 0, bool processSequenceButtons = false, bool holdInput = false)
        {
            uint currentOffset = 0;
            bool executeInputsSuccessful = CheckExecuteInputs(sequence, baseOffset, ref currentOffset);

            if (sequence.executeInputs.Count == 0)
            {
                currentOffset++;
                if (processSequenceButtons == false)
                {
                    executeInputsSuccessful = false;
                }
            }

            // We did not press the buttons required for this move.
            if (executeInputsSuccessful == false)
            {
                return false;
            }
            ClearBuffer();

            bool sequenceInputsSuccessful = CheckSequenceInputs(sequence, holdInput, ref currentOffset);

            if (sequenceInputsSuccessful == false)
            {
                return false;
            }
            return true;
        }
        protected virtual bool CheckExecuteInputs(InputSequence sequence, uint baseOffset, ref uint currentOffset)
        {
            throw new NotImplementedException("CheckExecuteInputs must be overriden.");
        }

        protected virtual bool CheckSequenceInputs(InputSequence sequence, bool holdInput, ref uint currentOffset)
        {
            throw new NotImplementedException("CheckSequenceInputs must be overriden.");
        }

        protected virtual void ClearBuffer()
        {

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
                OnExitHitStop?.Invoke(Manager);
                return;
            }
            OnEnterHitStop?.Invoke(Manager);
        }

        public virtual void AddHitStop(int value)
        {
            hitstop += value;
            OnHitStopAdded?.Invoke(Manager);
        }

        public virtual void SetHitStun(int value)
        {
            hitstun = value;
            if(hitstun == 0)
            {
                OnExitHitStun?.Invoke(Manager);
                return;
            }
            OnEnterHitStun?.Invoke(Manager);
         }

        public virtual void AddHitStun(int value)
        {
            hitstun += value;
            OnHitStunAdded?.Invoke(Manager);
        }

        public virtual void SetMoveset(int movesetIndex)
        {
            int oldMoveset = currentMoveset;
            currentMoveset = movesetIndex;
            OnMovesetChanged?.Invoke(Manager, oldMoveset);
        }

        public virtual void SetChargeLevel(int value)
        {
            int lastChargeLevel = value;
            CurrentChargeLevel = value;
            OnChargeLevelChanged?.Invoke(Manager, lastChargeLevel);
        }
        
        public virtual void SetChargeLevelCharge(int value)
        {
            int oldValue = CurrentChargeLevelCharge;
            CurrentChargeLevelCharge = value;
            OnChargeLevelChargeChanged?.Invoke(Manager, oldValue);
        }

        public virtual void IncrementChargeLevelCharge(int maxCharge)
        {
            CurrentChargeLevelCharge++;
            OnChargeLevelChargeChanged?.Invoke(Manager, CurrentChargeLevelCharge-1);
            if(CurrentChargeLevelCharge == maxCharge)
            {
                OnChargeLevelChargeMaxReached?.Invoke(Manager, CurrentChargeLevelCharge - 1);
            }
        }

        public virtual HitReactionBase Hurt(HurtInfoBase hurtInfoBase)
        {
            HitReactionBase hr = new HitReactionBase();
            OnHit?.Invoke(null, Manager, hurtInfoBase.hitInfo);
            return hr;
        }

        public virtual void Heal(HealInfoBase healInfo)
        {
            OnHealed?.Invoke(null, Manager, null);
        }

        public virtual int GetTeam()
        {
            return 0;
        }
    }
}