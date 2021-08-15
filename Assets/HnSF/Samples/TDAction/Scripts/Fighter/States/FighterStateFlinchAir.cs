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
            Manager.GetPhysicsManager().ApplyMovementFriction(Manager.statManager.CurrentStats.hitstunFrictionAir.GetCurrentValue());
            Manager.GetPhysicsManager().HandleGravity();
            Manager.StateManager.IncrementFrame();

            float f = (((float)Manager.StateManager.CurrentStateFrame / (float)Manager.CombatManager.HitStun) * 10.0f);
            (Manager as FighterManager).entityAnimator.SetFrame((int)f);

            CheckInterrupt();
        }

        public override bool CheckInterrupt()
        {
            Manager.PhysicsManager.CheckIfGrounded();
            if (Manager.StateManager.CurrentStateFrame >= Manager.CombatManager.HitStun)
            {
                Manager.CombatManager.SetHitStun(0);
                // Hitstun finished.
                if (Manager.PhysicsManager.IsGrounded)
                {
                    Manager.StateManager.ChangeState((int)FighterStates.IDLE);
                }
                else
                {
                    Manager.StateManager.ChangeState((int)FighterStates.FALL);
                }
            }
            else if (Manager.PhysicsManager.IsGrounded == true)
            {
                Manager.StateManager.ChangeState((int)FighterStates.FLINCH_GROUND, Manager.StateManager.CurrentStateFrame, false);
                return true;
            }
            return false;
        }
    }
}