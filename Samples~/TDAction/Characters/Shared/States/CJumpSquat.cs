using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TDAction.Entities.Characters
{
    public class CJumpSquat : CharacterState
    {

        public override void OnUpdate()
        {
            GetCharacterController().StateManager.IncrementFrame();

            CheckInterrupt();
        }

        public override bool CheckInterrupt()
        {
            CharacterManager c = GetCharacterController();
            CharacterStats stats = (CharacterStats)c.entityDefinition.GetEntityStats();
            if (c.StateManager.CurrentStateFrame >= stats.jumpSquatFrames)
            {
                c.StateManager.ChangeState((int)CharacterStates.JUMP);
                return true;
            }
            return false;
        }
    }
}