using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HnSF.Sample.TDAction.State
{
    public struct ConditionMovementMagnitude : IConditionVariables
    {
        public int FunctionMap => (int)ConditionFunctionEnum.MOVEMENT_MAGNITUDE;

        public float sqrMagnitude;
        public bool inverse;

        public IConditionVariables Copy()
        {
            return new ConditionMovementMagnitude()
            {
                sqrMagnitude = sqrMagnitude,
                inverse = inverse
            };
        }
    }
}