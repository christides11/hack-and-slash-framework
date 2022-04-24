using System.Collections;
using System.Collections.Generic;
using HnSF.Fighters;
using UnityEngine;

namespace HnSF.Sample.TDAction
{
    public class FighterManager : SimulationBehaviour, IFighterBase
    {
        public IFighterCombatManager CombatManager
        {
            get { return _combatManager; }
        }
        public IFighterStateManager StateManager
        {
            get { return _stateManager; }
        }
        public IFighterPhysicsManager PhysicsManager
        {
            get { return _physicsManager; }
        }
        
        public FighterInputManager inputManager
        {
            get { return inputManager; }
        }

        [SerializeField] private FighterStateManager _stateManager;
        [SerializeField] private FighterCombatManager _combatManager;
        [SerializeField] private FighterPhysicsManager _physicsManager;
        [SerializeField] private FighterInputManager _inputManager;

        public FighterDefinition definition;
        public SpriteRenderer visual;

        public virtual void ResetPreview()
        {
            transform.position = Vector3.zero;
            transform.eulerAngles = Vector3.zero;
        }
        
        public override void SimUpdate()
        {
            base.SimUpdate();
            if (CombatManager.HitStop == 0)
            {
                _physicsManager.CheckIfGrounded();
                _stateManager.Tick();
                PhysicsManager.Tick();
            }
            else
            {
                
            }
        }

        public GameObject GetGameObject()
        {
            return gameObject;
        }
        
    }
}