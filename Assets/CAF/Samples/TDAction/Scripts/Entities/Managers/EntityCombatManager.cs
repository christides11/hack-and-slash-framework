using CAF.Combat;
using System.Collections;
using System.Collections.Generic;
using TDAction.Combat;
using TDAction.Inputs;
using UnityEngine;

namespace TDAction.Entities
{
    public class EntityCombatManager : CAF.Entities.EntityCombatManager
    {
        public TeamTypes team = TeamTypes.FFA;

        protected override void Awake()
        {
            hitboxManager = new EntityHitboxManager(this, (EntityManager)manager);
        }

        public override int GetTeam()
        {
            return (int)team; 
        }

        public override HitReaction Hurt(HurtInfoBase hurtInfoBase)
        {
            EntityPhysicsManager physicsManager = (EntityPhysicsManager)manager.PhysicsManager;

            HurtInfo2D hurtInfo2D = (HurtInfo2D)hurtInfoBase;
            HitInfo hInfo = (HitInfo)hurtInfo2D.hitInfo;

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
                    forces.y = baseForce.y;
                    physicsManager.forceGravity.y = baseForce.y;
                    forces.y = 0;
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

            ((EntityManager)manager).healthManager.Hurt(hInfo.damageOnHit);

            // Change into the correct state.
            if (hInfo.groundBounces && manager.IsGrounded)
            {
                //manager.StateManager.ChangeState((int)EntityStates);
            }else if (hInfo.causesTumble)
            {
                manager.StateManager.ChangeState((int)EntityStates.TUMBLE);
            }
            else
            {
                manager.StateManager.ChangeState((int)(manager.IsGrounded ? EntityStates.FLINCH_GROUND : EntityStates.FLINCH_AIR));
            }
            return hitReaction;
        }

        protected override bool CheckStickDirection(CAF.Input.InputDefinition sequenceInput, int framesBack)
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