using UnityEngine;

namespace TDAction.Entities.States
{
    public class EntityStateFlinchAir : EntityState
    {
        public override string GetName()
        {
            return $"Flinch (AIR)";
        }

        public override void Initialize()
        {
            (Manager as FighterManager).entityAnimator.SetAnimation("hurt");
        }

        public override void OnUpdate()
        {
            FighterManager e = GetEntityManager();

            e.GetPhysicsManager().ApplyMovementFriction(e.entityDefinition.GetEntityStats().hitstunFrictionAir);
            e.GetPhysicsManager().HandleGravity();
            e.StateManager.IncrementFrame();

            float f = (((float)e.StateManager.CurrentStateFrame / (float)e.CombatManager.HitStun) * 10.0f);
            (Manager as FighterManager).entityAnimator.SetFrame((int)f);

            CheckInterrupt();
        }

        public override bool CheckInterrupt()
        {
            FighterManager e = GetEntityManager();
            e.PhysicsManager.CheckIfGrounded();
            if (e.StateManager.CurrentStateFrame >= e.CombatManager.HitStun)
            {
                e.CombatManager.SetHitStun(0);
                // Hitstun finished.
                if (e.IsGrounded)
                {
                    e.StateManager.ChangeState((int)EntityStates.IDLE);
                }
                else
                {
                    e.StateManager.ChangeState((int)EntityStates.FALL);
                }
            }
            else if (e.IsGrounded == true)
            {
                e.StateManager.ChangeState((int)EntityStates.FLINCH_GROUND, e.StateManager.CurrentStateFrame, false);
                return true;
            }
            return false;
        }
    }
}