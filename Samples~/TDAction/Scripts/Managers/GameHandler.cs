using TDAction.Camera;
using TDAction.Entities;
using TDAction.Simulation;
using UnityEngine;

namespace TDAction.Managers
{
    /// <summary>
    /// This script handles anything that the game needs to run.
    /// The main thing it does is update the simulation with the
    /// correct delta time.
    /// </summary>
    [System.Serializable]
    public class GameHandler
    {
        public SimObjectManager simulationObjectManager;

        [SerializeField] private EntityManager currentPlayerEntity;
        [SerializeField] private bool frameByFrameMode;

        public GameHandler()
        {
            simulationObjectManager = new SimObjectManager();
        }

        public virtual void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                frameByFrameMode = !frameByFrameMode;
            }
            if (frameByFrameMode)
            {
                if (Input.GetKeyDown(KeyCode.Alpha2))
                {
                    UpdateSimulation();
                }
            }
        }

        /// <summary>
        /// Ticks the game. This should be called from FixedUpdate.
        /// </summary>
        public virtual void Tick()
        {
            if (frameByFrameMode)
            {
                return;
            }
            UpdateSimulation();
        }

        private void UpdateSimulation()
        {
            simulationObjectManager.Update(Time.fixedDeltaTime);
            simulationObjectManager.LateUpdate(Time.fixedDeltaTime);
        }

        /// <summary>
        /// Spawns the player in the simulation and attaches the camera to them.
        /// </summary>
        /// <param name="entity">The entity to spawn that the player controls.</param>
        /// <param name="spawnPosition">The position to spawn them at.</param>
        public void SpawnPlayer(EntityManager entity, Vector3 spawnPosition)
        {
            currentPlayerEntity = GameObject.Instantiate(entity.gameObject, spawnPosition, Quaternion.identity)
                .GetComponent<EntityManager>();
            currentPlayerEntity.Initialize(CAF.Input.InputControlType.Direct);

            simulationObjectManager.RegisterObject(currentPlayerEntity);

            GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraHandler>().SetFollowTarget(currentPlayerEntity.transform);
        }
    }
}