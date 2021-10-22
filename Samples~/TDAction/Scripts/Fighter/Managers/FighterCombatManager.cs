using HnSF.Combat;
using HnSF.Fighters;
using HnSF.Input;
using System.Collections;
using System.Collections.Generic;
using TDAction.Combat;
using TDAction.Inputs;
using UnityEngine;
using HitInfo = TDAction.Combat.HitInfo;

namespace TDAction.Fighter
{
    public class FighterCombatManager : HnSF.Fighters.FighterCombatManager
    {
        public override IFighterBase Manager { get { return manager; } }
        public override IFighterPhysicsManager PhysicsManager { get { return physicsManager; } }
        public override IFighterStateManager StateManager { get { return stateManager; } }

        [SerializeField] protected FighterManager manager;
        [SerializeField] protected FighterPhysicsManager physicsManager;
        [SerializeField] protected FighterStateManager stateManager;

        public override HnSF.Combat.MovesetDefinition CurrentMoveset { get { return (Manager as FighterManager).entityDefinition.movesets[currentMoveset]; } }

        public TeamTypes team = TeamTypes.FFA;

        public override int GetTeam()
        {
            return (int)team; 
        }

        public override int GetMovesetCount()
        {
            return (Manager as FighterManager).entityDefinition.movesets.Count;
        }

        public override HnSF.Combat.MovesetDefinition GetMoveset(int index)
        {
            return (Manager as FighterManager).entityDefinition.movesets[index];
        }

        public float pForcePercentage = 1.0f;
        public override HitReactionBase Hurt(HurtInfoBase hurtInfoBase)
        {
            FighterPhysicsManager physicsManager = (Manager as FighterManager).PhysicsManager;

            HurtInfo2D hurtInfo2D = (HurtInfo2D)hurtInfoBase;
            HitInfo hInfo = hurtInfo2D.hitInfo as HitInfo;

            // Check if the box can hit this entity.
            if(hInfo.groundOnly && !physicsManager.IsGrounded
                || hInfo.airOnly && !physicsManager.IsGrounded)
            {
                return new HitReactionBase();
            }
            // Got hit, apply stun, damage, and forces.
            SetHitStop(hInfo.hitstop);
            SetHitStun(hInfo.hitstun);

            // Convert forces the attacker-based forward direction.
            switch (hInfo.forceType)
            {
                case HitboxForceType.SET:
                    Vector2 baseForce = hInfo.opponentForce;
                    Vector3 forces = new Vector3(baseForce.x * hurtInfo2D.faceDirection, 0, 0);
                    physicsManager.forceGravity.y = baseForce.y;
                    physicsManager.forceMovement = forces;
                    break;
                case HitboxForceType.PULL:
                    Vector2 dir = (Vector2)transform.position - hurtInfo2D.center;
                    if (!hInfo.forceIncludeYForce)
                    {
                        dir.y = 0;
                    }
                    Vector2 forceDir = Vector2.ClampMagnitude((dir) * hInfo.opponentForceMultiplier, hInfo.opponentMaxMagnitude);
                    float yForce = forceDir.y;
                    forceDir.y = 0;
                    if (hInfo.forceIncludeYForce)
                    {
                        physicsManager.forceGravity.y = yForce;
                    }
                    physicsManager.forceMovement = forceDir;
                    break;
            }

            if (hInfo.autoLink)
            {
                physicsManager.forceGravity.y += hurtInfo2D.attackerVelocity.y * pForcePercentage;
                physicsManager.forceMovement.x += hurtInfo2D.attackerVelocity.x * pForcePercentage;
            }

            if (physicsManager.forceGravity.y > 0)
            {
                physicsManager.SetGrounded(false);
            }

            ((FighterManager)Manager).healthManager.Hurt(hInfo.damageOnHit);

            // Change into the correct state.
            if (hInfo.groundBounces && physicsManager.IsGrounded)
            {
                //manager.StateManager.ChangeState((int)EntityStates);
            }else if (hInfo.causesTumble)
            {
                (Manager as FighterManager).StateManager.ChangeState((int)FighterStates.TUMBLE);
            }
            else
            {
                (Manager as FighterManager).StateManager.ChangeState((ushort)(physicsManager.IsGrounded ? FighterStates.FLINCH_GROUND : FighterStates.FLINCH_AIR));
            }
            return new HitReactionBase();
        }

        protected override bool CheckStickDirection(HnSF.Input.InputDefinition sequenceInput, uint framesBack)
        {
            Vector2 stickDir = (Manager as FighterManager).InputManager.GetAxis2D((int)EntityInputs.MOVEMENT, framesBack);
            if (stickDir.magnitude < 0.2f)
            {
                return false;
            }

            if (Vector2.Dot(stickDir, sequenceInput.stickDirection) >= sequenceInput.directionDeviation)
            {
                return true;
            }
            return false;
        }

        protected override bool CheckExecuteInputs(InputSequence sequence, uint baseOffset, ref uint currentOffset)
        {
            for (int e = 0; e < sequence.executeInputs.Count; e++)
            {
                switch (sequence.executeInputs[e].inputType)
                {
                    case HnSF.Input.InputDefinitionType.Stick:
                        if (CheckStickDirection(sequence.executeInputs[e], baseOffset) == false)
                        {
                            return false;
                        }
                        break;
                    case HnSF.Input.InputDefinitionType.Button:
                        if ((Manager as FighterManager).InputManager.GetButton(sequence.executeInputs[e].buttonID, out uint gotOffset, baseOffset,
                            true, sequence.executeWindow).firstPress == false)
                        {
                            return false;
                        }
                        if (gotOffset >= currentOffset)
                        {
                            currentOffset = gotOffset;
                        }
                        break;
                }
            }
            return true;
        }

        protected override bool CheckSequenceInputs(InputSequence sequence, bool holdInput, ref uint currentOffset)
        {
            for (int s = 0; s < sequence.sequenceInputs.Count; s++)
            {
                bool foundInput = false;
                switch (sequence.sequenceInputs[s].inputType)
                {
                    case HnSF.Input.InputDefinitionType.Stick:
                        for (uint f = currentOffset; f < currentOffset + sequence.sequenceWindow; f++)
                        {
                            if (CheckStickDirection(sequence.sequenceInputs[s], f))
                            {
                                foundInput = true;
                                currentOffset = f;
                                break;
                            }
                        }
                        if (foundInput == false)
                        {
                            return false;
                        }
                        break;
                    case HnSF.Input.InputDefinitionType.Button:
                        for (uint f = currentOffset; f < currentOffset + sequence.sequenceWindow; f++)
                        {
                            if ((!holdInput && (Manager as FighterManager).InputManager.GetButton(sequence.sequenceInputs[s].buttonID, out uint gotOffset, f, false).firstPress)
                                || (holdInput && (Manager as FighterManager).InputManager.GetButton(sequence.sequenceInputs[s].buttonID, out uint gotOffsetTwo, f, false).isDown))
                            {
                                foundInput = true;
                                currentOffset = f;
                                break;
                            }
                        }
                        if (foundInput == false)
                        {
                            return false;
                        }
                        break;
                }
            }
            return true;
        }
    }
}