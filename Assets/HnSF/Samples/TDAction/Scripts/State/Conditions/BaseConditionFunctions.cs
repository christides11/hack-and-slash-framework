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
    }
}