using System.Collections;
using System.Collections.Generic;
using TDAction.Combat;
using UnityEngine;

namespace TDAction.Entities
{
    [CreateAssetMenu(menuName = "TDA/Entities/Definition")]
    public class EntityDefinition : ScriptableObject
    {
        [SerializeField] protected EntityManager entityPrefab;
        [SerializeField] protected EntityStats entityStats;
        [SerializeField] protected List<MovesetDefinition> movesets = new List<MovesetDefinition>();

        public virtual EntityStats GetEntityStats()
        {
            return entityStats;
        }
    }
}