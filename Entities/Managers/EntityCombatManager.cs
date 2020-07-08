using CAF.Combat;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CAF.Entities
{
    public class EntityCombatManager : MonoBehaviour, IHurtable
    {
        public int Team { get; set; } = 0;
        public MovesetAttackNode CurrentAttack { get; set; } = null;
        public MovesetDefinition CurrentMoveset { get; protected set; } = null;
        public HitInfo LastHitBy { get; protected set; }

        public EntityManager controller;
        public EntityHitboxManager hitboxManager;
        [SerializeField] public int hitStun;
        [SerializeField] public int hitStop;

        protected virtual void Awake()
        {
            hitboxManager = new EntityHitboxManager(this, controller);
        }

        public virtual void CLateUpdate()
        {
            if (hitStop > 0)
            {
                hitStop--;
            }
            else if (hitStun > 0)
            {
                hitStun--;
            }
            hitboxManager.TickBoxes();
        }

        public virtual void Cleanup()
        {
            if (CurrentAttack == null)
            {
                return;
            }
            hitboxManager.Reset();
            CurrentAttack = null;
        }

        public virtual bool TryAttack()
        {
            if(CurrentAttack == null)
            {
                return CheckStartingNodes();
            }
            if (CheckCurrentAttackCancelWindows())
            {
                return true;
            }
            return false;
        }

        protected virtual bool CheckStartingNodes()
        {
            switch (controller.IsGrounded)
            {
                case true:
                    if (CheckAttackNodes(ref CurrentMoveset.groundAttackCommandNormals))
                    {
                        return true;
                    }
                    if (CheckAttackNodes(ref CurrentMoveset.groundAttackStartNodes))
                    {
                        return true;
                    }
                    break;
                case false:
                    if (CheckAttackNodes(ref CurrentMoveset.airAttackCommandNormals))
                    {
                        return true;
                    }
                    if (CheckAttackNodes(ref CurrentMoveset.airAttackStartNodes))
                    {
                        return true;
                    }
                    break;
            }
            return false;
        }

        protected virtual bool CheckCurrentAttackCancelWindows()
        {
            for (int i = 0; i < CurrentAttack.nextNode.Count; i++)
            {
                if (controller.StateManager.CurrentStateFrame >= CurrentAttack.nextNode[i].cancelWindow.x &&
                    controller.StateManager.CurrentStateFrame <= CurrentAttack.nextNode[i].cancelWindow.y)
                {
                    if (CheckAttackNode(CurrentAttack.nextNode[i].node))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        protected virtual bool CheckAttackNodes(ref List<MovesetAttackNode> nodes)
        {
            for (int i = 0; i < nodes.Count; i++)
            {
                if (CheckAttackNode(nodes[i]))
                {
                    return true;
                }
            }
            return false;
        }

        protected virtual bool CheckAttackNode(MovesetAttackNode node)
        {
            if (node.attackDefinition == null)
            {
                return false;
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
                return false;
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
                return false;
            }


            Cleanup();
            CurrentAttack = node;
            return true;
        }

        protected virtual bool CheckStickDirection(Vector2 wantedDirection, float deviation, int framesBack)
        {
            return false;
        }

        public virtual HitReaction Hurt(Vector3 center, Vector3 forward, Vector3 right, HitInfo hitInfo)
        {
            HitReaction hr = new HitReaction();
            hr.reactionType = HitReactionType.Hit;
            controller.HealthManager.Hurt(hitInfo.damageOnHit);
            return hr;
        }

        public virtual void Heal()
        {

        }
    }
}