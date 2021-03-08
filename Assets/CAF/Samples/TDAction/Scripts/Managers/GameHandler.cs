using TDAction.Camera;
using TDAction.Entities;
using TDAction.Simulation;
using TDAction.UI;
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

        [SerializeField] private FighterManager currentPlayerEntity;
        [SerializeField] private PlayerHUD playerHUD;
        [SerializeField] private bool frameByFrameMode;

        public GameHandler(PlayerHUD playerHUD)
        {
            simulationObjectManager = new SimObjectManager();
            this.playerHUD = playerHUD;
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
            simulationObjectManager.Update();
            simulationObjectManager.LateUpdate();
        }

        /// <summary>
        /// Spawns the player in the simulation and attaches the camera to them.
        /// </summary>
        /// <param name="entity">The entity to spawn that the player controls.</param>
        /// <param name="spawnPosition">The position to spawn them at.</param>
        public void SpawnPlayer(FighterManager entity, Vector3 spawnPosition)
        {
            currentPlayerEntity = GameObject.Instantiate(entity.gameObject, spawnPosition, Quaternion.identity)
                .GetComponent<FighterManager>();
            currentPlayerEntity.Initialize(CAF.Input.InputControlType.Direct);

            simulationObjectManager.RegisterObject(currentPlayerEntity);

            GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraHandler>().SetLookAtTarget(currentPlayerEntity.transform);

            if (playerHUD)
            {
                currentPlayerEntity.GetComponent<HealthManager>().OnHurt += (source, oldHealth, currentHealth) 
                    => { playerHUD.SetHealthValue(currentHealth, source.MaxHealth); };
            }
        }
    }
}