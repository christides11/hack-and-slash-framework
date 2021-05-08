using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TDAction.Fighter
{
    public class FighterState : HnSF.Fighters.FighterState
    {
        public virtual FighterManager GetEntityManager()
        {
            return (FighterManager)Manager;
        }

        public virtual FighterPhysicsManager GetPhysicsManager()
        {
            return (FighterPhysicsManager)Manager.PhysicsManager;
        }

        public virtual FighterStatsManager GetStatsManager()
        {
            return (Manager as FighterManager).statManager;
        }
    }
}