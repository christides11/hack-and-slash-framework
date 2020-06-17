using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TDAction.Managers
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private GameHandler gameHandler;

        public void SetupGame()
        {
            gameHandler = new GameHandler();
        }
    }
}