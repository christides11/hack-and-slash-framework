using CAF.Simulation;
using System.Collections;
using System.Collections.Generic;
using TDAction.Entities;
using UnityEngine;

namespace TDAction.Managers
{
    [System.Serializable]
    public class GameHandler
    {
        public SimObjectManager simulationObjectManager;
        public TimeStepManager timeStepManager;

        [SerializeField] private EntityController currentPlayerEntity;

        public GameHandler()
        {
            simulationObjectManager = new SimObjectManager();
            timeStepManager = new TimeStepManager(60.0f, 1.0f, 120.0f, 30.0f);
        }

        public void SpawnPlayer(EntityController entity, Vector3 spawnPosition)
        {
            currentPlayerEntity = GameObject.Instantiate(entity.gameObject, spawnPosition, Quaternion.identity)
                .GetComponent<EntityController>();

            currentPlayerEntity.Initialize(CAF.Input.InputControlType.Direct);
        }
    }
}