using System;
using System.Collections;
using System.Collections.Generic;
using HnSF.Fighters;
using UnityEngine;

namespace HnSF.Sample.TDAction
{
    public class FighterManager : SimulationBehaviour, IFighterBase
    {
        public IFighterCombatManager CombatManager => combatManager;
        public IFighterStateManager StateManager => stateManager;
        public IFighterPhysicsManager PhysicsManager => physicsManager;
        public FighterInputManager InputManager => inputManager;
        public FighterStatManager StatManager => statManager;

        public FighterStateManager stateManager;
        public FighterCombatManager combatManager;
        public FighterPhysicsManager physicsManager;
        public FighterInputManager inputManager;
        public FighterStatManager statManager;

        public FighterDefinition definition;
        public SpriteRenderer visual;

        public void Awake()
        {
            foreach (MovesetDefinition md in definition.movesets)
            {
                md.Initialize();
            }
        }

        public virtual void Start()
        {
            
        }
        
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
                physicsManager.CheckIfGrounded();
                stateManager.Tick();
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