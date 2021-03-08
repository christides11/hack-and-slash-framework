using CAF.Combat;
using System.Collections;
using System.Collections.Generic;
using TDAction.Combat;
using UnityEngine;

namespace TDAction.Entities
{
    [CreateAssetMenu(menuName = "TDA/Entities/Definition")]
    public class EntityDefinition : ScriptableObject
    {
        [System.Serializable]
        public class HurtboxDefinitionItem
        {
            public string name;
            public StateHurtboxDefinition hurtboxDefinition;
        }

        [SerializeField] protected FighterManager entityPrefab;
        [SerializeField] protected EntityStats entityStats;
        public List<Combat.MovesetDefinition> movesets = new List<Combat.MovesetDefinition>();
        public List<HurtboxDefinitionItem> hurtboxDefinitions = new List<HurtboxDefinitionItem>();

        public virtual EntityStats GetEntityStats()
        {
            return entityStats;
        }
    }
}