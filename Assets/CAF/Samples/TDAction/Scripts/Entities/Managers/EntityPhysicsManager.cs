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

        public virtual void ApplyGravity()
        {
            EntityStats stats = ((EntityManager)manager).entityDefinition.GetEntityStats();
            HandleGravity(stats.maxFallSpeed, stats.gravity, GravityScale);
        }

        public override void CheckIfGrounded()
        {
            ((EntityManager)manager).IsGrounded = ((EntityManager)manager).charController2D.isGrounded;
        }
    }
}