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
            ((EntityPhysicsManager)GetCharacterController().PhysicsManager).HandleGravity();

            ((CharacterPhysicsManager)GetCharacterController().PhysicsManager).AirDrift();

            CheckInterrupt();
        }

        public override bool CheckInterrupt()
        {
            if (GetCharacterController().IsGrounded)
            {
                GetCharacterController().StateManager.ChangeState((int)EntityStates.IDLE);
                return true;
            }
            return false;
        }
    }
}