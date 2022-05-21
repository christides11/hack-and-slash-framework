using HnSF.Fighters;
using HnSF.Sample.TDAction.State;
using UnityEngine;

namespace HnSF.Sample.TDAction
{
    public class BaseConditionFunctions
    {
        public static bool NoCondition(IFighterBase fighter, IConditionVariables variables, HnSF.StateTimeline arg3, int arg4)
        {
            return true;
        }

        public static bool MovementSqrMagnitude(IFighterBase fighter, IConditionVariables variables, HnSF.StateTimeline arg3, int arg4)
        {
            FighterInputManager inputManager = ((FighterManager)fighter).InputManager;
            ConditionMovementMagnitude vars = (ConditionMovementMagnitude)variables;

            Vector2 movement = inputManager.GetMovement(0);
            bool r = vars.inverse ? movement.sqrMagnitude < vars.sqrMagnitude : movement.sqrMagnitude > vars.sqrMagnitude;
            return r;
        }

        public static bool FallSpeed(IFighterBase fighter, IConditionVariables variables, HnSF.StateTimeline arg3, int arg4)
        {
            FighterPhysicsManager physicsManager = ((FighterManager)fighter).physicsManager;
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

        public static bool GroundedState(IFighterBase fighter, IConditionVariables variables, HnSF.StateTimeline arg3, int arg4)
        {
            ConditionGroundState vars = (ConditionGroundState)variables;

            bool r = fighter.PhysicsManager.IsGrounded;
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