using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TDAction.Fighter
{
    public class ADVAttack : FighterStateAttack
    {
        public override bool CheckInterrupt()
        {
            if (Manager.TryAttack())
            {
                return true;
            }
            if (Manager.StateManager.CurrentStateFrame >
                Manager.CombatManager.CurrentAttackNode.attackDefinition.length)
            {
                if (Manager.PhysicsManager.IsGrounded)
                {
                    Manager.StateManager.ChangeState((int)FighterStates.IDLE);
                }
                else
                {
                    Manager.StateManager.ChangeState((int)FighterStates.FALL);
                }
                Manager.CombatManager.Cleanup();
                return true;
            }
            return false;
        }
    }
}