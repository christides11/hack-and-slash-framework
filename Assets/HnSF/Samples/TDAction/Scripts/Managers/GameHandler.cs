using TDAction.Camera;
using TDAction.Fighter;
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
        [SerializeReference] public SimObjectManager simulationObjectManager;

        [SerializeField] private FighterManager currentPlayerEntity;
        [SerializeField] private PlayerHUD playerHUD;
        [SerializeField] private bool frameByFrameMode;

        public float damageValue = 10;

        public GameHandler(PlayerHUD playerHUD)
        {
            simulationObjectManager = new SimObjectManager();
            this.playerHUD = playerHUD;
        }

        public virtual void Update()
        {
            if (Input.GetKeyDown(KeyCode.F1))
            {
                frameByFrameMode = !frameByFrameMode;
            }
            if (frameByFrameMode)
            {
                if (Input.GetKeyDown(KeyCode.F2))
                {
                    UpdateSimulation();
                }
            }

            if (Input.GetKeyDown(KeyCode.F6))
            {
                currentPlayerEntity.GetComponent<HealthManager>().Hurt(damageValue);
            }
            if (Input.GetKeyDown(KeyCode.F7))
            {
                currentPlayerEntity.GetComponent<HealthManager>().Heal(damageValue);
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
            simulationObjectManager.Tick();
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
            currentPlayerEntity.Initialize(HnSF.Input.FighterControlType.Player);

            simulationObjectManager.RegisterObject(currentPlayerEntity);

            GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraHandler>().SetLookAtTarget(currentPlayerEntity.visual.transform);

            if (playerHUD)
            {
                currentPlayerEntity.GetComponent<HealthManager>().OnHurt += (source, oldHealth, currentHealth) 
                    => { playerHUD.Damage(currentHealth, source.MaxHealth); };
                currentPlayerEntity.GetComponent<HealthManager>().OnHeal += (source, oldHealth, currentHealth)
                    => { playerHUD.Heal(currentHealth, source.MaxHealth); };
            }
        }
    }
}