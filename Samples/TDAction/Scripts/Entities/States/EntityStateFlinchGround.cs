using UnityEngine;

namespace TDAction.Entities.States
{
    public class EntityStateFlinchGround: EntityState
    {
        public override string GetName()
        {
            return $"Flinch (GROUND)";
        }

        public override void OnUpdate()
        {
            EntityManager e = GetEntityManager();

            e.GetPhysicsManager().ApplyMovementFriction(e.entityDefinition.GetEntityStats().hitstunFrictionGround);
            e.StateManager.IncrementFrame();

            CheckInterrupt();
        }

        public override bool CheckInterrupt()
        {
            EntityManager e = GetEntityManager();
            if (e.CombatManager.HitStun == 0)
            {
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
            else if(!e.IsGrounded)
            {
                e.StateManager.ChangeState((int)EntityStates.FLINCH_AIR, e.StateManager.CurrentStateFrame, false);
                return true;
            }
            return false;
        }
    }
}