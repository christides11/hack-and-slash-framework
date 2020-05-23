using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CAF.Entities
{
    /// <summary>
    /// Handles the hitboxes and other boxes used by entities for combat.
    /// </summary>
    public class EntityHitboxManager
    {
        private EntityCombatManager combatManager;
        private EntityController controller;

        public EntityHitboxManager(EntityCombatManager combatManager, EntityController controller)
        {
            this.combatManager = combatManager;
            this.controller = controller;
        }
    }
}