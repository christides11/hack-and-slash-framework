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

        [SerializeField] private GameHandler gameHandler;
        [SerializeField] private TDAction.Entities.EntityManager playerEntity;
        [SerializeField] private Vector3 playerSpawnPosition;

        private void Awake()
        {
            instance = this;
        }

        /// <summary>
        /// Starts a game, creating a handler and spawning the player.
        /// </summary>
        public void SetupGame()
        {
            gameHandler = new GameHandler();

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