using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HnSF.Sample.TDAction
{
    public class ADVManager : FighterManager
    {
        public EnumStateBinding[] coreStates;

        public override void SimAwake()
        {
            foreach (var v in coreStates)
            {
                StateManager.AddState(v.timeline, (int)v.state);
            }

            StateManager.ChangeState((int)FighterStateEnum.IDLE);
        }
    }
}