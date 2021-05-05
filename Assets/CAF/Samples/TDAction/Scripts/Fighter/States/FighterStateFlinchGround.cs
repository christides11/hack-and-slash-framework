using UnityEngine;

namespace TDAction.Fighter
{
    public class FighterStateFlinchGround: FighterState
    {
        public override string GetName()
        {
            return $"Flinch (GROUND)";
        }

        public override void Initialize()
        {
            base.Initialize();
            (Manager as FighterManager).entityAnimator.SetAnimation("hurt");
        }

        public override void OnUpdate()
        {
            FighterManager e = GetEntityManager();

            e.GetPhysicsManager().ApplyMovementFriction(e.statManager.CurrentStats.hitstunFrictionGround.GetCurrentValue());
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
                    e.StateManager.ChangeState((int)FighterStates.IDLE);
                }
                else
                {
                    e.StateManager.ChangeState((int)FighterStates.FALL);
                }
            }
            else if(e.IsGrounded == false)
            {
                e.StateManager.ChangeState((int)FighterStates.FLINCH_AIR, e.StateManager.CurrentStateFrame, false);
                return true;
            }
            return false;
        }
    }
}