using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HnSF.Sample.TDAction
{
    public class ADVManager : FighterManager
    {
        public override void Start()
        {
            base.Start();
            StateManager.ChangeState((int)BaseStateEnum.IDLE);
        }
    }
}