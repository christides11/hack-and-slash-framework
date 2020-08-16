using CAF.Combat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TDAction.Entities
{
    public class EntityHitboxManager : CAF.Entities.EntityHitboxManager
    {
        public EntityHitboxManager(EntityCombatManager combatManager, EntityManager controller) 
            : base(combatManager, controller)
        {
            
        }
    }
}
