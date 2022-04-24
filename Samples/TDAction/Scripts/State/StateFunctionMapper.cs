using System;
using System.Collections;
using System.Collections.Generic;
using HnSF.Fighters;
using UnityEngine;

namespace HnSF.Sample.TDAction.State
{
    public class StateFunctionMapper
    {
        public Dictionary<int, Action<IFighterBase, IStateVariables>> functions =
            new Dictionary<int, Action<IFighterBase, IStateVariables>>();
        
        public StateFunctionMapper()
        {
            //functions.Add((int)StateFunctionEnum.APPLY_Y_FORCE, BaseStateFunctions.ApplyYForce);
            functions.Add((int)StateFunctionEnum.CHANGE_STATE, BaseStateFunctions.ChangeState);
        }
    }
}