using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TDAction.Fighter
{
    public class ADVAttack : FighterStateAttack
    {
        public override bool CheckInterrupt()
        {
            FighterManager entityManager = GetEntityManager();
            if (entityManager.TryAttack())
            {
                return true;
            }
            if (entityManager.StateManager.CurrentStateFrame >
                entityManager.CombatManager.CurrentAttack.attackDefinition.length)
            {
                if (entityManager.PhysicsManager.IsGrounded)
                {
                    entityManager.StateManager.ChangeState((int)FighterStates.IDLE);
                }
                else
                {
                    entityManager.StateManager.ChangeState((int)FighterStates.FALL);
                }
                Manager.CombatManager.Cleanup();
                return true;
            }
            return false;
        }
    }
}