using System.Collections;
using System.Collections.Generic;
using TDAction.Entities.States;
using UnityEngine;

namespace TDAction.Entities.Characters
{
    public class CharacterManager : EntityManager
    {


        protected override void SetupStates()
        {
            base.SetupStates();

            StateManager.AddState(new EntityStateFlinchAir(), (int)EntityStates.FLINCH_AIR);
            StateManager.AddState(new EntityStateFlinchGround(), (int)EntityStates.FLINCH_GROUND);
            StateManager.AddState(new EntityStateTumble(), (int)EntityStates.TUMBLE);
            StateManager.AddState(new EntityStateKnockdown(), (int)EntityStates.KNOCKDOWN);
        }
    }
}