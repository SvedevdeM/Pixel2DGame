using UnityEngine;
using Vices.Scripts.Core;

namespace Vices.Scripts.Game.UI
{
    public class Menu : MonoBehaviour
    {
        [SerializeField] private string _gameScene = "Campus";

        public void StartGame()
        {
            SceneSystem.Singleton.LoadScene(_gameScene);
        }

        public void Exit()
        {
            Application.Quit();
        }
    }
}
