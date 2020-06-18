using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TDAction.Entities.Characters
{
    [CreateAssetMenu(menuName = "TDA/Characters/Definition")]
    public class CharacterDefinition : EntityDefinition
    {
        public CharacterStats characterStats;

        public override EntityStats GetEntityStats()
        {
            return characterStats;
        }

        public virtual CharacterStats GetCharacterStats()
        {
            return characterStats;
        }
    }
}