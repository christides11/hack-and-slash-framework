using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TDAction.Inputs;
using TDAction.Entities.Characters;

namespace TDAction.Entities
{
    public class CharacterPhysicsManager : EntityPhysicsManager
    {
        public override void ApplyMovement(float accel, float max, float decel)
        {
            //Vector2 movement = controller.InputManager.GetAxis2D((int)CharacterInputs);
            //if()
        }

        public void AirDrift()
        {
            CharacterStats stats = (CharacterStats)((CharacterManager)manager).entityDefinition.GetEntityStats();
            Vector2 movement = manager.InputManager.GetAxis2D((int)EntityInputs.MOVEMENT);
            float tempMax;
            if (Mathf.Abs(movement.x) < InputConstants.moveDeadzone)
            {
                tempMax = 0;
            }
            else
            {
                tempMax = stats.airMaxSpeed * movement.x;
            }

            if ((tempMax < 0 && forceMovement.x < tempMax) 
                || (tempMax > 0 && forceMovement.x > tempMax))
            {
                if (forceMovement.x > 0)
                {
                    forceMovement.x -= stats.aerialFriction;
                    if (forceMovement.x < 0)
                    {
                        forceMovement.x = 0;
                    }
                }
                else
                {
                    forceMovement.x += stats.aerialFriction;
                    if (forceMovement.x > 0)
                    {
                        forceMovement.x = 0;
                    }
                }
            }
            else if (Mathf.Abs(movement.x) > 0.3f &&
              ((tempMax < 0 && forceMovement.x > tempMax) || (tempMax > 0 && forceMovement.x < tempMax)))
            {
                forceMovement.x += (stats.airAcceleration * movement.x)
                    + (Mathf.Sign(movement.x) * stats.airBaseAcceleration);
            }

            if (Mathf.Abs(movement.x) < InputConstants.moveDeadzone)
            {
                if (forceMovement.x > 0)
                {
                    forceMovement.x -= stats.aerialFriction;
                    if (forceMovement.x < 0)
                    {
                        forceMovement.x = 0;
                    }
                }
                else
                {
                    forceMovement.x += stats.aerialFriction;
                    if (forceMovement.x > 0)
                    {
                        forceMovement.x = 0;
                    }
                }
            }
        }
    }
}
