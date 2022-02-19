using System.Collections;
using System.Collections.Generic;
using HnSF.Fighters;
using UnityEngine;

namespace HnSF.Sample.TDAction
{
    public class FighterManager : MonoBehaviour, IFighterBase
    {
        public IFighterCombatManager combatManager
        {
            get { return _combatManager; }
        }
        public IFighterStateManager stateManager
        {
            get { return _stateManager; }
        }
        public IFighterPhysicsManager physicsManager
        {
            get { return _physicsManager; }
        }

        [SerializeField] private FighterStateManager _stateManager;
        [SerializeField] private FighterCombatManager _combatManager;
        [SerializeField] private FighterPhysicsManager _physicsManager;

        public SpriteRenderer visual;
        
        public GameObject GetGameObject()
        {
            return gameObject;
        }
    }
}