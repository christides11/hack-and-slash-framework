using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TDAction.Inputs;
using UnityEngine;

namespace TDAction.Fighter
{
    public class CIdle : FighterState
    {
        public override string GetName()
        {
            return "Idle";
        }

        public override void Initialize()
        {
            base.Initialize();
            FighterManager c = Manager as FighterManager;
            c.HurtboxManager.SetHurtboxDefinition(c.entityDefinition.hurtboxDefinitions.FirstOrDefault(x => x.name == "idle").hurtboxDefinition);
        }

        public override void OnUpdate()
        {
            FighterManager c = Manager as FighterManager;

            GetPhysicsManager().ApplyMovementFriction(c.statManager.CurrentStats.groundFriction.GetCurrentValue());

            CheckInterrupt();
            c.StateManager.IncrementFrame();
        }

        public override bool CheckInterrupt()
        {
            FighterManager fManager = Manager as FighterManager;
            if (fManager.TryAttack())
            {
                return true;
            }
            if (fManager.InputManager.GetButton((int)EntityInputs.JUMP).firstPress)
            {
                fManager.StateManager.ChangeState((int)FighterStates.JUMP_SQUAT);
                return true;
            }
            if (Mathf.Abs(fManager.InputManager.GetAxis2D((int)EntityInputs.MOVEMENT).x) > InputConstants.moveDeadzone)
            {
                fManager.StateManager.ChangeState((int)FighterStates.WALK);
                return true;
            }
            return false;
        }

    }
}