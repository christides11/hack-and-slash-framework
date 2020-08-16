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
            EntityManager entityManager = GetEntityController();
            if (entityManager.TryAttack())
            {
                return true;
            }
            if (entityManager.StateManager.CurrentStateFrame >
                entityManager.CombatManager.CurrentAttack.attackDefinition.length)
            {
                if (entityManager.IsGrounded)
                {
                    entityManager.StateManager.ChangeState((int)CharacterStates.IDLE);
                }
                else
                {
                    entityManager.StateManager.ChangeState((int)CharacterStates.FALL);
                }
                Controller.CombatManager.Cleanup();
                return true;
            }
            return false;
        }
    }
}