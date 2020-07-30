using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TDAction.Managers
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager instance;

        public GameHandler GameHandler { get { return gameHandler; } }

        [SerializeField] private GameHandler gameHandler;
        [SerializeField] private TDAction.Entities.EntityManager playerEntity;
        [SerializeField] private Vector3 playerSpawnPosition;

        private void Awake()
        {
            instance = this;
        }

        public void SetupGame()
        {
            gameHandler = new GameHandler();

            gameHandler.SpawnPlayer(playerEntity, playerSpawnPosition);
        }

        public void FixedUpdate()
        {
            if (gameHandler != null)
            {
                gameHandler.FixedUpdate();
            }
        }
    }
}