using CAF.Combat;
using CAF.Input;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CAF.Entities
{
    public class EntityCombatManager : MonoBehaviour, IHurtable
    {
        public delegate void EmptyAction(EntityManager self);
        public delegate void HealthChangedAction(EntityManager initializer, EntityManager self, HitInfoBase hitInfo);
        public delegate void MovesetChangedAction(EntityManager self, MovesetDefinition lastMoveset);
        public delegate void ChargeLevelChangedAction(EntityManager self, int lastChargeLevel);
        public delegate void ChargeLevelChargeChangedAction(EntityManager self, int lastChargeLevelCharge);
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

        public int HitStun { get { return hitstun; } set { hitstun = value; } }
        public int HitStop { get { return hitstop; } set { hitstop = value; } }
        public int CurrentChargeLevel { get; protected set; } = 0;
        public int CurrentChargeLevelCharge { get; protected set; } = 0;
        public MovesetAttackNode CurrentAttack { get; protected set; } = null;
        public MovesetDefinition CurrentMoveset { get; protected set; } = null;
        public HitInfoBase LastHitBy { get; protected set; }

        protected int hitstun = 0;
        protected int hitstop;

        public EntityManager manager;
        public EntityHitboxManager hitboxManager;



        protected virtual void Awake()
        {
            hitboxManager = new EntityHitboxManager(this, manager);
        }

        public virtual void CLateUpdate()
        {
            if (HitStop > 0)
            {
                HitStop--;
                if(HitStop == 0)
                {
                    OnExitHitStop?.Invoke(manager);
                }
            }
            else if (HitStun > 0)
            {
                HitStun--;
                if(HitStun == 0)
                {
                    OnExitHitStun?.Invoke(manager);
                }
            }
            hitboxManager.TickBoxes();
        }

        public virtual void Cleanup()
        {
            if (CurrentAttack == null)
            {
                return;
            }
            CurrentChargeLevel = 0;
            CurrentChargeLevelCharge = 0;
            hitboxManager.Reset();
            CurrentAttack = null;
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

        public virtual bool CheckForInputSequence(InputSequence sequence, bool processSequenceButtons = false, bool holdInput = false)
        {
            int currentOffset = 0;
            // Check execute button(s)
            bool pressedExecuteInputs = true;
            for (int e = 0; e < sequence.executeInputs.Count; e++)
            {
                switch (sequence.executeInputs[e].inputType)
                {
                    case Input.InputDefinitionType.Stick:
                        if (!CheckStickDirection(sequence.executeInputs[e], 0))
                        {
                            pressedExecuteInputs = false;
                            break;
                        }
                        break;
                    case Input.InputDefinitionType.Button:
                        if (!manager.InputManager.GetButton(sequence.executeInputs[e].buttonID, out int gotOffset, 0, 
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
                        for (int f = currentOffset; f < currentOffset + sequence.sequenceWindow; f++)
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
                        for (int f = currentOffset; f < currentOffset + sequence.sequenceWindow; f++)
                        {
                            if ( (!holdInput && manager.InputManager.GetButton(sequence.sequenceInputs[s].buttonID, out int gotOffset, f, false).firstPress)
                                || (holdInput && manager.InputManager.GetButton(sequence.sequenceInputs[s].buttonID, out int gotOffsetTwo, f, false).isDown) )
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

        protected virtual bool CheckStickDirection(InputDefinition sequenceInput, int framesBack)
        {
            Debug.LogError("CheckStickDirection has to be overrided for command inputs to work.");
            return false;
        }

        public virtual void SetHitStop(int value)
        {
            HitStop = value;
            OnEnterHitStop?.Invoke(manager);
        }

        public virtual void AddHitStop(int value)
        {
            HitStop += value;
            OnHitStopAdded?.Invoke(manager);
        }

        public virtual void SetHitStun(int value)
        {
            HitStun = value;
            OnEnterHitStun?.Invoke(manager);
         }

        public virtual void AddHitStun(int value)
        {
            HitStun += value;
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