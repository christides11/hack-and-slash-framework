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

            (Manager as FighterManager).entityAnimator.PlayAnimation((Manager as FighterManager).GetAnimationClip("jump-init")).onEnd += () => 
            {
                (Manager as FighterManager).entityAnimator.PlayAnimation((Manager as FighterManager).GetAnimationClip("jump"));
            };
        }

        public override void OnUpdate()
        {
            FighterManager c = Manager as FighterManager;

            ((FighterPhysicsManager)c.PhysicsManager).HandleGravity();

            ((FighterPhysicsManager)c.PhysicsManager).AirDrift();

            (Manager as FighterManager).entityAnimator.SetFrame((int)c.StateManager.CurrentStateFrame);
            c.StateManager.IncrementFrame();

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
