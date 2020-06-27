using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TDAction.Entities.Characters
{
    public class CFall : CharacterState
    {
        public override string GetName()
        {
            return "Fall";
        }
        public override void OnUpdate()
        {
            ((EntityPhysicsManager)GetCharacterController().PhysicsManager).ApplyGravity();

            CheckInterrupt();
        }

        public override bool CheckInterrupt()
        {
            if (GetCharacterController().IsGrounded)
            {
                GetCharacterController().StateManager.ChangeState((int)CharacterStates.IDLE);
                return true;
            }
            return false;
        }
    }
}