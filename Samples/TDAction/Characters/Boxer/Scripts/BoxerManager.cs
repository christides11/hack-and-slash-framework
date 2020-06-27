using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TDAction.Entities.Characters.Boxer
{
    public class BoxerManager : CharacterManager
    {

        protected override void SetupStates()
        {
            StateManager.AddState(new BIdle(), (int)CharacterStates.IDLE);
            StateManager.AddState(new BFall(), (int)CharacterStates.FALL);
            StateManager.AddState(new BWalk(), (int)CharacterStates.WALK);

            StateManager.ChangeState((int)CharacterStates.FALL);
        }
    }
}