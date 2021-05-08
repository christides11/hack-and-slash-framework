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
            FighterManager e = GetEntityManager();

            e.GetPhysicsManager().ApplyMovementFriction(e.statManager.CurrentStats.hitstunFrictionAir.GetCurrentValue());
            e.GetPhysicsManager().HandleGravity();
            e.StateManager.IncrementFrame();

            CheckInterrupt();
        }

        public override bool CheckInterrupt()
        {
            FighterManager e = GetEntityManager();
            // Landed, go into knockdown state.
            if (e.IsGrounded)
            {
                e.StateManager.ChangeState((int)FighterStates.KNOCKDOWN);
                return true;
            }
            if(e.StateManager.CurrentStateFrame < e.CombatManager.HitStun)
            {
                return false;
            }
            // Hitstun finished.
            if (e.IsGrounded)
            {
                e.StateManager.ChangeState((int)FighterStates.IDLE);
            }
            else
            {
                e.StateManager.ChangeState((int)FighterStates.FALL);
            }
            return true;
        }
    }
}