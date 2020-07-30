using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TDAction.Entities
{
    [CreateAssetMenu(menuName = "TDA/Entities/Definition")]
    public class EntityDefinition : ScriptableObject
    {
        [SerializeField] protected EntityManager entityPrefab;
        [SerializeField] protected EntityStats entityStats;

        public virtual EntityStats GetEntityStats()
        {
            return entityStats;
        }
    }
}