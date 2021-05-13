using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TDAction.Fighter
{
    public class ADVWalk : CWalk
    {
        public override void Initialize()
        {
            base.Initialize();
            (Manager as FighterManager).entityAnimator.PlayAnimation((Manager as FighterManager).GetAnimationClip("walk", Manager.CombatManager.CurrentMovesetIdentifier));
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            (Manager as FighterManager).entityAnimator.SetFrame((int)Manager.StateManager.CurrentStateFrame);
        }
    }
}