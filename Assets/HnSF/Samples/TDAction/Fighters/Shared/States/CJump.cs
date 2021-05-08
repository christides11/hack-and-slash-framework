using System.Collections;
using System.Collections.Generic;
using TDAction.Inputs;
using UnityEngine;

namespace TDAction.Fighter
{
    public class CJump : FighterState
    {
        public override string GetName()
        {
            return "Jump";
        }

        public override void Initialize()
        {
            FighterManager c = Manager as FighterManager;

            GetPhysicsManager().forceGravity.y = c.statManager.CurrentStats.fullHopForce.GetCurrentValue();
        }

        public override void OnUpdate()
        {
            FighterManager c = Manager as FighterManager;

            ((FighterPhysicsManager)c.PhysicsManager).HandleGravity();

            ((FighterPhysicsManager)c.PhysicsManager).AirDrift();

            CheckInterrupt();
        }

        public override bool CheckInterrupt()
        {
            FighterManager c = Manager as FighterManager;
            if (c.TryAttack())
            {
                return true;
            }
            if (GetPhysicsManager().forceGravity.y <= 0)
            {
                c.StateManager.ChangeState((int)FighterStates.FALL);
                return true;
            }
            return false;
        }
    }
}
