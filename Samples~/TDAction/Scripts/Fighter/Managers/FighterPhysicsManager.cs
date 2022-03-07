using System.Collections;
using System.Collections.Generic;
using HnSF.Fighters;
using UnityEngine;

namespace HnSF.Sample.TDAction
{
    public class FighterPhysicsManager : MonoBehaviour, IFighterPhysicsManager
    {
        private const float decelerationFactor = 0.97f;
        public bool IsGrounded { get; private set; } = false;
        public Vector2 forceMovement { get; set; } = Vector2.zero;
        public Vector2 forceDamage { get; set; } = Vector2.zero;

        public BoxCollider2D worldCollider;
        public CharacterController2D cc;

        public void Tick()
        {
            cc.move(GetOverallForce());
        }
        
        public virtual Vector2 GetOverallForce()
        {
            return forceMovement + forceDamage;
        }

        public void Freeze()
        {
            cc.move(Vector2.zero);
        }

        public void ResetForces()
        {
            forceMovement = Vector2.zero;
            forceDamage = Vector2.zero;
        }

        public void CheckIfGrounded()
        {
            IsGrounded = cc.isGrounded;
        }

        public void SetGrounded(bool value)
        {
            IsGrounded = value;
        }
        
        public virtual void ApplyGravity()
        {
            /*
            FighterManager m = ((FighterManager)manager);
            float maxFallSpeed = m.CombatManager.HitStun > 0 ? m.statManager.CurrentStats.hitstunMaxFallSpeed.GetCurrentValue() 
                : m.statManager.CurrentStats.maxFallSpeed.GetCurrentValue();
            float gravity = m.CombatManager.HitStun > 0 ?  m.statManager.CurrentStats.hitstunGravity.GetCurrentValue() 
                : m.statManager.CurrentStats.gravity.GetCurrentValue();
            HandleGravity(maxFallSpeed, gravity, GravityScale);*/
        }
        
        public virtual void ApplyGravity(float maxFallSpeed, float gravity, float gravityScale)
        {
            Vector2 tempMovement = this.forceMovement;
            if (tempMovement.y > -(maxFallSpeed))
            {
                tempMovement.y -= gravity * gravityScale;
                if (tempMovement.y < -(maxFallSpeed))
                {
                    tempMovement.y = -maxFallSpeed;
                }
            }
            else if (tempMovement.y < -(maxFallSpeed))
            {
                tempMovement.y *= decelerationFactor;
            }

            this.forceMovement = tempMovement;
        }
        
        public virtual void ApplyMovementFriction(float friction = 1)
        {
            Vector2 tempMovement = forceMovement;
            tempMovement.x = ApplyFriction(forceMovement.x, Mathf.Abs(friction));
            forceMovement = tempMovement;
        }
        
        protected virtual float ApplyFriction(float value, float friction)
        {
            if (value > 0)
            {
                value -= friction;
                if (value < 0)
                {
                    value = 0;
                }
            }
            else if (value < 0)
            {
                value += friction;
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
            var tmpMovement = this.forceMovement;
            tmpMovement.x = redirectedMovement;
            forceMovement = tmpMovement;
        }
    }
}