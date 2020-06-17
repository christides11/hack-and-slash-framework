using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TDAction.Managers
{
    public class BootLoader : MonoBehaviour
    {
        public delegate void EmptyAction();
        public static event EmptyAction OnPlaySceneLoaded;

        [SerializeField] private string playScene;
        [SerializeField] private GameManager gameManager;

        private void Awake()
        {
            OnPlaySceneLoaded += StartGame;

            if (SceneManager.GetSceneByName(playScene).IsValid())
            {
                OnPlaySceneLoaded.Invoke();
                return;
            }
            StartCoroutine(LoadPlayScene());
        }

        private void StartGame()
        {
            gameManager.SetupGame();
        }

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