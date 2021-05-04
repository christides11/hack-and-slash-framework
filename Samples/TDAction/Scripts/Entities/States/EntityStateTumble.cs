using System;
using System.Collections;
using System.Collections.Generic;
using TDAction.Combat;
using TDAction.Inputs;
using UnityEngine;

namespace TDAction.Entities.States
{
    public class EntityStateTumble : EntityState
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

            e.GetPhysicsManager().ApplyMovementFriction(e.entityDefinition.GetEntityStats().hitstunFrictionAir);
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
                e.StateManager.ChangeState((int)EntityStates.KNOCKDOWN);
                return true;
            }
            if(e.StateManager.CurrentStateFrame < e.CombatManager.HitStun)
            {
                return false;
            }
            // Hitstun finished.
            if (e.IsGrounded)
            {
                e.StateManager.ChangeState((int)EntityStates.IDLE);
            }
            else
            {
                e.StateManager.ChangeState((int)EntityStates.FALL);
            }
            return true;
        }
    }
}