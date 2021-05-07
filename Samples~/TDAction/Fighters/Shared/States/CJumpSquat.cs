using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TDAction.Fighter
{
    public class CJumpSquat : FighterState
    {

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
                c.StateManager.ChangeState((int)FighterStates.JUMP);
                return true;
            }
            return false;
        }
    }
}