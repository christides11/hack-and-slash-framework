using System.Collections;
using System.Collections.Generic;
using TDAction.Inputs;
using UnityEngine;

namespace TDAction.Fighter
{
    public class CEnemyStep : FighterState
    {
        public override string GetName()
        {
            return "Enemy Step";
        }

        Vector3 storedMovement;
        public override void Initialize()
        {
            base.Initialize();
            FighterPhysicsManager physicsManager = (FighterPhysicsManager)GetPhysicsManager();
            storedMovement = physicsManager.forceMovement;
            physicsManager.forceMovement = Vector3.zero;
            physicsManager.forceGravity = Vector3.zero;
        }

        public override void OnUpdate()
        {
            CheckInterrupt();

            Manager.StateManager.IncrementFrame();
        }

        public override bool CheckInterrupt()
        {
            FighterManager fManager = Manager as FighterManager;
            FighterPhysicsManager physicsManager = (FighterPhysicsManager)GetPhysicsManager();
            if (fManager.StateManager.CurrentStateFrame == fManager.statManager.CurrentStats.enemyStepLength)
            {
                fManager.StateManager.ChangeState((int)FighterStates.AIR_JUMP);
                return true;
            }
            if (fManager.TryAttack())
            {
                Vector2 movementInput = fManager.InputManager.GetAxis2D((int)EntityInputs.MOVEMENT);
                if (movementInput.magnitude > InputConstants.moveDeadzone)
                {
                    physicsManager.RedirectInertia(storedMovement.x, movementInput);
                }
                else
                {
                    physicsManager.forceMovement = storedMovement;
                }
                return true;
            }
            return false;
        }

    }
}