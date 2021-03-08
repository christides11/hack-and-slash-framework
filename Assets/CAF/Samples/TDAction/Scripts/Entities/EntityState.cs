using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TDAction.Entities
{
    public class EntityState : CAF.Fighters.FighterState
    {
        public virtual FighterManager GetEntityManager()
        {
            return (FighterManager)Manager;
        }

        public virtual EntityPhysicsManager GetPhysicsManager()
        {
            return (EntityPhysicsManager)Manager.PhysicsManager;
        }
    }
}