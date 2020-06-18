using TDAction.Entities;
using TDAction.Simulation;
using UnityEngine;

namespace TDAction.Managers
{
    [System.Serializable]
    public class GameHandler
    {
        public SimObjectManager simulationObjectManager;

        [SerializeField] private EntityController currentPlayerEntity;

        public GameHandler()
        {
            simulationObjectManager = new SimObjectManager();
        }

        public virtual void FixedUpdate()
        {
            simulationObjectManager.Update(Time.fixedDeltaTime);
            simulationObjectManager.LateUpdate(Time.fixedDeltaTime);
        }

        public void SpawnPlayer(EntityController entity, Vector3 spawnPosition)
        {
            currentPlayerEntity = GameObject.Instantiate(entity.gameObject, spawnPosition, Quaternion.identity)
                .GetComponent<EntityController>();
            currentPlayerEntity.Initialize(CAF.Input.InputControlType.Direct);

            simulationObjectManager.RegisterObject(currentPlayerEntity);
        }
    }
}