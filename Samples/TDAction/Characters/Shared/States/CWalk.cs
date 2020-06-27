using System.Collections;
using System.Collections.Generic;
using TDAction.Inputs;
using UnityEngine;

namespace TDAction.Entities.Characters
{
    public class CWalk : CharacterState
    {
        public override string GetName()
        {
            return "Walk";
        }

        public override void OnUpdate()
        {
            CharacterManager c = GetCharacterController();
            CharacterStats stats = (CharacterStats)c.entityDefinition.GetEntityStats();
            Vector2 movementAxis = c.InputManager.GetAxis2D((int)EntityInputs.MOVEMENT);
            float maxSpeed = (stats.walkMaxSpeed * movementAxis.x);

            Vector3 currentSpeed = c.PhysicsManager.forceMovement;

            if(Mathf.Abs(c.PhysicsManager.forceMovement.x) > Mathf.Abs(maxSpeed))
            {
                c.PhysicsManager.ApplyMovementFriction(stats.groundFriction);
            }
            else
            {
                float tempAcc = ((stats.walkAcceleration * Mathf.Abs(movementAxis.x)) + stats.walkBaseAcceleration)
                    * c.FaceDirection;

                currentSpeed.x += tempAcc;
                if (currentSpeed.x * c.FaceDirection > maxSpeed * c.FaceDirection)
                {
                    currentSpeed.x = maxSpeed;
                }
            }

            c.PhysicsManager.forceMovement = currentSpeed;

            c.SetFaceDirection((int)Mathf.Sign(currentSpeed.x));

            CheckInterrupt();
        }

        public override bool CheckInterrupt()
        {
            CharacterManager c = GetCharacterController();
            if (Mathf.Abs(c.InputManager.GetAxis2D((int)EntityInputs.MOVEMENT).x) <= 0.2f)
            {
                c.StateManager.ChangeState((int)CharacterStates.IDLE);
                return true;
            }
            return false;
        }
    }
}