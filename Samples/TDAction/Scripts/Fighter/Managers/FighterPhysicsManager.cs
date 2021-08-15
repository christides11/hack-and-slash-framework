using System.Collections;
using System.Collections.Generic;
using TDAction.Inputs;
using UnityEngine;
using HnSF.Fighters;

namespace TDAction.Fighter
{
    public class FighterPhysicsManager : MonoBehaviour, IFighterPhysicsManager
    {
        public bool IsGrounded { get; protected set; } = false;
        public float GravityScale { get; set; } = 1.0f;

        protected float decelerationFactor = 0.97f;


        [SerializeField] protected FighterManager manager;

        [Header("Forces")]
        public Vector2 forceMovement;
        public Vector2 forceGravity;
        public Vector2 forceDamage;
        public Vector2 forcePushbox;

        public virtual void ResetForces()
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

        public virtual void SetGrounded(bool value)
        {
            IsGrounded = value;
        }

        public virtual void Tick()
        {
            manager.charController2D.move(GetOverallForce());
        }

        public virtual void Freeze()
        {
            manager.charController2D.move(Vector3.zero);
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

        public virtual void CheckIfGrounded()
        {
            IsGrounded = ((FighterManager)manager).charController2D.isGrounded;
        }

        public virtual void HandleGravity()
        {
            FighterManager m = ((FighterManager)manager);
            float maxFallSpeed = m.CombatManager.HitStun > 0 ? m.statManager.CurrentStats.hitstunMaxFallSpeed.GetCurrentValue() 
                : m.statManager.CurrentStats.maxFallSpeed.GetCurrentValue();
            float gravity = m.CombatManager.HitStun > 0 ?  m.statManager.CurrentStats.hitstunGravity.GetCurrentValue() 
                : m.statManager.CurrentStats.gravity.GetCurrentValue();
            HandleGravity(maxFallSpeed, gravity, GravityScale);
        }

        public void AirDrift()
        {
            FighterStatsManager statsManager = (manager as FighterManager).statManager;
            Vector2 movement = (manager as FighterManager).InputManager.GetAxis2D((int)EntityInputs.MOVEMENT);
            float tempMax;
            if (Mathf.Abs(movement.x) < InputConstants.moveDeadzone)
            {
                tempMax = 0;
            }
            else
            {
                tempMax = statsManager.CurrentStats.airMaxSpeed.GetCurrentValue() * movement.x;
            }

            if ((tempMax < 0 && forceMovement.x < tempMax)
                || (tempMax > 0 && forceMovement.x > tempMax))
            {
                if (forceMovement.x > 0)
                {
                    forceMovement.x -= statsManager.CurrentStats.aerialFriction.GetCurrentValue();
                    if (forceMovement.x < 0)
                    {
                        forceMovement.x = 0;
                    }
                }
                else
                {
                    forceMovement.x += statsManager.CurrentStats.aerialFriction.GetCurrentValue();
                    if (forceMovement.x > 0)
                    {
                        forceMovement.x = 0;
                    }
                }
            }
            else if (Mathf.Abs(movement.x) > 0.3f &&
              ((tempMax < 0 && forceMovement.x > tempMax) || (tempMax > 0 && forceMovement.x < tempMax)))
            {
                forceMovement.x += (statsManager.CurrentStats.airAcceleration.GetCurrentValue() * movement.x)
                    + (Mathf.Sign(movement.x) * statsManager.CurrentStats.airBaseAcceleration.GetCurrentValue());
            }

            if (Mathf.Abs(movement.x) < InputConstants.moveDeadzone)
            {
                if (forceMovement.x > 0)
                {
                    forceMovement.x -= statsManager.CurrentStats.aerialFriction.GetCurrentValue();
                    if (forceMovement.x < 0)
                    {
                        forceMovement.x = 0;
                    }
                }
                else
                {
                    forceMovement.x += statsManager.CurrentStats.aerialFriction.GetCurrentValue();
                    if (forceMovement.x > 0)
                    {
                        forceMovement.x = 0;
                    }
                }
            }
        }
    }
}