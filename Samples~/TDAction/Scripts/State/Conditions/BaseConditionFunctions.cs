using HnSF.Fighters;
using HnSF.Sample.TDAction.State;
using UnityEngine;


namespace HnSF.Sample.TDAction
{
    public class BaseConditionFunctions
    {
        public static bool NoCondition(IConditionVariables variables, HnSF.StateTimeline timeline, StateMachineContext smContext, StateFunctionContext sfContext)
        {
            return true;
        }

        public static bool MovementSqrMagnitude(IConditionVariables variables, HnSF.StateTimeline timeline, StateMachineContext smContext, StateFunctionContext sfContext)
        {
            var smc = smContext as FighterStateMachineContext;
            FighterInputManager inputManager = smc.fighter.InputManager;
            ConditionMovementMagnitude vars = (ConditionMovementMagnitude)variables;

            Vector2 movement = inputManager.GetMovement(0);
            bool r = vars.inverse ? movement.sqrMagnitude < vars.sqrMagnitude : movement.sqrMagnitude > vars.sqrMagnitude;
            return r;
        }

        public static bool FallSpeed(IConditionVariables variables, HnSF.StateTimeline timeline, StateMachineContext smContext, StateFunctionContext sfContext)
        {
            var smc = smContext as FighterStateMachineContext;
            FighterPhysicsManager physicsManager = smc.fighter.physicsManager;
            ConditionFallSpeed vars = (ConditionFallSpeed)variables;

            bool r = false;
            if (vars.inverse)
            {
                r = physicsManager.forceMovement.y < vars.minValue || physicsManager.forceMovement.y > vars.maxValue;
            }
            else
            {
                r = physicsManager.forceMovement.y >= vars.minValue && physicsManager.forceMovement.y <= vars.maxValue;
            }
            return r;
        }

        public static bool GroundedState(IConditionVariables variables, HnSF.StateTimeline timeline, StateMachineContext smContext, StateFunctionContext sfContext)
        {
            var smc = smContext as FighterStateMachineContext;
            ConditionGroundState vars = (ConditionGroundState)variables;

            bool r = smc.fighter.PhysicsManager.IsGrounded;
            if (vars.inverse) r = !r;
            return r;
        }

        public static bool CheckButton(IFighterBase fighter, IConditionVariables variables)
        {
            ConditionButton vars = (ConditionButton)variables;

            bool r = false;
            switch (vars.buttonState)
            {
                case ConditionButton.ButtonStateType.FirstPress:
                    break;
                case ConditionButton.ButtonStateType.Released:
                    break;
                case ConditionButton.ButtonStateType.IsDown:
                    break;
            }
            return r;
        }
    }
}