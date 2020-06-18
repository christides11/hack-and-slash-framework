using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TDAction.Entities.Characters
{
    public class CharacterState : EntityState
    {

        public virtual CharacterManager GetCharacterController()
        {
            return (CharacterManager)Controller;
        }
    }
}