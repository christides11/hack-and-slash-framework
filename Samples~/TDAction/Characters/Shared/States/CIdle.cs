using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

        public override void Initialize()
        {
            base.Initialize();
            CharacterManager c = GetCharacterController();
            c.HurtboxManager.SetHurtboxDefinition(c.entityDefinition.hurtboxDefinitions.FirstOrDefault(x => x.name == "idle").hurtboxDefinition);
        }

        public override void OnUpdate()
        {
            CharacterManager c = GetCharacterController();
            CharacterStats stats = (CharacterStats)c.entityDefinition.GetEntityStats();

            GetPhysicsManager().ApplyMovementFriction(stats.groundFriction);

            CheckInterrupt();

            c.StateManager.IncrementFrame();
            if(c.StateManager.CurrentStateFrame > 60)
            {
                c.StateManager.SetFrame(1);
            }
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
                c.StateManager.ChangeState((int)EntityStates.JUMP_SQUAT);
                return true;
            }
            if (Mathf.Abs(c.InputManager.GetAxis2D((int)EntityInputs.MOVEMENT).x) > InputConstants.moveDeadzone)
            {
                c.StateManager.ChangeState((int)EntityStates.WALK);
                return true;
            }
            return false;
        }

    }
}