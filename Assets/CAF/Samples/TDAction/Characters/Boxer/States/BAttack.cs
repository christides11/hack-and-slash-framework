using System.Collections;
using System.Collections.Generic;
using TDAction.Entities.States;
using UnityEngine;

namespace TDAction.Entities.Characters.Boxer
{
    public class BAttack : EntityStateAttack
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
                if (entityManager.IsGrounded)
                {
                    entityManager.StateManager.ChangeState((int)EntityStates.IDLE);
                }
                else
                {
                    entityManager.StateManager.ChangeState((int)EntityStates.FALL);
                }
                Manager.CombatManager.Cleanup();
                return true;
            }
            return false;
        }
    }
}