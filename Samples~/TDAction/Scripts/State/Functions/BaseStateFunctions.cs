namespace HnSF.Sample.TDAction
{
    public static class BaseStateFunctions
    {
        public static void ChangeState(IStateVariables variables, HnSF.StateTimeline timeline, StateMachineContext smContext, StateFunctionContext context)
        {
            var smc = smContext as FighterStateMachineContext;
            State.ChangeState vars = (State.ChangeState)variables;
            smc.fighter.StateManager.MarkForStateChange(vars.stateID, vars.stateMovesetID);
        }
        
        public static void ApplyGravity(IStateVariables variables, HnSF.StateTimeline timeline, StateMachineContext smContext, StateFunctionContext context)
        {
            var smc = smContext as FighterStateMachineContext;
            State.VarApplyGravity vars = (State.VarApplyGravity)variables;
            smc.fighter.physicsManager.ApplyGravity(vars.useMaxFallSpeedStat ? smc.fighter.statManager.maxFallSpeed : vars.maxFallSpeed, 
                vars.useGravityStat ? smc.fighter.statManager.gravity : vars.gravity);
        }

        public static void ApplyTraction(IStateVariables variables, HnSF.StateTimeline timeline, StateMachineContext smContext, StateFunctionContext context)
        {
            var smc = smContext as FighterStateMachineContext;
            State.VarApplyTraction vars = (State.VarApplyTraction)variables;
            if (vars.useTractionStat)
            {
                smc.fighter.physicsManager.ApplyFriction(vars.aerialTraction ? smc.fighter.statManager.aerialTraction : smc.fighter.statManager.groundTraction);
            }
            else
            {
                smc.fighter.physicsManager.ApplyFriction(vars.traction);
            }
        }

        public static void SetFallSpeed(IStateVariables variables, HnSF.StateTimeline timeline, StateMachineContext smContext, StateFunctionContext context)
        {
            var smc = smContext as FighterStateMachineContext;
            State.VarSetFallSpeed vars = (State.VarSetFallSpeed)variables;
            smc.fighter.physicsManager.SetFallSpeed(vars.value);
        }
    }
}