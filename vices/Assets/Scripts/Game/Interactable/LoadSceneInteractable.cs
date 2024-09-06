using UnityEngine;
using UnityEngine.SceneManagement;
using Vices.Scripts.Core;

namespace Vices.Scripts
{
    public class LoadSceneInteractable : Interactable
    {
        [SerializeField] private string _sceneName;
        private CharacterController _characterController;

        protected override void OnEnter(Collider other)
        {
            if (!other.TryGetComponent<CharacterController>(out _characterController)) return;

            if (_sceneName == GameContext.Context.PreviousScene)
            {
                _characterController.enabled = false;
                _characterController.transform.position = GameContext.Context.PreviousTeleportPosition;
            }

            GameContext.Context.PreviousScene = SceneManager.GetActiveScene().name;

            SceneSystem.Singleton.LoadScene(_sceneName);
        }

        private void AfterLoading(CharacterController characterController)
        {
            FindObjectOfType<PlayerMovement>();
            GameContext.Context.PreviousTeleportPosition = characterController.transform.position;
        }

        protected override void OnInteract()
        {
        }
    }
}