using System.Collections;
using System.Collections.Generic;
using TDAction.Inputs;
using UnityEngine;

namespace TDAction.Fighter
{
    public class CRun : FighterState
    {
        public override string GetName()
        {
            return "Run";
        }

        public override void Initialize()
        {
            base.Initialize();
            (Manager as FighterManager).entityAnimator.SetAnimation("run");
        }

        public override void OnUpdate()
        {
            FighterManager c = Manager as FighterManager;
            FighterPhysicsManager physicsManager = GetPhysicsManager();
            Vector2 movementAxis = c.InputManager.GetAxis2D((int)EntityInputs.MOVEMENT);
            float maxSpeed = (c.statManager.CurrentStats.runMaxSpeed.GetCurrentValue() * movementAxis.x);

            Vector3 currentSpeed = physicsManager.forceMovement;

            if (Mathf.Abs(physicsManager.forceMovement.x) > Mathf.Abs(maxSpeed))
            {
                physicsManager.ApplyMovementFriction(c.statManager.CurrentStats.groundFriction.GetCurrentValue());
            }
            else
            {
                float tempAcc = ((c.statManager.CurrentStats.runAcceleration.GetCurrentValue() * Mathf.Abs(movementAxis.x))
                    + c.statManager.CurrentStats.runBaseAcceleration.GetCurrentValue())
                    * c.FaceDirection;

                currentSpeed.x += tempAcc;
                if (currentSpeed.x * c.FaceDirection > maxSpeed * c.FaceDirection)
                {
                    currentSpeed.x = maxSpeed;
                }
            }

            physicsManager.forceMovement = currentSpeed;

            if (movementAxis.magnitude >= InputConstants.moveDeadzone)
            {
                c.SetFaceDirection((int)Mathf.Sign(movementAxis.x));
            }

            if(CheckInterrupt() == false)
            {
                c.StateManager.IncrementFrame();
                (Manager as FighterManager).entityAnimator.SetFrame((int)Manager.StateManager.CurrentStateFrame);
            }
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
            if (Mathf.Abs(c.InputManager.GetAxis2D((int)EntityInputs.MOVEMENT).x) <= InputConstants.moveDeadzone)
            {
                c.StateManager.ChangeState((int)FighterStates.IDLE);
                return true;
            }
            return false;
        }
    }
}
