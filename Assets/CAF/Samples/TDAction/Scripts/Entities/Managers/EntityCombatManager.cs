using CAF.Combat;
using System.Collections;
using System.Collections.Generic;
using TDAction.Combat;
using UnityEngine;

namespace TDAction.Entities
{
    public class EntityCombatManager : CAF.Entities.EntityCombatManager
    {
        public TeamTypes team = TeamTypes.FFA;

        protected override void Awake()
        {
            hitboxManager = new EntityHitboxManager(this, (EntityManager)controller);
        }

        public override int GetTeam()
        {
            return (int)team; 
        }

        public override HitReaction Hurt(Vector3 center, Vector3 forward, Vector3 right, HitInfoBase hitInfo)
        {
            return base.Hurt(center, forward, right, hitInfo);
        }
    }
}