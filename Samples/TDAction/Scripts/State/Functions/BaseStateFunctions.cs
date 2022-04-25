using System.Collections;
using System.Collections.Generic;
using HnSF.Fighters;
using UnityEngine;

namespace HnSF.Sample.TDAction
{
    public static class BaseStateFunctions
    {
        public static void ChangeState(IFighterBase fighter, IStateVariables variables)
        {
            State.ChangeState vars = (State.ChangeState)variables;
            fighter.StateManager.MarkForStateChange(vars.stateMovesetID, vars.stateID);
        }
        
        public static void ApplyGravity(IFighterBase fighter, IStateVariables variables)
        {
            State.VarApplyGravity vars = (State.VarApplyGravity)variables;
            ((FighterPhysicsManager)fighter.PhysicsManager).ApplyGravity(vars.maxFallSpeed, vars.gravity);
        }
    }
}