using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TDAction.Entities
{
    public abstract class EntityDefinition : ScriptableObject
    {
        public EntityManager entityPrefab;

        public abstract EntityStats GetEntityStats();
    }
}