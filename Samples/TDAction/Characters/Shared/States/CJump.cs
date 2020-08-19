using System.Collections;
using System.Collections.Generic;
using TDAction.Inputs;
using UnityEngine;

namespace TDAction.Entities.Characters
{
    public class CJump : CharacterState
    {
        public override string GetName()
        {
            return "Jump";
        }

        public override void Initialize()
        {
            CharacterManager c = GetCharacterController();
            CharacterStats stats = (CharacterStats)c.entityDefinition.GetEntityStats();

            c.PhysicsManager.forceGravity.y = stats.fullHopForce;
        }

        public override void OnUpdate()
        {
            CharacterManager c = GetCharacterController();

            ((EntityPhysicsManager)c.PhysicsManager).HandleGravity();

            ((CharacterPhysicsManager)c.PhysicsManager).AirDrift();

            CheckInterrupt();
        }

        public override bool CheckInterrupt()
        {
            CharacterManager c = GetCharacterController();
            if (c.TryAttack())
            {
                return true;
            }
            if (c.PhysicsManager.forceGravity.y <= 0)
            {
                c.StateManager.ChangeState((int)EntityStates.FALL);
                return true;
            }
            return false;
        }
    }
}
