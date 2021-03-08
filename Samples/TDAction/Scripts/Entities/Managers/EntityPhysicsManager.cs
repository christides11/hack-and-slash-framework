using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TDAction.Entities
{
    public class EntityPhysicsManager : CAF.Fighters.FighterPhysicsManager2D
    {

        public override void Tick()
        {
            ((FighterManager)manager).charController2D.move(GetOverallForce());
        }

        public override void Freeze()
        {
            ((FighterManager)manager).charController2D.move(Vector3.zero);
        }

        /// <summary>
        /// Create a force based on the parameters given and
        /// adds it to our movement force.
        /// </summary>
        /// <param name="accel">How fast the entity accelerates in the movement direction.</param>
        /// <param name="max">The max magnitude of our movement force.</param>
        /// <param name="decel">How much the entity decelerates when moving faster than the max magnitude.
        /// 1.0 = doesn't decelerate, 0.0 = force set to 0.</param>
        public virtual void ApplyMovement(float accel, float max, float decel)
        {/*
          * 
          */
        }

        public override void CheckIfGrounded()
        {
            ((FighterManager)manager).IsGrounded = ((FighterManager)manager).charController2D.isGrounded;
        }

        public virtual void HandleGravity()
        {
            FighterManager m = ((FighterManager)manager);
            EntityStats stats = m.entityDefinition.GetEntityStats();
            float maxFallSpeed = m.CombatManager.HitStun > 0 ? stats.hitstunMaxFallSpeed : stats.maxFallSpeed;
            float gravity = m.CombatManager.HitStun > 0 ?  stats.hitstunGravity : stats.gravity;
            HandleGravity(maxFallSpeed, gravity, GravityScale);
        }
    }
}