using System.Collections;
using System.Collections.Generic;
using TDAction.Inputs;
using UnityEngine;

namespace TDAction.Entities.Characters
{
    public class CEnemyStep : CharacterState
    {
        public override string GetName()
        {
            return "Enemy Step";
        }

        Vector3 storedMovement;
        public override void Initialize()
        {
            base.Initialize();
            CharacterManager c = GetCharacterController();
            EntityPhysicsManager physicsManager = (EntityPhysicsManager)GetPhysicsManager();
            storedMovement = physicsManager.forceMovement;
            physicsManager.forceMovement = Vector3.zero;
            physicsManager.forceGravity = Vector3.zero;
        }

        public override void OnUpdate()
        {
            CharacterManager c = GetCharacterController();
            CharacterStats stats = (CharacterStats)c.entityDefinition.GetEntityStats();

            CheckInterrupt();

            c.StateManager.IncrementFrame();
        }

        public override bool CheckInterrupt()
        {
            CharacterManager c = GetCharacterController();
            EntityPhysicsManager physicsManager = (EntityPhysicsManager)GetPhysicsManager();
            CharacterStats stats = (CharacterStats)c.entityDefinition.GetEntityStats();
            if (c.StateManager.CurrentStateFrame == stats.enemyStepLength)
            {
                c.StateManager.ChangeState((int)EntityStates.AIR_JUMP);
                return true;
            }
            if (c.TryAttack())
            {
                Vector2 movementInput = c.InputManager.GetAxis2D((int)EntityInputs.MOVEMENT);
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