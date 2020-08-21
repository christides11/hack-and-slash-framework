using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TDAction.Entities
{
    public class EntityPhysicsManager : CAF.Entities.EntityPhysicsManager
    {

        public override void Tick()
        {
            ((EntityManager)manager).charController2D.move(GetOverallForce());
        }

        public override void CheckIfGrounded()
        {
            ((EntityManager)manager).IsGrounded = ((EntityManager)manager).charController2D.isGrounded;
        }

        public virtual void HandleGravity()
        {
            EntityManager m = ((EntityManager)manager);
            EntityStats stats = m.entityDefinition.GetEntityStats();
            float maxFallSpeed = m.CombatManager.HitStun > 0 ? stats.hitstunMaxFallSpeed : stats.maxFallSpeed;
            float gravity = m.CombatManager.HitStun > 0 ?  stats.hitstunGravity : stats.gravity;
            HandleGravity(maxFallSpeed, gravity, GravityScale);
        }
    }
}