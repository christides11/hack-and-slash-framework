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
                stateManager.AddState(v.timeline, (int)v.state);
            }

            stateManager.ChangeState((int)FighterStateEnum.IDLE);
        }
    }
}