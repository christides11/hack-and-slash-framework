using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HnSF.Sample.TDAction
{
    public class ADVManager : FighterManager
    {
        public FighterDefinition definition;

        public override void SimAwake()
        {
            //StateManager.ChangeState((int)FighterStateEnum.IDLE);
        }
    }
}