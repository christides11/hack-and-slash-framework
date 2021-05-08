using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TDAction.Fighter
{
    public class CFall : FighterState
    {
        public override string GetName()
        {
            return "Fall";
        }
        public override void OnUpdate()
        {
            ((FighterPhysicsManager)Manager.PhysicsManager).HandleGravity();

            ((FighterPhysicsManager)Manager.PhysicsManager).AirDrift();

            CheckInterrupt();
        }

        public override bool CheckInterrupt()
        {
            FighterManager c = Manager as FighterManager;
            if (c.TryAttack())
            {
                return true;
            }
            if (Manager.IsGrounded)
            {
                Manager.StateManager.ChangeState((int)FighterStates.IDLE);
                return true;
            }
            return false;
        }
    }
}