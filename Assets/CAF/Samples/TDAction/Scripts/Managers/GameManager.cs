using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TDAction.Managers
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private GameHandler gameHandler;
        [SerializeField] private TDAction.Entities.EntityController playerEntity;
        [SerializeField] private Vector3 playerSpawnPosition;

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