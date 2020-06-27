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
            if (Mathf.Abs(GetCharacterController().InputManager.GetAxis2D((int)EntityInputs.MOVEMENT).x) > 0.2f)
            {
                GetCharacterController().StateManager.ChangeState((int)CharacterStates.WALK);
                return true;
            }
            return false;
        }

    }
}