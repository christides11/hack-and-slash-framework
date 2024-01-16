using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HnSF.Sample.TDAction.State
{
    public struct ConditionNone : IConditionVariables
    {
        public int FunctionMap => (int)ConditionFunctionEnum.NONE;

        public void SetupDefaults()
        {
            
        }

        public IConditionVariables Copy()
        {
            return new ConditionNone();
        }
    }
}