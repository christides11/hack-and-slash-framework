using System.Collections;
using System.Collections.Generic;
using TDAction.Inputs;
using UnityEngine;

namespace TDAction.Fighter
{
    public class CJumpSquat : FighterState
    {
        public override void Initialize()
        {
            base.Initialize();
            (Manager as FighterManager).entityAnimator.SetAnimation("jumpsquat");
        }

        public override void OnUpdate()
        {
            Manager.StateManager.IncrementFrame();

            CheckInterrupt();
        }

        public override bool CheckInterrupt()
        {
            FighterManager c = Manager as FighterManager;
            if (c.StateManager.CurrentStateFrame >= c.statManager.CurrentStats.jumpSquatFrames)
            {
                float xMove = c.InputManager.GetAxis2D((int)EntityInputs.MOVEMENT).x;
                if (xMove != 0)
                {
                    c.SetFaceDirection((int)Mathf.Sign(xMove));
                }
                c.StateManager.ChangeState((int)FighterStates.JUMP);
                return true;
            }
            return false;
        }
    }
}