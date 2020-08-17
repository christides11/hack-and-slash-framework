using System.Collections;
using System.Collections.Generic;
using TDAction.Inputs;
using UnityEngine;

namespace TDAction.Entities.Characters
{
    public class CIdle : CharacterState
    {
        public override string GetName()
        {
            return "Idle";
        }

        public override void OnUpdate()
        {
            CharacterManager c = GetCharacterController();
            CharacterStats stats = (CharacterStats)c.entityDefinition.GetEntityStats();

            c.PhysicsManager.ApplyMovementFriction(stats.groundFriction);

            CheckInterrupt();
        }

        public override bool CheckInterrupt()
        {
            CharacterManager c = GetCharacterController();
            CharacterStats stats = (CharacterStats)c.entityDefinition.GetEntityStats();
            if (c.TryAttack())
            {
                return true;
            }
            if (c.InputManager.GetButton((int)EntityInputs.JUMP).firstPress)
            {
                c.StateManager.ChangeState((int)CharacterStates.JUMP_SQUAT);
                return true;
            }
            if (Mathf.Abs(c.InputManager.GetAxis2D((int)EntityInputs.MOVEMENT).x) > InputConstants.moveDeadzone)
            {
                c.StateManager.ChangeState((int)CharacterStates.WALK);
                return true;
            }
            return false;
        }

    }
}