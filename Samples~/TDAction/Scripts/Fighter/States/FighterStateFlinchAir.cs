using UnityEngine;

namespace TDAction.Fighter
{
    public class FighterStateFlinchAir : FighterState
    {
        public override string GetName()
        {
            return $"Flinch (AIR)";
        }

        public override void Initialize()
        {
            (Manager as FighterManager).entityAnimator.PlayAnimation((Manager as FighterManager).GetAnimationClip("hurt"));
        }

        public override void OnUpdate()
        {
            FighterManager e = GetEntityManager();

            e.GetPhysicsManager().ApplyMovementFriction(e.statManager.CurrentStats.hitstunFrictionAir.GetCurrentValue());
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
                if (e.PhysicsManager.IsGrounded)
                {
                    e.StateManager.ChangeState((int)FighterStates.IDLE);
                }
                else
                {
                    e.StateManager.ChangeState((int)FighterStates.FALL);
                }
            }
            else if (e.PhysicsManager.IsGrounded == true)
            {
                e.StateManager.ChangeState((int)FighterStates.FLINCH_GROUND, e.StateManager.CurrentStateFrame, false);
                return true;
            }
            return false;
        }
    }
}