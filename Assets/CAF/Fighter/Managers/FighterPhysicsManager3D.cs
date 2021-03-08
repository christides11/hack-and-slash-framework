using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CAF.Fighters
{
    public class FighterPhysicsManager3D : FighterPhysicsManagerBase
    {
        public float GravityScale { get; set; } = 1.0f;

        protected float decelerationFactor = 0.97f;

        [Header("Forces")]
        public Vector3 forceMovement;
        public Vector3 forceGravity;
        public Vector3 forceDamage;
        public Vector3 forcePushbox;

        public override void ResetForces()
        {
            forceMovement = Vector3.zero;
            forceGravity = Vector3.zero;
            forceDamage = Vector3.zero;
            forcePushbox = Vector3.zero;
        }

        public virtual Vector3 GetOverallForce()
        {
            return forceMovement + forceGravity + forceDamage;
        }

        public virtual void HandleGravity(float maxFallSpeed, float gravity, float gravityScale)
        {
            if (forceGravity.y > -(maxFallSpeed))
            {
                forceGravity.y -= gravity * gravityScale;
                if (forceGravity.y < -(maxFallSpeed))
                {
                    forceGravity.y = -maxFallSpeed;
                }
            }
            else if (forceGravity.y < -(maxFallSpeed))
            {
                forceGravity.y *= decelerationFactor;
            }
        }

        public virtual void ApplyMovementFriction(float friction = -1)
        {
            if (friction == -1)
            {
                friction = 1;
            }
            Vector3 realFriction = forceMovement.normalized * friction;
            forceMovement.x = ApplyFriction(forceMovement.x, Mathf.Abs(realFriction.x));
            forceMovement.z = ApplyFriction(forceMovement.z, Mathf.Abs(realFriction.z));
        }

        public virtual void ApplyGravityFriction(float friction)
        {
            forceGravity.y = ApplyFriction(forceGravity.y, friction);
        }

        /// <summary>
        /// Applies friction on the given value based on the traction given.
        /// </summary>
        /// <param name="value">The value to apply traction to.</param>
        /// <param name="traction">The traction to apply.</param>
        /// <returns>The new value with the traction applied.</returns>
        protected virtual float ApplyFriction(float value, float traction)
        {
            if (value > 0)
            {
                value -= traction;
                if (value < 0)
                {
                    value = 0;
                }
            }
            else if (value < 0)
            {
                value += traction;
                if (value > 0)
                {
                    value = 0;
                }
            }
            return value;
        }

        public virtual void RedirectInertia(Vector3 forceMovement, Vector2 movementInput)
        {
            movementInput.Normalize();
            Vector3 redirectedMovement = forceMovement.magnitude
                * manager.GetMovementVector(movementInput.x, movementInput.y);
            this.forceMovement = redirectedMovement;
        }
    }
}