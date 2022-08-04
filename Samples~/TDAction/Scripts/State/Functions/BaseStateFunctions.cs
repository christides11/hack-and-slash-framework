using System.Collections;
using System.Collections.Generic;
using HnSF.Fighters;
using UnityEngine;

namespace HnSF.Sample.TDAction
{
    public static class BaseStateFunctions
    {
        public static void ChangeState(IFighterBase fighter, IStateVariables variables, HnSF.StateTimeline arg3, int arg4)
        {
            State.ChangeState vars = (State.ChangeState)variables;
            fighter.StateManager.MarkForStateChange(vars.stateID, vars.stateMovesetID);
        }
        
        public static void ApplyGravity(IFighterBase fighter, IStateVariables variables, HnSF.StateTimeline arg3, int arg4)
        {
            State.VarApplyGravity vars = (State.VarApplyGravity)variables;
            FighterManager fm = (FighterManager)fighter;
            fm.physicsManager.ApplyGravity(vars.useMaxFallSpeedStat ? fm.statManager.maxFallSpeed : vars.maxFallSpeed, 
                vars.useGravityStat ? fm.statManager.gravity : vars.gravity);
        }

        public static void ApplyTraction(IFighterBase fighter, IStateVariables variables, HnSF.StateTimeline arg3, int arg4)
        {
            State.VarApplyTraction vars = (State.VarApplyTraction)variables;
            FighterManager fm = (FighterManager)fighter;
            if (vars.useTractionStat)
            {
                fm.physicsManager.ApplyFriction(vars.aerialTraction ? fm.statManager.aerialTraction : fm.statManager.groundTraction);
            }
            else
            {
                fm.physicsManager.ApplyFriction(vars.traction);
            }
        }

        public static void SetFallSpeed(IFighterBase fighter, IStateVariables variables, HnSF.StateTimeline arg3, int arg4)
        {
            State.VarSetFallSpeed vars = (State.VarSetFallSpeed)variables;
            FighterManager fm = (FighterManager)fighter;
            fm.physicsManager.SetFallSpeed(vars.value);
        }
    }
}