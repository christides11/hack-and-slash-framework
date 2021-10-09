﻿using HnSF.Combat;
using TDAction.UI;
using UnityEngine;

namespace TDAction.Managers
{
    /// <summary>
    /// Manages the whole game.
    /// mainly it sets up the game with the proper variables when it's ready,
    /// and then ticks the game every FixedUpdate.
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        public static GameManager instance;

        public GameHandler GameHandler { get { return gameHandler; } }

        [SerializeField] private GameHandler gameHandler = null;
        [SerializeField] private TDAction.Fighter.FighterManager playerEntity;
        [SerializeField] private Vector3 playerSpawnPosition;
        public Hurtbox hurtboxPrefab;

        private void Awake()
        {
            instance = this;
        }

        private void Update()
        {
            if (gameHandler != null)
            {
                gameHandler.Update();
            }
        }

        /// <summary>
        /// Starts a game, creating a handler and spawning the player.
        /// </summary>
        public void SetupGame()
        {
            gameHandler = new GameHandler(GameObject.FindObjectOfType<PlayerHUD>());
        }

        public void StartGame()
        {
            if(gameHandler == null)
            {
                return;
            }
            gameHandler.SpawnPlayer(playerEntity, playerSpawnPosition);
        }

        public void FixedUpdate()
        {
            if (gameHandler != null)
            {
                gameHandler.Tick();
            }
        }
    }
}