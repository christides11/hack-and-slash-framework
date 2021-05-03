using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TDAction.Entities.Characters.Boxer
{
    public class BIdle : CIdle
    {
        public override void Initialize()
        {
            base.Initialize();
            (Manager as FighterManager).entityAnimator.SetAnimation("idle");
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            if((int)GetCharacterController().StateManager.CurrentStateFrame > 60)
            {
                GetCharacterController().StateManager.SetFrame(0);
            }
            (Manager as FighterManager).entityAnimator.SetFrame((int)GetCharacterController().StateManager.CurrentStateFrame);
        }
    }
}