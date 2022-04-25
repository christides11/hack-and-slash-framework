using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HnSF.Sample.TDAction
{
    public class ADVManager : FighterManager
    {
        private void Start()
        {
            StateManager.ChangeState((int)BaseStateEnum.IDLE);
        }
    }
}