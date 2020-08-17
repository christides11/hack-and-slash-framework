using CAF.Combat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TDAction.Entities
{
    public class EntityHitboxManager : CAF.Entities.EntityHitboxManager
    {
        public EntityHitboxManager(EntityCombatManager combatManager, EntityManager manager) 
            : base(combatManager, manager)
        {
            
        }

        protected override Hitbox InstantiateHitbox(Vector3 position, Quaternion rotation)
        {
            return GameObject.Instantiate(((EntityManager)manager).hitboxPrefab, position, rotation);
        }
    }
}
