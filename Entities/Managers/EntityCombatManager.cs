using CAF.Combat;
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

        public int HitStun { get; protected set; } = 0;
        public int HitStop { get; protected set; } = 0;
        public int CurrentChargeLevel { get; protected set; } = 0;
        public int CurrentChargeLevelCharge { get; protected set; } = 0;
        public MovesetAttackNode CurrentAttack { get; protected set; } = null;
        public MovesetDefinition CurrentMoveset { get; protected set; } = null;
        public HitInfo LastHitBy { get; protected set; }

        public EntityManager controller;
        public EntityHitboxManager hitboxManager;



        protected virtual void Awake()
        {
            hitboxManager = new EntityHitboxManager(this, controller);
        }

        public virtual void CLateUpdate()
        {
            if (HitStop > 0)
            {
                HitStop--;
                if(HitStop == 0)
                {
                    OnExitHitStop?.Invoke(controller);
                }
            }
            else if (HitStun > 0)
            {
                HitStun--;
                if(HitStun == 0)
                {
                    OnExitHitStun?.Invoke(controller);
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
            switch (controller.IsGrounded)
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
            switch (controller.IsGrounded)
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
                if (controller.StateManager.CurrentStateFrame >= CurrentAttack.nextNode[i].cancelWindow.x &&
                    controller.StateManager.CurrentStateFrame <= CurrentAttack.nextNode[i].cancelWindow.y)
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

        protected virtual MovesetAttackNode CheckAttackNodes(ref List<MovesetAttackNode> nodes)
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

            int currentOffset = 0;

            // Check execute button(s)
            bool pressedExecuteInputs = true;
            for (int e = 0; e < node.executeInputs.Count; e++)
            {
                switch (node.executeInputs[e].inputType)
                {
                    case Input.InputDefinitionType.Stick:
                        if (!CheckStickDirection(node.executeInputs[e].stickDirection, node.executeInputs[e].directionDeviation, 0))
                        {
                            pressedExecuteInputs = false;
                            break;
                        }
                        break;
                    case Input.InputDefinitionType.Button:
                        if (!controller.InputManager.GetButton(node.executeInputs[e].buttonID, out int gotOffset, 0, true, 3).firstPress)
                        {
                            pressedExecuteInputs = false;
                            break;
                        }
                        if(gotOffset < currentOffset)
                        {
                            currentOffset = gotOffset;
                        }
                        break;
                }
            }

            if (node.executeInputs.Count <= 0)
            {
                pressedExecuteInputs = false;
            }
            // We did not press the buttons required for this move.
            if (!pressedExecuteInputs)
            {
                return null;
            }

            bool pressedSequenceButtons = true;
            for (int s = 0; s < node.inputSequence.Count; s++)
            {
                switch (node.inputSequence[s].inputType) 
                {
                    case Input.InputDefinitionType.Stick:
                        bool foundDir = false;
                        for (int f = currentOffset; f < currentOffset + 8; f++)
                        {
                            if (CheckStickDirection(node.inputSequence[s].stickDirection, node.inputSequence[s].directionDeviation, f))
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
                        break;
                }
                if (!pressedSequenceButtons)
                {
                    break;
                }
            }

            if (!pressedSequenceButtons)
            {
                return null;
            }

            return node;
        }

        protected virtual bool CheckStickDirection(Vector2 wantedDirection, float deviation, int framesBack)
        {
            return false;
        }

        public virtual void SetHitStop(int value)
        {
            HitStop = value;
            OnEnterHitStop?.Invoke(controller);
        }

        public virtual void AddHitStop(int value)
        {
            HitStop += value;
            OnHitStopAdded?.Invoke(controller);
        }

        public virtual void SetHitStun(int value)
        {
            HitStun = value;
            OnEnterHitStun?.Invoke(controller);
         }

        public virtual void AddHitStun(int value)
        {
            HitStun += value;
            OnHitStunAdded?.Invoke(controller);
        }

        public virtual void SetMoveset(MovesetDefinition moveset)
        {
            MovesetDefinition oldMoveset = CurrentMoveset;
            CurrentMoveset = moveset;
            OnMovesetChanged?.Invoke(controller, oldMoveset);
        }

        public virtual void SetChargeLevel(int value)
        {
            int lastChargeLevel = value;
            CurrentChargeLevel = value;
            OnChargeLevelChanged?.Invoke(controller, lastChargeLevel);
        }
        
        public virtual void SetChargeLevelCharge(int value)
        {
            int oldValue = CurrentChargeLevelCharge;
            CurrentChargeLevelCharge = value;
            OnChargeLevelChargeChanged?.Invoke(controller, oldValue);
        }

        public virtual void IncrementChargeLevelCharge()
        {
            CurrentChargeLevelCharge++;
            OnChargeLevelChargeChanged?.Invoke(controller, CurrentChargeLevelCharge-1);
        }

        public virtual HitReaction Hurt(Vector3 center, Vector3 forward, Vector3 right, HitInfoBase hitInfo)
        {
            Debug.Log("Hit.");
            HitReaction hr = new HitReaction();
            hr.reactionType = HitReactionType.Hit;
            OnHit?.Invoke(null, controller, hitInfo);
            return hr;
        }

        public virtual void Heal(HealInfoBase healInfo)
        {
            OnHealed?.Invoke(null, controller, null);
        }

        public virtual int GetTeam()
        {
            return 0;
        }
    }
}