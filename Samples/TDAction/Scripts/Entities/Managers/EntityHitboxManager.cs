using CAF.Combat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TDAction.Entities
{
    public class EntityHitboxManager : CAF.Fighters.FighterHitboxManager
    {
        public EntityHitboxManager(EntityCombatManager combatManager, FighterManager manager) 
            : base(combatManager, manager)
        {
            
        }

        protected override HitboxBase InstantiateHitbox(BoxDefinitionBase hitboxDefinitionBase)
        {
            return GameObject.Instantiate(((FighterManager)manager).hitboxPrefab, 
                GetHitboxPosition(hitboxDefinitionBase), 
                GetHitboxRotation(hitboxDefinitionBase));
        }

        protected virtual Vector3 GetHitboxPosition(BoxDefinitionBase hitboxDefinitionBase)
        {
            BoxDefinition hitboxDefinition = (BoxDefinition)hitboxDefinitionBase;
            return manager.transform.position
                + manager.GetVisualBasedDirection(Vector3.right) * hitboxDefinition.offset.x * ((FighterManager)manager).FaceDirection
                + manager.GetVisualBasedDirection(Vector3.up) * hitboxDefinition.offset.y;
        }

        protected virtual Quaternion GetHitboxRotation(BoxDefinitionBase hitboxDefinitionBase)
        {
            BoxDefinition hitboxDefinition = (BoxDefinition)hitboxDefinitionBase;
            return Quaternion.Euler(new Vector3(0, 0, manager.visual.transform.localScale.x < 0 ? 180 : 0)
                + hitboxDefinition.rotation);
        }
    }
}
