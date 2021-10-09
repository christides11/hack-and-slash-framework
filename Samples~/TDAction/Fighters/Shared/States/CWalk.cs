using System.Collections;
using System.Collections.Generic;
using TDAction.Inputs;
using UnityEngine;

namespace TDAction.Fighter
{
    public class CWalk : FighterState
    {
        public override string GetName()
        {
            return "Walk";
        }

        public override void OnUpdate()
        {
            FighterManager fManager = Manager as FighterManager;
            FighterStatsManager statManager = fManager.statManager;
            FighterPhysicsManager physicsManager = GetPhysicsManager();
            Vector2 movementAxis = fManager.InputManager.GetAxis2D((int)EntityInputs.MOVEMENT);
            float maxSpeed = (statManager.CurrentStats.walkMaxSpeed.GetCurrentValue() * movementAxis.x);

            Vector3 currentSpeed = physicsManager.forceMovement;

            if(Mathf.Abs(physicsManager.forceMovement.x) > Mathf.Abs(maxSpeed))
            {
                physicsManager.ApplyMovementFriction(statManager.CurrentStats.groundFriction.GetCurrentValue());
                currentSpeed = physicsManager.forceMovement;
            }
            else
            {
                float tempAcc = ((statManager.CurrentStats.walkAcceleration.GetCurrentValue() 
                    * Mathf.Abs(movementAxis.x)) + statManager.CurrentStats.walkBaseAcceleration.GetCurrentValue())
                    * fManager.FaceDirection;

                currentSpeed.x += tempAcc;
                if (currentSpeed.x * fManager.FaceDirection > maxSpeed * fManager.FaceDirection)
                {
                    currentSpeed.x = maxSpeed;
                }
            }

            physicsManager.forceMovement = currentSpeed;

            if (movementAxis.x != 0)
            {
                fManager.SetFaceDirection((int)Mathf.Sign(movementAxis.x));
            }

            CheckInterrupt();
            fManager.StateManager.IncrementFrame();
        }

        public override bool CheckInterrupt()
        {
            FighterManager c = Manager as FighterManager;
            if (c.TryAttack())
            {
                return true;
            }
            if (c.InputManager.GetButton((int)EntityInputs.JUMP).firstPress)
            {
                c.StateManager.ChangeState((int)FighterStates.JUMP_SQUAT);
                return true;
            }
            if (c.PhysicsManager.IsGrounded == false)
            {
                c.StateManager.ChangeState((int)FighterStates.FALL);
                return true;
            }
            if (Mathf.Abs(c.InputManager.GetAxis2D((int)EntityInputs.MOVEMENT).x) <= InputConstants.moveDeadzone)
            {
                c.StateManager.ChangeState((int)FighterStates.IDLE);
                return true;
            }
            if (c.InputManager.GetButton((int)EntityInputs.DASH).firstPress)
            {
                c.StateManager.ChangeState((int)FighterStates.RUN);
                return true;
            }
            return false;
        }
    }
}