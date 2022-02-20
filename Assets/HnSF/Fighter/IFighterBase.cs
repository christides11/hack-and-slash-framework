using UnityEngine;

namespace HnSF.Fighters
{
    public interface IFighterBase
    {
        public IFighterCombatManager CombatManager { get; }
        public IFighterStateManager StateManager { get;  }
        public IFighterPhysicsManager PhysicsManager { get; }
        
        GameObject GetGameObject();
    }
}