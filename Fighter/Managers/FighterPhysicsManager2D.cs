using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CAF.Fighters
{
    public class FighterPhysicsManager2D : FighterPhysicsManagerBase
    {
        public float GravityScale { get; set; } = 1.0f;

        protected float decelerationFactor = 0.97f;

        [Header("Forces")]
        public Vector2 forceMovement;
        public Vector2 forceGravity;
        public Vector2 forceDamage;
        public Vector2 forcePushbox;

        public override void ResetForces()
        {
            forceMovement = Vector2.zero;
            forceGravity = Vector2.zero;
            forceDamage = Vector2.zero;
            forcePushbox = Vector2.zero;
        }

        public virtual Vector2 GetOverallForce()
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

        public virtual void RedirectInertia(float forceMovementX, Vector2 movementInput)
        {
            float redirectedMovement = Mathf.Sign(movementInput.x)
                * forceMovementX;
            this.forceMovement.x = redirectedMovement;
        }
    }
}