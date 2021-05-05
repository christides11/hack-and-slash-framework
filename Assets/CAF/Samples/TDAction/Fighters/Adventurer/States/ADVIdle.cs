using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TDAction.Fighter
{
    public class ADVIdle : CIdle
    {
        public override void Initialize()
        {
            base.Initialize();
            (Manager as FighterManager).entityAnimator.SetAnimation("idle");
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            if((int)Manager.StateManager.CurrentStateFrame > 60)
            {
                Manager.StateManager.SetFrame(0);
            }
            (Manager as FighterManager).entityAnimator.SetFrame((int)Manager.StateManager.CurrentStateFrame);
        }
    }
}