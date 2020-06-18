using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CAF.Entities
{
    public class EntityPhysicsManager : MonoBehaviour
    {
        public float GravityScale { get; set; } = 1.0f;

        [SerializeField] protected EntityManager controller;

        [Header("Forces")]
        public Vector3 forceMovement;
        public Vector3 forceGravity;
        public Vector3 forceDamage;
        public Vector3 forcePushbox;

        protected float decelerationFactor = 0.97f;

        public virtual void Tick()
        {

        }

        public virtual void SetForceDirect(Vector3 movement, Vector3 gravity)
        {

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
            Vector2 movement = controller.InputManager.GetMovement(0);
            if (movement.magnitude >= InputConstants.movementMagnitude)
            {
                //Translate movment based on "camera."
                Vector3 translatedMovement = controller.lookTransform.TransformDirection(new Vector3(movement.x, 0, movement.y));
                translatedMovement.y = 0;
                translatedMovement *= accel;

                forceMovement += translatedMovement;
                //Limit movement velocity.
                if (forceMovement.magnitude > max * movement.magnitude)
                {
                    forceMovement *= decel;
                }
            }*/
        }

        /// <summary>
        /// Check if we are on the ground.
        /// </summary>
        public virtual void CheckIfGrounded()
        {

        }
    }
}