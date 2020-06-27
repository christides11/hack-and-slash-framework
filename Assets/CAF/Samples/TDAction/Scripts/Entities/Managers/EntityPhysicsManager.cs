using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TDAction.Entities
{
    public class EntityPhysicsManager : CAF.Entities.EntityPhysicsManager
    {

        public override void Tick()
        {
            ((EntityManager)controller).charController2D.move(GetOverallForce());
        }

        public virtual void ApplyGravity()
        {
            EntityStats stats = ((EntityManager)controller).entityDefinition.GetEntityStats();
            HandleGravity(stats.maxFallSpeed, stats.gravity, GravityScale);
        }

        public override void CheckIfGrounded()
        {
            ((EntityManager)controller).IsGrounded = ((EntityManager)controller).charController2D.isGrounded;
        }
    }
}