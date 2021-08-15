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
            (Manager as FighterManager).entityAnimator.PlayAnimation((Manager as FighterManager).GetAnimationClip("hurt"));
        }

        public override void OnUpdate()
        {
            Manager.PhysicsManager.ApplyMovementFriction(Manager.statManager.CurrentStats.hitstunFrictionGround.GetCurrentValue());
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
            else if(Manager.PhysicsManager.IsGrounded == false)
            {
                Manager.StateManager.ChangeState((int)FighterStates.FLINCH_AIR, Manager.StateManager.CurrentStateFrame, false);
                return true;
            }
            return false;
        }
    }
}