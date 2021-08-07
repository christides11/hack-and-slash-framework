using System.Collections;
using System.Collections.Generic;
using TDAction.Inputs;
using UnityEngine;

namespace TDAction.Fighter
{
    public class FighterPhysicsManager : HnSF.Fighters.FighterPhysicsManager2D
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