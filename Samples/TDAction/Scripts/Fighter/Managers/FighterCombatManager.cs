﻿using HnSF.Combat;
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
        public override HnSF.Combat.MovesetDefinition CurrentMoveset { get { return (manager as FighterManager).entityDefinition.movesets[currentMoveset]; } }

        public TeamTypes team = TeamTypes.FFA;

        protected override void Awake()
        {
            hitboxManager = new FighterHitboxManager(this, (FighterManager)manager);
        }

        public override int GetTeam()
        {
            return (int)team; 
        }

        public override int GetMovesetCount()
        {
            return (manager as FighterManager).entityDefinition.movesets.Count;
        }

        public override HnSF.Combat.MovesetDefinition GetMoveset(int index)
        {
            return (manager as FighterManager).entityDefinition.movesets[index];
        }

        public override HitReaction Hurt(HurtInfoBase hurtInfoBase)
        {
            FighterPhysicsManager physicsManager = (FighterPhysicsManager)manager.PhysicsManager;

            HurtInfo2D hurtInfo2D = (HurtInfo2D)hurtInfoBase;
            HitInfo hInfo = hurtInfo2D.hitInfo as HitInfo;

            HitReaction hitReaction = new HitReaction();
            hitReaction.reactionType = HitReactionType.Hit;
            // Check if the box can hit this entity.
            if(hInfo.groundOnly && !manager.IsGrounded
                || hInfo.airOnly && !manager.IsGrounded)
            {
                hitReaction.reactionType = HitReactionType.Avoided;
                return hitReaction;
            }
            // Got hit, apply stun, damage, and forces.
            LastHitBy = hInfo;
            SetHitStop(hInfo.hitstop);
            SetHitStun(hInfo.hitstun);

            // Convert forces the attacker-based forward direction.
            switch (hInfo.forceType)
            {
                case HitboxForceType.SET:
                    Vector2 baseForce = hInfo.opponentForceDir * hInfo.opponentForceMagnitude;
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
                    Vector2 forceDir = Vector2.ClampMagnitude((dir) * hInfo.opponentForceMagnitude, hInfo.opponentMaxMagnitude);
                    float yForce = forceDir.y;
                    forceDir.y = 0;
                    if (hInfo.forceIncludeYForce)
                    {
                        physicsManager.forceGravity.y = yForce;
                    }
                    physicsManager.forceMovement = forceDir;
                    break;
            }

            if (physicsManager.forceGravity.y > 0)
            {
                manager.IsGrounded = false;
            }

            ((FighterManager)manager).healthManager.Hurt(hInfo.damageOnHit);

            // Change into the correct state.
            if (hInfo.groundBounces && manager.IsGrounded)
            {
                //manager.StateManager.ChangeState((int)EntityStates);
            }else if (hInfo.causesTumble)
            {
                manager.StateManager.ChangeState((int)FighterStates.TUMBLE);
            }
            else
            {
                manager.StateManager.ChangeState((ushort)(manager.IsGrounded ? FighterStates.FLINCH_GROUND : FighterStates.FLINCH_AIR));
            }
            return hitReaction;
        }

        protected override bool CheckStickDirection(HnSF.Input.InputDefinition sequenceInput, uint framesBack)
        {
            Vector2 stickDir = manager.InputManager.GetAxis2D((int)EntityInputs.MOVEMENT, framesBack);
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
    }
}