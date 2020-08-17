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
            StateManager.AddState(new BIdle(), (int)EntityStates.IDLE);
            StateManager.AddState(new BWalk(), (int)EntityStates.WALK);
            StateManager.AddState(new CJumpSquat(), (int)EntityStates.JUMP_SQUAT);
            StateManager.AddState(new CJump(), (int)EntityStates.JUMP);
            StateManager.AddState(new BFall(), (int)EntityStates.FALL);
            StateManager.AddState(new CRun(), (int)EntityStates.RUN);
            StateManager.AddState(new BAttack(), (int)EntityStates.ATTACK);

            StateManager.ChangeState((int)EntityStates.FALL);
        }
    }
}