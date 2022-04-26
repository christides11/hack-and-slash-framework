using HnSF.Fighters;
using HnSF.Sample.TDAction.State;
using UnityEngine;

namespace HnSF.Sample.TDAction
{
    public class BaseConditionFunctions
    {
        public static bool NoCondition(IFighterBase fighter, IConditionVariables variables)
        {
            return true;
        }

        public static bool MovementSqrMagnitude(IFighterBase fighter, IConditionVariables variables)
        {
            FighterInputManager inputManager = ((FighterManager)fighter).InputManager;
            ConditionMovementMagnitude vars = (ConditionMovementMagnitude)variables;

            Vector2 movement = inputManager.GetMovement(0);
            bool r = vars.inverse ? movement.sqrMagnitude < vars.sqrMagnitude : movement.sqrMagnitude > vars.sqrMagnitude;
            return r;
        }

        public static bool FallSpeed(IFighterBase fighter, IConditionVariables variables)
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

        public static bool GroundedState(IFighterBase fighter, IConditionVariables variables)
        {
            ConditionGroundState vars = (ConditionGroundState)variables;

            bool r = fighter.PhysicsManager.IsGrounded;
            if (vars.inverse) r = !r;
            return r;
        }
    }
}