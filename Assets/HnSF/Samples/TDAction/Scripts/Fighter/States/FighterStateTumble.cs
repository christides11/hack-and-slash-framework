using System;
using System.Collections;
using System.Collections.Generic;
using TDAction.Combat;
using TDAction.Inputs;
using UnityEngine;

namespace TDAction.Fighter
{
    public class FighterStateTumble : FighterState
    {
        public override string GetName()
        {
            return $"Tumble";
        }

        public override void Initialize()
        {

        }

        public override void OnUpdate()
        {
            Manager.GetPhysicsManager().ApplyMovementFriction(Manager.statManager.CurrentStats.hitstunFrictionAir.GetCurrentValue());
            Manager.GetPhysicsManager().HandleGravity();
            Manager.StateManager.IncrementFrame();

            CheckInterrupt();
        }

        public override bool CheckInterrupt()
        {
            // Landed, go into knockdown state.
            if (Manager.PhysicsManager.IsGrounded)
            {
                Manager.StateManager.ChangeState((int)FighterStates.KNOCKDOWN);
                return true;
            }
            if(Manager.StateManager.CurrentStateFrame < Manager.CombatManager.HitStun)
            {
                return false;
            }
            // Hitstun finished.
            if (Manager.PhysicsManager.IsGrounded)
            {
                Manager.StateManager.ChangeState((int)FighterStates.IDLE);
            }
            else
            {
                Manager.StateManager.ChangeState((int)FighterStates.FALL);
            }
            return true;
        }
    }
}