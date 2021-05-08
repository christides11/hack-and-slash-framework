using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TDAction.Managers
{
    /// <summary>
    /// Handles initializing the game. 
    /// This is the first script that runs when the game is started.
    /// </summary>
    public class BootLoader : MonoBehaviour
    {
        public delegate void EmptyAction();
        public static event EmptyAction OnPlaySceneLoaded;

        [SerializeField] private string playScene;
        [SerializeField] private GameManager gameManager;

        private void Awake()
        {
            gameManager.SetupGame();

            OnPlaySceneLoaded += StartGame;
            // Play scene is already loaded.
            if (SceneManager.GetSceneByName(playScene).IsValid())
            {
                OnPlaySceneLoaded.Invoke();
                return;
            }
            StartCoroutine(LoadPlayScene());
        }

        private void StartGame()
        {
            gameManager.StartGame();
        }

        /// <summary>
        /// Loads the play scene and sets it as the active one.
        /// </summary>
        /// <returns></returns>
        IEnumerator LoadPlayScene()
        {
            var ls = SceneManager.LoadSceneAsync(playScene, LoadSceneMode.Additive);

            while (!ls.isDone)
            {
                yield return null;
            }
            yield return null;
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(playScene));

            OnPlaySceneLoaded.Invoke();
        }
    }
}