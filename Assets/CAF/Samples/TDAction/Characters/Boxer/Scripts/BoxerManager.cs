using CAF.Input;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TDAction.Entities.Characters.Boxer
{
    public class BoxerManager : CharacterManager
    {
        public override void Initialize(InputControlType controlType)
        {
            base.Initialize(controlType);
            CombatManager.SetMoveset(entityDefinition.movesets[0]);
        }

        protected override void SetupStates()
        {
            StateManager.AddState(new BIdle(), (int)CharacterStates.IDLE);
            StateManager.AddState(new BWalk(), (int)CharacterStates.WALK);
            StateManager.AddState(new CJumpSquat(), (int)CharacterStates.JUMP_SQUAT);
            StateManager.AddState(new CJump(), (int)CharacterStates.JUMP);
            StateManager.AddState(new BFall(), (int)CharacterStates.FALL);
            StateManager.AddState(new CRun(), (int)CharacterStates.RUN);
            StateManager.AddState(new BAttack(), (int)CharacterStates.ATTACK);

            StateManager.ChangeState((int)CharacterStates.FALL);
        }
    }
}