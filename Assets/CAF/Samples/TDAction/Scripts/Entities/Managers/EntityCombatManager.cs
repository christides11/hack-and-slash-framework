using CAF.Combat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TDAction.Entities
{
    public class EntityCombatManager : CAF.Entities.EntityCombatManager
    {

        public override HitReaction Hurt(Vector3 center, Vector3 forward, Vector3 right, HitInfoBase hitInfo)
        {
            return base.Hurt(center, forward, right, hitInfo);
        }
    }
}