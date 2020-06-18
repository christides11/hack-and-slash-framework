using System.Collections;
using System.Collections.Generic;
using TDAction.Entities;
using TDAction.Simulation;
using UnityEngine;

namespace TDAction.Managers
{
    [System.Serializable]
    public class GameHandler
    {
        public SimObjectManager simulationObjectManager;
        public CAF.Simulation.TimeStepManager timeStepManager;

        [SerializeField] private EntityController currentPlayerEntity;

        public GameHandler()
        {
            simulationObjectManager = new SimObjectManager();
            timeStepManager = new CAF.Simulation.TimeStepManager(60.0f, 1.0f, 120.0f, 30.0f);
            timeStepManager.OnUpdate += Tick;
        }

        public virtual void FixedUpdate()
        {
            timeStepManager.ManualUpdate(Time.fixedDeltaTime);
        }

        public void SpawnPlayer(EntityController entity, Vector3 spawnPosition)
        {
            currentPlayerEntity = GameObject.Instantiate(entity.gameObject, spawnPosition, Quaternion.identity)
                .GetComponent<EntityController>();
            currentPlayerEntity.Initialize(CAF.Input.InputControlType.Direct);

            simulationObjectManager.RegisterObject(currentPlayerEntity);
        }

        public virtual void Tick(float dt)
        {
            TickUpdate(dt);
            TickLateUpdate(dt);
        }

        protected virtual void TickUpdate(float dt)
        {
            simulationObjectManager.Update(dt);
        }

        protected virtual void TickLateUpdate(float dt)
        {
            simulationObjectManager.LateUpdate(dt);
        }
    }
}