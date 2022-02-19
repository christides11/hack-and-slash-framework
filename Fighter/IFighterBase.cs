using UnityEngine;

namespace HnSF.Fighters
{
    public interface IFighterBase
    {
        public IFighterCombatManager combatManager { get; }
        public IFighterStateManager stateManager { get;  }
        public IFighterPhysicsManager physicsManager { get; }
        
        GameObject GetGameObject();
    }
}